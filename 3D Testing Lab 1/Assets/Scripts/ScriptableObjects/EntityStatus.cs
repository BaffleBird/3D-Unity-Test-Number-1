using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scritable Object/Entity Status")]
public class EntityStatus : ScriptableObject
{
	//Lerp player's movement to input * Maxspeed
    [SerializeField] float maxSpeed = 0;
	public float MaxSpeed{ get { return maxSpeed; } }
	[SerializeField] float lerpSpeed = 0;
	public float LerpSpeed { get { return lerpSpeed; } }

	[SerializeField] float maxGravity = 0;
	public float MaxGravity { get { return maxGravity; } }
	[SerializeField] float lerpGravity = 0;
	public float LerpGravity { get { return lerpGravity; } }

	[HideInInspector] public Vector3 currentMovement;
}
