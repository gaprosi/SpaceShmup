
using UnityEngine;

public enum eWeaponType
{
    none,
    blaster,
    spread,
    phaser,
    missile,
    laser,
    shield
}

[System.Serializable]
public class WeaponDefinition
{
    public eWeaponType type = eWeaponType.none;

    [Tooltip("Letter to show on the PowerUp Cube")]
    public string letter;

    [Tooltip("Color of PowerUp Cube")]
    public Color powerUpColor = Color.white;

    [Tooltip("Weapon model attached to the Hero")]
    public GameObject weaponModelPrefab;

    [Tooltip("Projectile prefab")]
    public GameObject projectilePrefab;

    [Tooltip("Projectile color")]
    public Color projectileColor = Color.white;

    [Tooltip("Damage from one projectile")]
    public float damageOnHit = 0;

    [Tooltip("Damage per second")]
    public float damagePerSec = 0;

    [Tooltip("Seconds between shots")]
    public float delayBetweenShots = 0;

    [Tooltip("Projectile speed")]
    public float velocity = 50;
}

public class Weapon : MonoBehaviour
{

}
