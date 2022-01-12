using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePod : MonoBehaviour
{
    [SerializeField] StateMachine CoreStateMachine;
    [SerializeField] ParticleSystem missileSystem;
    ParticleSystem.Particle[] missiles;
    [SerializeField] float missileTurnRate;
    [SerializeField] float missileSpeed;

    Vector3 targetPoint;
    LayerMask laserMask;

    void Start()
    {
        CoreStateMachine.TestSignalEvent += OnStateMachineSignal;

        missiles = new ParticleSystem.Particle[missileSystem.main.maxParticles];

        laserMask |= (1 << LayerMask.NameToLayer("Player"));
        laserMask |= (1 << LayerMask.NameToLayer("Terrain"));
        laserMask |= (1 << LayerMask.NameToLayer("Wall"));
        laserMask |= (1 << LayerMask.NameToLayer("Water"));
    }

	private void Update()
	{
        targetPoint = CoreStateMachine.myInputs.PointerTarget;
        int numMissiles = missileSystem.GetParticles(missiles);
        TextUpdate.Instance.SetText("Number of Missiles", missileSystem.particleCount.ToString());

        for(int i = 0; i < numMissiles; i++)
	    {
            Vector3 targetDirection = (targetPoint - missiles[i].position).normalized * missileSpeed;
            missiles[i].velocity = Vector3.Lerp(missiles[i].velocity, targetDirection, missileTurnRate);
        }

        missileSystem.SetParticles(missiles, numMissiles);
    }

	void OnStateMachineSignal(string signalID)
    {
        switch (signalID)
        {
            case "Fire":
                FireMissiles();
                break;
            case "Ceasefire":
                CeaseMissiles();
                break;
        }
    }

    void FireMissiles()
    {
        missileSystem.Play();
    }

    void CeaseMissiles()
    {
        missileSystem.Stop();
    }

    private void OnDisable()
    {
        CoreStateMachine.TestSignalEvent -= OnStateMachineSignal;
    }
}
