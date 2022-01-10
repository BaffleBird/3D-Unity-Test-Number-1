using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    [SerializeField] StateMachine CoreStateMachine;
    [SerializeField] ParticleSystem laserSource;
    [SerializeField] Transform laserBeam;
    [SerializeField] ParticleSystem laserImpact;

    Vector3 targetPoint;
    Vector3 targetDirection;
    bool laserOn = false;

    LayerMask laserMask;

    void Start()
    {
        CeaseLaser();
        CoreStateMachine.TestSignalEvent += OnStateMachineSignal;
        laserMask |= (1 << LayerMask.NameToLayer("Player"));
        laserMask |= (1 << LayerMask.NameToLayer("Terrain"));
        laserMask |= (1 << LayerMask.NameToLayer("Wall"));
        laserMask |= (1 << LayerMask.NameToLayer("Water"));
    }

    void OnStateMachineSignal(string signalID)
    {
        switch (signalID)
        {
            case "Charge":
                break;
            case "Fire":
                FireLaser();
                break;
            case "Ceasefire":
                CeaseLaser();
                break;
            case "Cancel":
                break;
        }
    }

	private void FixedUpdate()
	{
        RaycastHit hit;
        if (Physics.Raycast(transform.position, targetDirection, out hit, 10, laserMask))
		{
            Vector3 newScale = laserBeam.transform.localScale;
            newScale.z = Vector3.Distance(transform.position, targetPoint);
            laserBeam.transform.localScale = newScale;
            laserImpact.transform.position = hit.point;
        }
        else
		{
            Vector3 endPoint = transform.position + (targetDirection * 10);

            Vector3 newScale = laserBeam.transform.localScale;
            newScale.z = Vector3.Distance(transform.position, endPoint);
            laserBeam.transform.localScale = newScale;
            laserImpact.transform.position = endPoint;
        }
    }

    void FireLaser()
	{
        laserOn = true;

        targetPoint = CoreStateMachine.myInputs.PointerTarget;
        targetDirection = (targetPoint - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        laserSource.Play();
        laserBeam.gameObject.SetActive(true);
        laserImpact.Play();
    }

    void CeaseLaser()
	{
        laserSource.Stop();
        laserBeam.gameObject.SetActive(false);
        laserImpact.Stop();
        laserOn = false;
    }

    private void OnDisable()
    {
        CoreStateMachine.TestSignalEvent -= OnStateMachineSignal;
    }

    
}
