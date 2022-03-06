using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameStats
{
    private float health;
    private float maxHealth;

    private float heat;
    private float chargeHeat;
    private float maxHeat;
    public float HeatRatio => heat / maxHeat;
    public float ChargeRatio => chargeHeat;

    private float cooldown;
    private bool _overheated = false;
    public bool Overheated => _overheated;
    private bool _overcharged = false;
    public bool OverCharged => _overcharged;


    public PlayerGameStats(float maxHP, float heatCap, float cooldownSpd)
	{
        maxHealth = maxHP;
        health = maxHealth;
        maxHeat = heatCap;
        cooldown = cooldownSpd;
	}

    public bool HeatUp(float heatLevel)
	{
        
        heat += heatLevel;
        if (heat >= maxHeat)
		{
            heat = maxHeat;
            _overheated = true;
            AlmightySingleton.Instance.FireSignal("HeatChange");
            return true;
		}
        AlmightySingleton.Instance.FireSignal("HeatChange");
        return false;
	}

    public void Cooldown(float modifier)
	{
        if (!OverCharged)
		{
            if (heat > 0)
            {
                AlmightySingleton.Instance.FireSignal("HeatChange");
                heat -= cooldown * modifier;
            }

            if (heat <= 0)
            {
                heat = 0;
                _overheated = false;
            }
        }
	}

    public void SetCharge(float heatLevel)
    {
        AlmightySingleton.Instance.FireSignal("ChargeChange");

        chargeHeat = heatLevel + HeatRatio;
    }

    public void FireCharge()
	{
        heat = (chargeHeat * maxHeat) + heat;
        chargeHeat = 0;
        AlmightySingleton.Instance.FireSignal("ChargeChange");
        AlmightySingleton.Instance.FireSignal("HeatChange");
        _overcharged = true;
    }

    public void ReleaseCharge()
	{
        _overcharged = false;
        if (heat >= maxHeat)
		{
            _overheated = true;
		}
    }
}
