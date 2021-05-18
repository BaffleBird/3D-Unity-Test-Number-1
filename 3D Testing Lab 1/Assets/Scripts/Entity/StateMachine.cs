using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	[SerializeField] EntityStatus _myStatus = null;
	public EntityStatus myStatus => _myStatus;

	[SerializeField] InputHandler _myInputs = null;
	public InputHandler myInputs => _myInputs;

	[SerializeField] Rigidbody _myRigidbody = null;
	public Rigidbody myRigidbody => _myRigidbody;

	[SerializeField] GameObject _myModel = null;
	public GameObject myModel => _myModel;

	[SerializeField] Collider _myCollider = null;
	public Collider myCollider => _myCollider;

	protected Dictionary<string, State> States = new Dictionary<string, State>();
	protected State currentState = null;
	protected string _previousState = "";
	public string previousState { get { return _previousState; } }

	

	protected virtual void Awake()
    {
        
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
		myRigidbody.MovePosition(myRigidbody.position + (currentState.MotionUpdate() * Time.fixedDeltaTime));
		myStatus.currentMovement = currentState.MotionUpdate();
	}

	//STATE MANAGEMENT
	protected void SwitchState(State newState)
	{
		currentState.EndState();
		_previousState = currentState.StateName;
		currentState = newState;
		currentState.StartState();
	}

	public void SwitchState(string stateName)
	{
		if (myStatus.GetCooldown(stateName))
			SwitchState(States[stateName]);
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
	public abstract Vector3 MotionUpdate();
	public abstract void EndState();
}
