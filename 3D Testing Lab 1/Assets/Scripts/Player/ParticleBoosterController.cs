using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleBoosterController : MonoBehaviour
{
    [SerializeField] StateMachine CoreStateMachine;

    [SerializeField] Light boosterLight;
    [SerializeField] GameObject[] thrusters;
    List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    [SerializeField] VisualEffect rocketFlare;

    private float outputLevel = 0f;
    [SerializeField] float lightMaxIntensity = 0.64f;
    [SerializeField] float sizeMax = 0.25f;
    [SerializeField] float speedMax = 1.2f;
    [Range(0,1)][SerializeField] float ignitionSpeed = 0.25f, decaySpeed = 0.1f;

    bool thrusterON = false;

    // Start is called before the first frame update
    void Start()
    {
        boosterLight.intensity = 0;

        for (int i = 0; i < thrusters.Length; i++)
        {
            particleSystems.Add(thrusters[i].GetComponent<ParticleSystem>());
        }

        for (int i = 0; i < particleSystems.Count; i++)
		{
            var main = particleSystems[i].main;
            main.startSize = 0;
            main.startSpeed = 0.2f;
        }

        CoreStateMachine.TestSignalEvent += OnStateMachineSignal;
    }

    bool rocketBurst = false;
    // Update is called once per frame
    void Update()
    {
        if (thrusterON)
		{
            outputLevel = Mathf.Lerp(outputLevel, 1, ignitionSpeed);
            if (!rocketBurst)
			{
                rocketFlare.Play();
                rocketBurst = true;
			}
        }
        else
		{
            outputLevel = Mathf.Lerp(outputLevel, 0, decaySpeed);
            rocketBurst = false;
        }   
    }
	private void LateUpdate()
	{
        boosterLight.intensity = outputLevel * lightMaxIntensity;
        for (int i = 0; i < particleSystems.Count; i++)
        {
            var main = particleSystems[i].main;
            main.startSize = sizeMax * outputLevel;
            main.startSpeed = speedMax * outputLevel;
        }
    }

	private void OnDisable()
	{
        CoreStateMachine.TestSignalEvent -= OnStateMachineSignal;
    }

	void OnStateMachineSignal(string signalID)
    {
        if (signalID == "ThrusterOn")
            thrusterON = true;
        else if (signalID == "ThrusterOff")
            thrusterON = false;
    }
}
