using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scritable Object/Entity Status")]
public class EnemySpiderStats : ScriptableObject
{
	public float moveSpeed = 0;
	public float turnSpeed = 0;
	public float leapAngle = 0;
	public float leapGravity = 0;

}
