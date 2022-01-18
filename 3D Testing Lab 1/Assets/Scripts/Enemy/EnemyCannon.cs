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
    float laserScale = 1;

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
            case "FireLaser":
                StartCoroutine(ChargeLaser());
                break;
            case "CeaseLaser":
                CeaseLaser();
                break;
            case "Cancel":
                CeaseLaser();
                break;
        }
    }

	private void FixedUpdate()
	{
    }

    IEnumerator ChargeLaser()
	{
        float chargeTime = 2;
        laserScale = 0.1f;
        laserSource.Play();

        while (chargeTime > 0)
		{
            chargeTime -= Time.deltaTime;
            laserScale = Mathf.Lerp(laserScale, 4, 0.05f);
            laserSource.transform.localScale = Vector3.one * laserScale;
            yield return null;
        }

        StartCoroutine(FireLaser());
	}

    IEnumerator FireLaser()
	{
        // Targeting System
        targetPoint = CoreStateMachine.myInputs.PointerTarget;
        targetDirection = (targetPoint - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        laserBeam.gameObject.SetActive(true);
        laserBeam.localScale = Vector3.one * 5;
        laserImpact.Play();

        // Launch the laser with Scaling Magic
        // * Use targetPoint as a projectile move it along the targetDirection at a rate of fireSpeed per frame
        // * Stretch and thin laser until it reaches fireRange or strikes a surface (Linecast or Raycast will do)
        // * Stick the Impact effect at the end of it all the way
        float fireRange = 30;
        float fireSpeed = 200;
        float currentPosition = 0;
        RaycastHit hit;

        while(currentPosition < fireRange ||
            !Physics.Linecast(transform.position, targetPoint, out hit, laserMask))
		{
            currentPosition += fireSpeed * Time.deltaTime;
            targetPoint = transform.position + (targetDirection * currentPosition);

            Vector3 newScale = laserBeam.transform.localScale;
            newScale.z = Vector3.Distance(transform.position, targetPoint) * 0.95f;
            newScale.x = Mathf.Lerp(newScale.x, 2f, 0.1f);
            newScale.y = newScale.x;
            laserBeam.transform.localScale = newScale;

            laserImpact.transform.position = targetPoint;
            yield return null;
        }


        // [Optional] Raycast and Sweep
        // * Use an adjusted vector to aim to the left or right of the player
        // * Smoothdamp or lerp the aim point across the player position and to the other side
        // * Raycast all the way


        // Disipate the Laser
        // * Lerp the laser thickness (scale y and z) to zero
        // * Lerp the AlphaClip of the Material to one


        CeaseLaser();
    }

    void CeaseLaser()
	{
        //Scale laser components down
        laserSource.Stop();
        laserBeam.gameObject.SetActive(false);
        laserImpact.Stop();
    }

    private void OnDisable()
    {
        CoreStateMachine.TestSignalEvent -= OnStateMachineSignal;
    }

    
}
