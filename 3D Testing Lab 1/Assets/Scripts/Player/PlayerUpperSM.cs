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

	[SerializeField] Transform _gunBarrel = null;
	public Transform gunBarrel => _gunBarrel;

	protected override void Awake()
	{
		base.Awake();

		States.Add("StandyBy", new PlayerUpper_StandyByState("StandyBy", this, this));
		States.Add("Shoot", new PlayerUpper_ShootState("Shoot", this, this));

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

	public PlayerUpper_StandyByState(string name, StateMachine stateMachine, PlayerUpperSM upperStateMachine) : base(name, stateMachine) 
	{
		USM = upperStateMachine;
	}

	public override void StartState()
	{
	}

	public override void UpdateState()
	{
		if (USM.animationRig.weight > 0)
		{
			USM.animationRig.weight = Mathf.Lerp(USM.animationRig.weight, 0, 0.04f);
			if (USM.animationRig.weight < 0.01f)
			{
				USM.animationRig.weight = 0;
				USM.rigBuilder.enabled = false;
			}
		}

		if (SM.myInputs.GetInput("Shoot") && SM.myStatus.currentState != "Dodge" && SM.myStatus.currentState != "Sprint")
			SM.SwitchState("Shoot");
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
	float rayDistance = 100;

	Vector3 targetPoint;
	float targetSpeed = 15f;

	float maxAngle = 130;
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
		LayerMask mask = LayerMask.GetMask("default");
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, mask))
			targetPoint = hit.point - USM.gunBarrel.position;
		else
			targetPoint = (Camera.main.transform.position + Camera.main.transform.forward * rayDistance) - USM.gunBarrel.position;

		//Interpolate the Aim Target there
		USM.aimTarget.transform.position = Vector3.Lerp(USM.aimTarget.transform.position, targetPoint, Time.deltaTime * targetSpeed);

		//Adjust Body target to match
		//You can get the direction of the target, and which side you're turned to
		//Try finding the angle from the front
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
			//If entering the deadzone, note which side the entry is
			if (!entered_deadzone) crossEntry = targetAngleDir;
			entered_deadzone = true;

			//Currently calculates which side the of the dead zone to lock to
			Vector3 cappedVector = Quaternion.Euler(0, crossEntry * maxAngle, 0) * SM.transform.forward;
			USM.bodyAimTarget.transform.position = Vector3.Lerp(USM.bodyAimTarget.transform.position, SM.transform.position + cappedVector, Time.deltaTime * targetSpeed);
		}

		if (!SM.myInputs.GetInput("Shoot") || SM.myStatus.currentState == "Dodge" || SM.myStatus.currentState == "Sprint")
			SM.SwitchState("StandyBy");
	}

	public override Vector3 MotionUpdate()
	{
		return Vector3.zero;
	}

	public override void EndState()
	{
		SM.myAnimator.SetBool("Shoot", false);
	}
}
