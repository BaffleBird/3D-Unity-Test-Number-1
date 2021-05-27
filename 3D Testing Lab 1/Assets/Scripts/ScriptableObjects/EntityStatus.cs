using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName ="Scritable Object/Entity Status")]
public class EntityStatus : ScriptableObject
{
	[SerializeField] float gravity = 0;
	public float Gravity { get { return gravity; } }

	[HideInInspector] public float wallSlope;

	public bool isStableGround = false;
	[HideInInspector] public Vector3 hitNormal;

	public Vector3 currentMovement;

	Dictionary<string, float> myCooldowns = new Dictionary<string, float>();

	//COOLDOWN MANAGEMENT
	public void SetCooldown(string key, float cooldown)
	{
		if (!myCooldowns.ContainsKey(key))
			myCooldowns.Add(key, cooldown);
		else
			myCooldowns[key] = cooldown;
	}

	public bool GetCooldown(string actionName)
	{
		if (myCooldowns.ContainsKey(actionName))
			return myCooldowns[actionName] <= 0;
		return true;
	}

	public void UpdateCooldowns()
	{
		foreach (string cd in myCooldowns.Keys.ToList())
		{
			if (myCooldowns[cd] > 0)
				myCooldowns[cd] -= Time.deltaTime;
		}
	}
}