using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scritable Object/Entity Status")]
public class EntityStatus : ScriptableObject
{
	//Lerp player's movement to input * Maxspeed
    [SerializeField] float moveSpeed = 0;
	public float MoveSpeed{ get { return moveSpeed; } }

	[SerializeField] float gravity = 0;
	public float Gravity { get { return Gravity; } }

	public bool isGrounded = true;

	[HideInInspector] public Vector3 currentMovement;
}
