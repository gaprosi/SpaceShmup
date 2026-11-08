
using UnityEngine;

public class Enemy_3 : Enemy
{
    [Header("Enemy_3 Inscribed Fields")]
    public float lifeTime = 5;
    public Vector2 midpointYRange =
        new Vector2(1.5f, 3);

    public bool drawDebugInfo = true;

    [Header("Enemy_3 Private Fields")]
    [SerializeField] private Vector3[] points;
    [SerializeField] private float birthTime;

    void Start()
    {
        points = new Vector3[3];

        // first point is the starting position
        points[0] = pos;

        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;

        // middle control point
        points[1] = Vector3.zero;
        points[1].x = Random.Range(xMin, xMax);

        float midYMult = Random.Range(
            midpointYRange[0], midpointYRange[1]);

        points[1].y = -bndCheck.camHeight * midYMult;

        // last point is above the screen
        points[2] = Vector3.zero;
        points[2].y = pos.y;
        points[2].x = Random.Range(xMin, xMax);

        birthTime = Time.time;

        if (drawDebugInfo)
        {
            DrawDebug();
        }
    }

    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;

        if (u > 1)
        {
            Destroy(gameObject);
            return;
        }

        // rotate as the ship follows its path
        transform.rotation =
            Quaternion.Euler(u * 180, 0, 0);

        // ease the movement through the curve
        u = u - 0.1f * Mathf.Sin(u * Mathf.PI * 2);

        pos = Utils.Bezier(u, points);
    }

    void DrawDebug()
    {
        Debug.DrawLine(points[0], points[1],
                       Color.cyan, lifeTime);

        Debug.DrawLine(points[1], points[2],
                       Color.yellow, lifeTime);

        float numSections = 20;
        Vector3 prevPoint = points[0];

        for (int i = 1; i < numSections; i++)
        {
            float u = i / numSections;
            Vector3 pt = Utils.Bezier(u, points);

            Color col = Color.Lerp(
                Color.cyan, Color.yellow, u);

            Debug.DrawLine(prevPoint, pt, col, lifeTime);

            prevPoint = pt;
        }
    }
}
