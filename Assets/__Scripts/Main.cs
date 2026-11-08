
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Main : MonoBehaviour
{
    static private Main S;

    static private Dictionary<eWeaponType, WeaponDefinition>
        WEAP_DICT;

    [Header("Inscribed")]
    public bool spawnEnemies = true;
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyInsetDefault = 1.5f;
    public float gameRestartDelay = 2f;
    
public GameObject prefabPowerUp;

public eWeaponType[] powerUpFrequency = new eWeaponType[]
{
    eWeaponType.blaster,
    eWeaponType.blaster,
    eWeaponType.spread,
    eWeaponType.shield
};

    public WeaponDefinition[] weaponDefinitions;
    
[Header("Score")]
public TMP_Text scoreText;

private int totalScore = 0;

    private BoundsCheck bndCheck;

    void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        UpdateScoreDisplay();
        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);

        WEAP_DICT =
            new Dictionary<eWeaponType, WeaponDefinition>();

        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    public void SpawnEnemy()
    {
        if (!spawnEnemies)
        {
            Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
            return;
        }

        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate(prefabEnemies[ndx]);

        float enemyInset = enemyInsetDefault;

        BoundsCheck enemyBnd = go.GetComponent<BoundsCheck>();

        if (enemyBnd != null)
        {
            enemyInset = Mathf.Abs(enemyBnd.radius);
        }

        Vector3 pos = Vector3.zero;

        float xMin = -bndCheck.camWidth + enemyInset;
        float xMax = bndCheck.camWidth - enemyInset;

        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyInset;

        go.transform.position = pos;

        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
    }

    void DelayedRestart()
    {
        Invoke(nameof(Restart), gameRestartDelay);
    }

    void Restart()
    {
        SceneManager.LoadScene("__Scene_0");
    }

    static public void HERO_DIED()
    {
        S.DelayedRestart();
    }

    static public WeaponDefinition GET_WEAPON_DEFINITION(
        eWeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt))
        {
            return WEAP_DICT[wt];
        }

        return new WeaponDefinition();
    }
    
static public void SHIP_DESTROYED(Enemy e)
{
    S.AddScore(e.score);
    // randomly decide whether to drop a powerup
    if (Random.value <= e.powerUpDropChance)
    {
        // choose a random powerup type
        int ndx = Random.Range(
            0, S.powerUpFrequency.Length
        );

        eWeaponType pUpType = S.powerUpFrequency[ndx];

        // create the powerup
        GameObject go = Instantiate(S.prefabPowerUp);

        PowerUp pUp = go.GetComponent<PowerUp>();

        // assign its type
        pUp.SetType(pUpType);

        // place it where the enemy was destroyed
        pUp.transform.position = e.transform.position;
    }
}


private void AddScore(int points)
{
    totalScore += points;
    UpdateScoreDisplay();
}

private void UpdateScoreDisplay()
{
    if (scoreText != null)
    {
        scoreText.text = "Score: " + totalScore;
    }
}

}
