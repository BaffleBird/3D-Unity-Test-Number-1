using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIK : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

	private void FixedUpdate()
	{
	}

	private void OnAnimatorIK(int layerIndex)
	{
	}
}
