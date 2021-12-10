using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	#region MajorComponents
	[SerializeField] EntityStatus _myStatus = null;
	public EntityStatus myStatus => _myStatus;

	[SerializeField] InputHandler _myInputs = null;
	public InputHandler myInputs => _myInputs;

	[SerializeField] CharacterController _myCController = null;
	public CharacterController myCController => _myCController;

	[SerializeField] GameObject _myModel = null;
	public GameObject myModel => _myModel;

	[SerializeField] Animator _myAnimator = null;
	public Animator myAnimator => _myAnimator;
	#endregion

	#region StateVariables
	protected Dictionary<string, State> States = new Dictionary<string, State>();
	protected State currentState = null;
	public string currentStateName => currentState.StateName;
	protected string _previousState = "";
	public string previousState { get { return _previousState; } }
	#endregion

	#region TestDelegate
	public delegate void TestSignal(string signalID);
	public event TestSignal TestSignalEvent;

	public void FireSignal(string signalID)
	{
		TestSignalEvent(signalID);
	}
	#endregion

	LayerMask terrainMask;
	protected virtual void Awake()
    {
		terrainMask = LayerMask.GetMask("Terrain");
	}

    protected virtual void Update()
    {
		if (currentState != null)
			currentState.UpdateState();
		myStatus.UpdateCooldowns();
	}

	protected virtual void FixedUpdate()
	{
		currentState.FixedUpdateState();
		myCController.Move(currentState.MotionUpdate() * Time.fixedDeltaTime);
		myStatus.currentMovement = currentState.MotionUpdate();
	}

	protected virtual void LateUpdate()
	{
		currentState.LateUpdateState();
	}

	//STATE MANAGEMENT
	protected virtual void SwitchState(State newState)
	{
		currentState.EndState();
		_previousState = currentState.StateName;
		currentState = newState;
		currentState.StartState();
	}

	public void SwitchState(string stateName)
	{
		if (myStatus.GetCooldown(stateName))
		{
			SwitchState(States[stateName]);
		}
	}

	protected virtual void StabilityCheck()
	{
		if (myCController.isGrounded)
		{
			myCController.Move(Vector3.down * 0.2f);
			myStatus.isStableGround = (Vector3.Angle(Vector3.up, myStatus.hitNormal)) <= myCController.slopeLimit;
		}
		else
		{
			myStatus.isStableGround = false;
		}
	}

	public float StabilityPercentage()
	{
		return MathHelper.Percentage((Vector3.Angle(Vector3.up, myStatus.hitNormal)), myCController.slopeLimit);
	}

}

public abstract class State
{
	protected StateMachine SM;

	string _stateName = null;
	public string StateName { get { return _stateName; } }

	public State(string name, StateMachine stateMachine)
	{
		_stateName = name;
		SM = stateMachine;
	}

	public abstract void StartState();
	public abstract void UpdateState();
	public virtual void FixedUpdateState() { }
	public virtual void LateUpdateState() { }
	public abstract void Transition();
	public abstract Vector3 MotionUpdate();
	public abstract void EndState();
}
