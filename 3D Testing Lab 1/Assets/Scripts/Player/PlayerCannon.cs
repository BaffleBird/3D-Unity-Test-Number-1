using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerCannon : MonoBehaviour
{
    [SerializeField] StateMachine CoreStateMachine;
    [SerializeField] VisualEffect laserSource;
    [SerializeField] Transform laserBeam;
    [SerializeField] Renderer beamRenderer;
    [SerializeField] VisualEffect laserImpact;
    [SerializeField] Transform target;

    LayerMask laserMask;
    float chargeLevel = 0;

    void Start()
    {
        CeaseLaser();

        CoreStateMachine.TestSignalEvent += OnStateMachineSignal;
        laserMask |= (1 << LayerMask.NameToLayer("Player"));
        laserMask |= (1 << LayerMask.NameToLayer("Terrain"));
        laserMask |= (1 << LayerMask.NameToLayer("Wall"));
        laserMask |= (1 << LayerMask.NameToLayer("Water"));

        chargeLevel = 0;
    }

    Coroutine lastCoroutine = null;
    void ResetCoroutine() { if (lastCoroutine != null) StopCoroutine(lastCoroutine); }

	void OnStateMachineSignal(string signalID)
    {
        switch (signalID)
        {
            case "Charge":
                ResetCoroutine();
                lastCoroutine = StartCoroutine(ChargeLaser());
                break;
            case "FireCannon":
                ResetCoroutine();
                lastCoroutine = StartCoroutine(FireLaser());
                break;
            case "CeaseLaser":
                CeaseLaser();
                break;
            case "Cancel":
                ResetCoroutine();
                lastCoroutine = StartCoroutine(DechargeLaser());
                break;
        }
    }

    IEnumerator ChargeLaser()
    {
        chargeLevel = 0;
        laserSource.Play();

        while (chargeLevel < 1)
		{
            chargeLevel += Time.deltaTime;
            laserSource.SetFloat("Strength", chargeLevel);
            AlmightySingleton.Instance.PlayerStats.SetCharge(chargeLevel);
            yield return null;
        }

        chargeLevel = 1;
        laserSource.SetFloat("Strength", chargeLevel);
    }

    IEnumerator DechargeLaser()
    {
        while (chargeLevel > 0)
        {
            chargeLevel -= Time.deltaTime;
            laserSource.SetFloat("Strength", chargeLevel);
            AlmightySingleton.Instance.PlayerStats.SetCharge(chargeLevel);
            yield return null;
        }
        chargeLevel = 0;
        laserSource.SetFloat("Strength", chargeLevel);
        laserSource.Stop();
    }

    IEnumerator FireLaser()
    {
        Vector3 targetPoint = target.position;

        laserSource.SetFloat("Strength", 0);
        laserSource.Stop();
        laserBeam.gameObject.SetActive(true);
        laserBeam.localScale = Vector3.one * 5;

        float fireRange = 50;
        float fireSpeed = 300;
        float currentPosition = 0;
        RaycastHit hit;

        AlmightySingleton.Instance.PlayerStats.FireCharge();

        while (currentPosition < fireRange)
        {
            targetPoint = target.position;
			Vector3 targetDirection = (targetPoint - transform.position).normalized;

			currentPosition += fireSpeed * Time.deltaTime;
            targetPoint = transform.position + (targetDirection * currentPosition);

            Vector3 newScale = laserBeam.transform.localScale;
            newScale.z = Vector3.Distance(transform.position, targetPoint) * 0.95f;
            newScale.x = Mathf.Lerp(newScale.x, 2f * chargeLevel, 0.5f);
            newScale.y = newScale.x;

            laserBeam.transform.localScale = newScale;

            if (Physics.Linecast(transform.position, target.position, out hit, laserMask))
			{
                laserImpact.transform.position = hit.point;
                laserImpact.Play();
            }

            yield return null;
        }

        float holdTime = 1.5f;
        while (holdTime > 0)
		{
            if (Physics.Linecast(transform.position, target.position, out hit, laserMask))
            {
                laserImpact.transform.position = hit.point;
                laserImpact.Play();
            }
            holdTime -= Time.deltaTime;
            yield return null;
        }

        laserImpact.Stop();
        ResetCoroutine();
        lastCoroutine = StartCoroutine(DissipateLaser());
    }

    IEnumerator DissipateLaser()
    {
        float dissipateTime = 1;
        float dissipate = 0;

        AlmightySingleton.Instance.PlayerStats.ReleaseCharge();

        while (dissipateTime > 0)
        {
            dissipateTime -= Time.deltaTime;

            Vector3 newScale = laserBeam.transform.localScale;
            newScale.x = Mathf.Lerp(newScale.x, 0f, 0.05f);
            newScale.y = newScale.x;
            laserBeam.transform.localScale = newScale;

            dissipate = Mathf.Lerp(dissipate, 1, 0.01f);
            beamRenderer.material.SetFloat("AlphaClip", dissipate);
            yield return null;
        }

        CeaseLaser();
    }

    void CeaseLaser()
	{
        chargeLevel = 0;
        laserSource.SetFloat("Strength", chargeLevel);
        laserSource.Stop();
        beamRenderer.material.SetFloat("AlphaClip", 0);
        laserBeam.gameObject.SetActive(false);
        laserImpact.Stop();
    }

    private void OnDisable()
    {
        CoreStateMachine.TestSignalEvent -= OnStateMachineSignal;
    }
}
