using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scritable Object/Entity Status")]
public class EntityStatus : ScriptableObject
{
	[SerializeField] float gravity = 0;
	public float Gravity { get { return gravity; } }

	public bool isGrounded = true;

	[HideInInspector] public Vector3 currentMovement;
}
