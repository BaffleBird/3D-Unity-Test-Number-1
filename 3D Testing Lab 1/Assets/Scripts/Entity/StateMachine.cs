using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	[SerializeField] EntityStatus _myStatus = null;
	public EntityStatus myStatus => _myStatus;

	[SerializeField] InputHandler _myInputs = null;
	public InputHandler myInputs => _myInputs;

	[SerializeField] CharacterController _myCController;
	public CharacterController myCController => _myCController;

	[SerializeField] GameObject _myModel = null;
	public GameObject myModel => _myModel;

	[SerializeField] Collider _myCollider = null;
	public Collider myCollider => _myCollider;

	protected Dictionary<string, State> States = new Dictionary<string, State>();
	protected State currentState = null;
	protected string _previousState = "";
	public string previousState { get { return _previousState; } }


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
		TextUpdate.Instance.SetText("State", currentState.StateName);
	}

	protected virtual void FixedUpdate()
	{
		currentState.FixedUpdateState();
		myCController.Move(currentState.MotionUpdate() * Time.fixedDeltaTime);
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

	//PHYSICS CHECKS
	protected void WallCheck(float rayLength)
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position, MathHelper.ZeroVectorY(myStatus.currentMovement).normalized, out hit, rayLength, terrainMask))
			myStatus.wallSlope = (Vector3.Angle(hit.normal, Vector3.down));
		else
			myStatus.wallSlope = 0;

		//Debug.DrawRay(transform.position, MathHelper.ZeroVectorY(myStatus.currentMovement).normalized * rayLength, Color.cyan);
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
