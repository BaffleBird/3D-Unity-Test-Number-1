using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerUpperSM : StateMachine
{
	//Reference IK Fields
	[Header("IK Stuff")]
	[SerializeField] RigBuilder _rigBuilder = null;
	public RigBuilder rigBuilder => _rigBuilder;

	[SerializeField] Rig _animationRig = null;
	public Rig animationRig => _animationRig;

	[SerializeField] GameObject _aimTarget = null;
	public GameObject aimTarget => _aimTarget;

	[SerializeField] GameObject _aimTarget2 = null;
	public GameObject bodyAimTarget => _aimTarget2;

	[SerializeField] GameObject _trueTarget = null;
	public GameObject trueTarget => _trueTarget;

	[SerializeField] Transform _gunBarrel = null;
	public Transform gunBarrel => _gunBarrel;

	[SerializeField] Animator _gunAnimator = null;
	public Animator gunAnimator => _gunAnimator;

	protected override void Awake()
	{
		base.Awake();

		States.Add("StandyBy", new PlayerUpper_StandyByState("StandyBy", this, this));
		States.Add("Shoot", new PlayerUpper_ShootState("Shoot", this, this));
		States.Add("Charge", new PlayerUpper_ChargeState("Charge", this, this));
		States.Add("Cannon", new PlayerUpper_CannonState("Cannon", this, this));

		currentState = States["StandyBy"];
		_previousState = currentState.StateName;
		currentState.StartState();
	}

	protected override void Update()
	{
		base.Update();
		TextUpdate.Instance.SetText("Gun State", currentState.StateName);
	}

	protected override void FixedUpdate()
	{
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		myStatus.hitNormal = hit.normal;
	}
}

public class PlayerUpper_StandyByState : State
{
	public PlayerUpperSM USM;
	Vector3 targetPoint;
	float targetSpeed = 20f;

	public PlayerUpper_StandyByState(string name, StateMachine stateMachine, PlayerUpperSM upperStateMachine) : base(name, stateMachine) 
	{
		USM = upperStateMachine;
	}

	public override void StartState()
	{
		targetPoint = SM.transform.position + (SM.transform.forward * 2f);
	}

	public override void UpdateState()
	{

		USM.aimTarget.transform.position = Vector3.Lerp(USM.aimTarget.transform.position, targetPoint, Time.deltaTime * targetSpeed);
		if (USM.animationRig.weight > 0)
		{
			USM.animationRig.weight = Mathf.Lerp(USM.animationRig.weight, 0, 0.1f);
			if (USM.animationRig.weight < 0.01f)
			{
				USM.animationRig.weight = 0;
				USM.rigBuilder.enabled = false;
			}
		}

		if (SM.myStatus.currentState != "Dodge" && SM.myStatus.currentState != "Sprint")
		{
			if (SM.myInputs.GetInput("Shoot") && SM.myStatus.GetCooldown("Laser"))
				SM.SwitchState("Shoot");
			else if (SM.myInputs.GetInput("Charge") && SM.myStatus.GetCooldown("Laser"))
				SM.SwitchState("Charge");
		}
		
		
	}

	public override Vector3 MotionUpdate()
	{
		return Vector3.zero;
	}

	public override void EndState()
	{
	}
}

public class PlayerUpper_ShootState : State
{
	public PlayerUpperSM USM; //Reference IK components through here
	RaycastHit hit;
	float rayDistance = 500;

	Vector3 targetPoint;
	float targetSpeed = 15f;

	float maxAngle = 145;
	bool entered_deadzone = false;
	float crossEntry = 0;
	float rotaryAngle = 0;

	public PlayerUpper_ShootState(string name, StateMachine stateMachine, PlayerUpperSM upperStateMachine) : base(name, stateMachine) 
	{
		USM = upperStateMachine;
	}

	public override void StartState()
	{
		//Activate IK components
		USM.rigBuilder.enabled = true;
		//Trigger Animation Controller
		SM.myAnimator.SetBool("Shoot", true);
		USM.gunAnimator.SetBool("Shooting", true);
		targetPoint = SM.transform.position + (SM.transform.forward * 2f);
	}

