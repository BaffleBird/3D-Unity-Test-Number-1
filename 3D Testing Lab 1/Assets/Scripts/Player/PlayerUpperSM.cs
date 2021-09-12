using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerUpperSM : StateMachine
{
	//Reference IK Fields
	[SerializeField] RigBuilder _rigBuilder = null;
	public RigBuilder RigBuilder => _rigBuilder;

	[SerializeField] Rig _animationRig = null;
	public Rig animationRig => _animationRig;

	[SerializeField] GameObject _aimTarget = null;
	public GameObject bodyAimTarget => _aimTarget;

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
			USM.animationRig.weight = Mathf.Lerp(USM.animationRig.weight, 0, 0.12f);
			if (USM.animationRig.weight < 0.01f)
			{
				USM.animationRig.weight = 0;
				USM.RigBuilder.enabled = false;
			}
		}

		if (SM.myInputs.GetInput("Shoot") && SM.currentStateName != "Dodge") //And not currently not wall jumping or dashing
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

	public PlayerUpper_ShootState(string name, StateMachine stateMachine, PlayerUpperSM upperStateMachine) : base(name, stateMachine) 
	{
		USM = upperStateMachine;
	}

	public override void StartState()
	{
		//Activate IK components
		USM.RigBuilder.enabled = true;
		//Trigger Animation Controller
		SM.myAnimator.SetTrigger("Shoot");
	}

	public override void UpdateState()
	{
		if (USM.animationRig.weight < 0.9f)
		{
			USM.animationRig.weight = Mathf.Lerp(USM.animationRig.weight, 1f, 0.12f);
			if (USM.animationRig.weight > 0.99f) USM.animationRig.weight = 1f;
		}

		//Raycast from camera and/or the gun and move the Aim Target there
		LayerMask mask = LayerMask.GetMask("default");
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance, mask))
		{
			USM.bodyAimTarget.transform.position = hit.point;
			//If you end up using multiple targets, dis is da way.
		}
		else
		{
			USM.bodyAimTarget.transform.position = Camera.main.transform.position + Camera.main.transform.forward * rayDistance;
		}
			

		if (!SM.myInputs.GetInput("Shoot")) //Or cancelled by Wall jumping or Dashing. Possibly Cancelled on Landing with a short cooldown
			SM.SwitchState("StandyBy");
	}

	public override Vector3 MotionUpdate()
	{
		return Vector3.zero;
	}

	public override void EndState()
	{
	}
}
