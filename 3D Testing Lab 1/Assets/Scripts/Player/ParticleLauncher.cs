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

    [Header("Light Stuff")]
    [SerializeField] Light muzzleLight;
    [SerializeField] float lightMaxIntensity = 0.64f;

    [Header("Laser Stuff")]
    [SerializeField] VisualEffect cannonOrb;
    [SerializeField] VisualEffect beam;
    [SerializeField] VisualEffect smoke;
    float impactTimer = 0;

    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    [SerializeField]float fireRate = 0.08f;
    float timer = 0;
    bool fire;

    bool charge;
    float strength = 0;
    bool fireCannon;
    bool cannonFired;

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
            muzzleLight.intensity = lightMaxIntensity;
            if (timer <= 0)
			{
                particleLauncher.Emit(1);
                muzzleFlash.Play();
                timer = fireRate;
            }
        }
        else if (fireCannon)
		{
            if(!cannonFired)
			{
                beam.Play();
                impactTimer = 1.85f;
                cannonFired = true;
            }
		}
        else if (charge)
		{
            strength = Mathf.Lerp(strength, 1, 0.005f);
            cannonOrb.SetFloat("Strength", strength);
            muzzleLight.intensity = lightMaxIntensity;
        }
        else
		{
            cannonFired = false;
            strength = Mathf.Lerp(strength, 0, 0.01f);
            cannonOrb.SetFloat("Strength", strength);
            muzzleLight.intensity = 0;
            if (strength < 0.05f)
                cannonOrb.Stop();
        }

        BeamImpact();
    }

    bool smokeIsPlaying = false;
    void BeamImpact()
	{
        if (impactTimer > 0)
		{
            LayerMask mask = LayerMask.GetMask("Terrain");
            RaycastHit hit;
            impactTimer -= Time.deltaTime;
            if(Physics.Linecast(transform.position, target.position, out hit, mask))
			{
                smoke.transform.position = hit.point;
                if (!smokeIsPlaying)
				{
                    smokeIsPlaying = true;
                    smoke.Play();
                }
                   
            }
            else
			{
                if (smokeIsPlaying)
                {
                    smokeIsPlaying = false;
                    smoke.Stop();
                }
            }
        }
        else if (smokeIsPlaying)
        {
            smokeIsPlaying = false;
            smoke.Stop();
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
                fireCannon = false;
                break;
            case "Charge":
                charge = true;
                cannonOrb.Play();
                break;
            case "Release":
                fireCannon = true;
                break;
            case "Cancel":
                charge = false;
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
