using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeatMeter : MonoBehaviour
{
	[SerializeField] Image meterFill;
	[SerializeField] Image chargeMeterFill;
	[SerializeField] Gradient heatColorGradient;
	[SerializeField] Color overChargeColor;

    void Start()
    {
        AlmightySingleton.Instance.TestSignalEvent += UpdateMeterUI;
    }

	private void UpdateMeterUI(string signalID)
	{
		switch (signalID)
		{
			case "HeatChange":
				meterFill.fillAmount = AlmightySingleton.Instance.PlayerStats.HeatRatio;
				break;
			case "ChargeChange":
				chargeMeterFill.fillAmount = AlmightySingleton.Instance.PlayerStats.ChargeRatio;
				break;
		}

		if (AlmightySingleton.Instance.PlayerStats.OverCharged)
		{
			meterFill.color = overChargeColor;
		}
		else if (AlmightySingleton.Instance.PlayerStats.Overheated)
		{
			meterFill.color = heatColorGradient.Evaluate(1);
		}
		else
		{
			meterFill.color = heatColorGradient.Evaluate(AlmightySingleton.Instance.PlayerStats.HeatRatio);
		}
	}
}
