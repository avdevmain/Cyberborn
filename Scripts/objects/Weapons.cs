using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Weapon")]
public class Weapons : Item
{

    public AnimatorOverrideController projectileAOC;

    public Sprite projectileIcon;
    public float damage;
    public float shieldPiercing;
    public float projectileSpeed;
    public int projectileAmount;
    public float projectileDelay;

    public WeaponType weapon_type;
    public ProjectileType projectile_Type;

    public enum ProjectileType
    {
        Single,
        Scatter,
        Burst,
        Beam
    }

    public enum WeaponType
    {
        Kinetic,
        Energy,
        Missile,
        Special
    }

    /*
    
    public enum Trait
    {
    Standard,
    Overclocked,
    Damaged,
    Wrecked,
    Modded
    }

    public Trait weaponTrait;
    
     */


}
