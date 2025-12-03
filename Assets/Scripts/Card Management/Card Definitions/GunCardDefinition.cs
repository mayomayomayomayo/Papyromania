using System;
using UnityEngine;

[Serializable]
public class GunCardDefinition : CardDefinition
{
    public float damage;
    public int maxAmmo;
    public int ammo;
    public bool isFullAuto;
    public float shotDelay;
    public float shotRange;

    public GunCardDefinition(CardData raw) : base(raw)
    {
        damage = raw.damage;
        maxAmmo = raw.maxAmmo;
        isFullAuto = raw.isFullAuto;
        shotDelay = raw.shotDelay;
        shotRange = raw.shotRange != 0 ? raw.shotRange : Mathf.Infinity;
    }
}