	public override void UpdateState()
	{
		if (USM.animationRig.weight < 1)
		{
			USM.animationRig.weight = Mathf.Lerp(USM.animationRig.weight, 1f, 0.1f);
			if (USM.animationRig.weight > 0.99f) USM.animationRig.weight = 1f;
		}

		//Raycast from camera and/or the gun and set the target
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		LayerMask mask = LayerMask.GetMask("default");
		mask |= (1 << LayerMask.NameToLayer("Terrain"));

		if (Physics.Raycast(ray, out hit, rayDistance, mask))
		{
			targetPoint = hit.point - USM.gunBarrel.position;
			USM.trueTarget.transform.position = hit.point;
		}
		else
		{
			targetPoint = (Camera.main.transform.position + Camera.main.transform.forward * rayDistance) - USM.gunBarrel.position;
			USM.trueTarget.transform.position = Camera.main.transform.position + Camera.main.transform.forward * rayDistance;
		}
			
		//Interpolate the Aim Target there
		USM.aimTarget.transform.position = Vector3.Lerp(USM.aimTarget.transform.position, USM.trueTarget.transform.position, Time.deltaTime * targetSpeed);

		if (USM.animationRig.weight > 0.9f)
			USM.FireSignal("Fire");
		else
			USM.FireSignal("CeaseFire");

		//Adjust Body target to match
		//You can get the direction of the target (targetAngle), and which side you're turned to (targetAngleDir)
		//Try finding the angle from the character's front
		float targetAngle = Vector3.Angle(SM.transform.forward, targetPoint);
		float targetAngleDir = MathHelper.AngleDir(targetPoint, SM.transform.forward) * -1;

		if (targetAngle < maxAngle)
		{
			entered_deadzone = false;

			// Use a rotary angle
			// Lerp said rotary angle towards target angle, this means rotating around the normal way and not trying to skip from 180 to -180
			// Use Euler to apply said rotary angle to aim target
			rotaryAngle = Mathf.Lerp(rotaryAngle, targetAngle * targetAngleDir, Time.deltaTime * targetSpeed);
			Vector3 targetVector = Quaternion.Euler(0, rotaryAngle, 0) * SM.transform.forward;
			USM.bodyAimTarget.transform.position = Vector3.Lerp(USM.bodyAimTarget.transform.position, SM.transform.position + targetVector, Time.deltaTime * targetSpeed);
		}
		else if (targetAngle > maxAngle)
		{
			//If entering the deadzone, note the side of entry
			if (!entered_deadzone) crossEntry = targetAngleDir;
			entered_deadzone = true;

			//Currently calculates which side the of the dead zone to lock to and lerps to the max angle
			Vector3 cappedVector = Quaternion.Euler(0, crossEntry * maxAngle, 0) * SM.transform.forward;
			USM.bodyAimTarget.transform.position = Vector3.Lerp(USM.bodyAimTarget.transform.position, SM.transform.position + cappedVector, Time.deltaTime * targetSpeed);
		}

		if (!SM.myInputs.GetInput("Shoot") || SM.myStatus.currentState == "Dodge" || SM.myStatus.currentState == "Sprint")
			SM.SwitchState("StandyBy"); SM.FireSignal("CeaseFire");
	}

	public override Vector3 MotionUpdate()
	{
		return Vector3.zero;
	}

	public override void EndState()
	{
		SM.myAnimator.SetBool("Shoot", false);
		USM.gunAnimator.SetBool("Shooting", false);
		USM.FireSignal("Ceasefire");
	}
}

public class PlayerUpper_ChargeState : State
{
	public PlayerUpperSM USM;
	Vector3 targetPoint;
	float targetSpeed = 20f;

	public PlayerUpper_ChargeState(string name, StateMachine stateMachine, PlayerUpperSM upperStateMachine) : base(name, stateMachine)
	{
		USM = upperStateMachine;
	}

	public override void StartState()
	{
		targetPoint = SM.transform.position + (SM.transform.forward * 2f);
		USM.FireSignal("Charge");
		USM.gunAnimator.SetBool("Shooting", true);
	}

	public override void UpdateState()
	{
		USM.aimTarget.transform.position = Vector3.Lerp(USM.aimTarget.transform.position, targetPoint, Time.deltaTime * targetSpeed);
		if (USM.animationRig.weight > 0)
		{
			USM.animationRig.weight = Mathf.Lerp(USM.animationRig.weight, 0, 0.1f);
			if (USM.animationRig.weight < 0.01f)
			{
				USM.animationRig.weight = 0;
				USM.rigBuilder.enabled = false;
			}
		}

		if (SM.myInputs.GetInput("Shoot"))
			SM.SwitchState("Cannon");
		else if (!SM.myInputs.GetInput("Charge"))
			SM.SwitchState("StandyBy");
	}

	public override Vector3 MotionUpdate()
	{
		return Vector3.zero;
	}

	public override void EndState()
	{
		USM.FireSignal("Cancel");
		USM.gunAnimator.SetBool("Shooting", false);
	}
}

