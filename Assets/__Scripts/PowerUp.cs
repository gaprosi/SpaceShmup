
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class PowerUp : MonoBehaviour
{
    [Header("Inscribed")]
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(0.25f, 2);
    public float lifeTime = 10;
    public float fadeTime = 4;

    [Header("Dynamic")]
    [SerializeField]
    private eWeaponType _type = eWeaponType.none;

    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Material cubeMat;

    void Awake()
    {
        cube = transform.GetChild(0).gameObject;

        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();

        cubeMat = cube.GetComponent<Renderer>().material;

        Vector3 vel = Random.onUnitSphere;
        vel.z = 0;
        vel.Normalize();

        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        transform.rotation = Quaternion.identity;

        rotPerSecond = new Vector3(
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y)
        );

        birthTime = Time.time;
    }

    void Update()
    {
        // rotate the cube
        cube.transform.rotation =
            Quaternion.Euler(rotPerSecond * Time.time);

        // calculate fade amount
        float u = (Time.time - (birthTime + lifeTime))
                  / fadeTime;

        if (u >= 1)
        {
            Destroy(gameObject);
            return;
        }

        if (u > 0)
        {
            Color c = cubeMat.color;
            c.a = 1f - u;
            cubeMat.color = c;

            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if (!bndCheck.isOnScreen)
        {
            Destroy(gameObject);
        }
    }

    public eWeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }

    public void SetType(eWeaponType wt)
    {
        WeaponDefinition def =
            Main.GET_WEAPON_DEFINITION(wt);

        cubeMat.color = def.powerUpColor;
        letter.text = def.letter;

        _type = wt;
    }

    public void AbsorbedBy(GameObject target)
    {
        Destroy(gameObject);
    }
}
