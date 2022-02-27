using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleLauncher : MonoBehaviour
{
    [Header("Main Stuff")]
    [SerializeField] StateMachine CoreStateMachine;
    [SerializeField] Transform target;

    [Header("Bullet Stuff")]
    [SerializeField] ParticleSystem particleLauncher;
    [SerializeField] VisualEffect muzzleFlash;
    [SerializeField] VisualEffect bulletImpact;

    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    [SerializeField]float fireRate = 0.08f;
    float timer = 0;
    bool fire;

    void Start()
    {
        CoreStateMachine.TestSignalEvent += OnStateMachineSignal;
    }

    void Update()
    {
        timer -= timer <= 0 ? 0 : Time.deltaTime;
        if(fire)
		{
            if (target)
                transform.rotation = Quaternion.LookRotation(target.position - transform.position);
            if (timer <= 0)
			{
                particleLauncher.Emit(1);
                muzzleFlash.Play();
                timer = fireRate;
            }
        }
    }

    private void OnDisable()
    {
        CoreStateMachine.TestSignalEvent -= OnStateMachineSignal;
    }

    void OnStateMachineSignal(string signalID)
    {
        switch (signalID)
		{
            case "Fire":
                fire = true;
                break;
            case "Ceasefire":
                fire = false;
                break;
        }
    }

	private void OnParticleCollision(GameObject other)
	{
        int events = particleLauncher.GetCollisionEvents(other, collisionEvents);
        for (int i = 0; i < events; i++)
		{
            bulletImpact.transform.position = collisionEvents[i].intersection;
            bulletImpact.SetVector3("Rotation", Quaternion.LookRotation(collisionEvents[i].normal, Vector3.up).eulerAngles);
            bulletImpact.Play();
        }
	}
}
