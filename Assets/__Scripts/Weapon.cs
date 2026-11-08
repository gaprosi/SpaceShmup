
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
    static public Transform PROJECTILE_ANCHOR;

    [Header("Dynamic")]
    [SerializeField]
    [Tooltip("Setting this manually while playing does not work properly.")]
    private eWeaponType _type = eWeaponType.none;

    public WeaponDefinition def;
    public float nextShotTime;

    private GameObject weaponModel;
    private Transform shotPointTrans;

    void Start()
    {
        // create the projectile parent if needed
        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        // the first child should be ShotPoint
        shotPointTrans = transform.GetChild(0);

        SetType(_type);

        // connect this weapon to the Hero's firing event
        Hero hero = GetComponentInParent<Hero>();

        if (hero != null)
        {
            hero.fireEvent += Fire;
        }
    }

    public eWeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }

    public void SetType(eWeaponType wt)
    {
        _type = wt;

        if (type == eWeaponType.none)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        def = Main.GET_WEAPON_DEFINITION(_type);

        // replace the old weapon model
        if (weaponModel != null)
        {
            Destroy(weaponModel);
        }

        weaponModel = Instantiate(
            def.weaponModelPrefab, transform
        );

        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localScale = Vector3.one;

        nextShotTime = 0;
    }

    private void Fire()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (Time.time < nextShotTime)
            return;

        ProjectileHero p;
        Vector3 vel = Vector3.up * def.velocity;

        switch (type)
        {
            case eWeaponType.blaster:
                p = MakeProjectile();
                p.vel = vel;
                break;

            case eWeaponType.spread:
                // middle projectile
                p = MakeProjectile();
                p.vel = vel;

                // right projectile
                p = MakeProjectile();
                p.transform.rotation =
                    Quaternion.AngleAxis(10, Vector3.back);
                p.vel = p.transform.rotation * vel;

                // left projectile
                p = MakeProjectile();
                p.transform.rotation =
                    Quaternion.AngleAxis(-10, Vector3.back);
                p.vel = p.transform.rotation * vel;
                break;
        }
    }

    private ProjectileHero MakeProjectile()
    {
        GameObject go = Instantiate(
            def.projectilePrefab, PROJECTILE_ANCHOR
        );

        ProjectileHero p = go.GetComponent<ProjectileHero>();

        Vector3 pos = shotPointTrans.position;
        pos.z = 0;

        p.transform.position = pos;
        p.type = type;

        nextShotTime = Time.time + def.delayBetweenShots;

        return p;
    }
}
