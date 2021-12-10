using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleBoosterController : MonoBehaviour
{
    [SerializeField] StateMachine CoreStateMachine;

    [Header("Effect Components")]
    [SerializeField] Light boosterLight;
    [SerializeField] VisualEffect[] thrusters;
    [SerializeField] VisualEffect rocketFlare;
    [SerializeField] VisualEffect terrainTrail;

    [Header("Intensity Controls")]
    [SerializeField] float lightMaxIntensity = 0.64f;
    [Range(0,1)][SerializeField] float ignitionSpeed = 0.25f, decaySpeed = 0.1f;
    private float outputLevel = 0f;

    bool thrusterON = false;
    LayerMask dustMask;
    Color waterDustColor = new Color(0.86f, 0.87f, 0.89f, 1);
    Color terrainDustColor = new Color(0.46f, 0.27f, 0.19f, 1);


    // Start is called before the first frame update
    void Start()
    {
        dustMask = LayerMask.GetMask("Terrain");
        dustMask |= (1 << LayerMask.NameToLayer("Water"));
        boosterLight.intensity = 0;
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
                for (int i = 0; i < thrusters.Length; i++)
                    thrusters[i].Play();
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 6, dustMask))
			{
                terrainTrail.Play();
                terrainTrail.SetVector3("WorldPosition", hit.point);

                TextUpdate.Instance.SetText("Trail", LayerMask.LayerToName(hit.transform.gameObject.layer));

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
                    terrainTrail.SetVector4("DustColor", waterDustColor);
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                    terrainTrail.SetVector4("DustColor", terrainDustColor);
            }
        }
        else
		{
            outputLevel = Mathf.Lerp(outputLevel, 0, decaySpeed);
            rocketBurst = false;
            for (int i = 0; i < thrusters.Length; i++)
                thrusters[i].Stop();
        }   
    }
	private void LateUpdate()
	{
        boosterLight.intensity = outputLevel * lightMaxIntensity;
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