public class PlayerUpper_CannonState : State
{
	public PlayerUpperSM USM; //Reference IK components through here
	RaycastHit hit;
	float rayDistance = 500;

	Vector3 targetPoint;
	float targetSpeed = 15f;

	float maxAngle = 145;
	bool entered_deadzone = false;
	float crossEntry = 0;
	float rotaryAngle = 0;

	float fireTime = 0;

	public PlayerUpper_CannonState(string name, StateMachine stateMachine, PlayerUpperSM upperStateMachine) : base(name, stateMachine)
	{
		USM = upperStateMachine;
	}

	public override void StartState()
	{
		//Activate IK components
		USM.rigBuilder.enabled = true;

		//Trigger Animation Controller
		SM.myAnimator.SetBool("Shoot", true);
		USM.gunAnimator.SetBool("Shooting", true);
		targetPoint = SM.transform.position + (SM.transform.forward * 2f);

		//Firing Timer
		fireTime = 2;
	}

	public override void UpdateState()
	{
		if (USM.animationRig.weight < 1)
		{
			USM.animationRig.weight = Mathf.Lerp(USM.animationRig.weight, 1f, 0.1f);
			if (USM.animationRig.weight > 0.99f) USM.animationRig.weight = 1f;
		}
		fireTime -= (fireTime <= 0) ? 0 : Time.deltaTime;

		//Raycast from camera and/or the gun and set the target
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		LayerMask mask = LayerMask.GetMask("default");
		mask |= (1 << LayerMask.NameToLayer("Terrain"));

		if (Physics.Raycast(ray, out hit, rayDistance, mask))
		{
			targetPoint = hit.point - USM.gunBarrel.position;
			USM.trueTarget.transform.position = hit.point;
		}
		else
		{
			targetPoint = (Camera.main.transform.position + Camera.main.transform.forward * rayDistance) - USM.gunBarrel.position;
			USM.trueTarget.transform.position = Camera.main.transform.position + Camera.main.transform.forward * rayDistance;
		}

		//Interpolate the Aim Target there
		USM.aimTarget.transform.position = Vector3.Lerp(USM.aimTarget.transform.position, USM.trueTarget.transform.position, Time.deltaTime * targetSpeed);

		if (USM.animationRig.weight > 0.9f)
		{
			USM.FireSignal("Release");
		}
			

		//Adjust Body target to match
		//You can get the direction of the target (targetAngle), and which side you're turned to (targetAngleDir)
		//Try finding the angle from the character's front
		float targetAngle = Vector3.Angle(SM.transform.forward, targetPoint);
		float targetAngleDir = MathHelper.AngleDir(targetPoint, SM.transform.forward) * -1;

		if (targetAngle < maxAngle)
		{
			entered_deadzone = false;

			// Use a rotary angle
			// Lerp said rotary angle towards target angle, this means rotating around the normal way and not trying to skip from 180 to -180
			// Use Euler to apply said rotary angle to aim target
			rotaryAngle = Mathf.Lerp(rotaryAngle, targetAngle * targetAngleDir, Time.deltaTime * targetSpeed);
			Vector3 targetVector = Quaternion.Euler(0, rotaryAngle, 0) * SM.transform.forward;
			USM.bodyAimTarget.transform.position = Vector3.Lerp(USM.bodyAimTarget.transform.position, SM.transform.position + targetVector, Time.deltaTime * targetSpeed);
		}
		else if (targetAngle > maxAngle)
		{
			//If entering the deadzone, note the side of entry
			if (!entered_deadzone) crossEntry = targetAngleDir;
			entered_deadzone = true;

			//Currently calculates which side the of the dead zone to lock to and lerps to the max angle
			Vector3 cappedVector = Quaternion.Euler(0, crossEntry * maxAngle, 0) * SM.transform.forward;
			USM.bodyAimTarget.transform.position = Vector3.Lerp(USM.bodyAimTarget.transform.position, SM.transform.position + cappedVector, Time.deltaTime * targetSpeed);
		}

		if (fireTime <= 0 || SM.myStatus.currentState == "Dodge" || SM.myStatus.currentState == "Sprint")
		{
			SM.SwitchState("StandyBy");
		}
	}

	public override Vector3 MotionUpdate()
	{
		return Vector3.zero;
	}

	public override void EndState()
	{
		SM.myAnimator.SetBool("Shoot", false);
		USM.gunAnimator.SetBool("Shooting", false);
		USM.FireSignal("Ceasefire");
		SM.myStatus.SetCooldown("Laser", 3);
	}
}