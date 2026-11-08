
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Inscribed")]
    public Transform playerTrans;
    public Transform[] panels;

    public float scrollSpeed = -30f;
    public float motionMult = 0.25f;

    private float panelHt;
    private float depth;

    void Start()
    {
        panelHt = panels[0].localScale.y;
        depth = panels[0].position.z;

        // start with two panels next to each other
        panels[0].position = new Vector3(0, 0, depth);
        panels[1].position = new Vector3(0, panelHt, depth);
    }

    void Update()
    {
        float tX = 0;

        // scroll the foreground downward
        float tY = Time.time * scrollSpeed % panelHt
                   + panelHt * 0.5f;

        // move the stars slightly when the player moves
        if (playerTrans != null)
        {
            tX = -playerTrans.position.x * motionMult;
        }

        panels[0].position = new Vector3(tX, tY, depth);

        // keep the second panel next to the first
        if (tY >= 0)
        {
            panels[1].position =
                new Vector3(tX, tY - panelHt, depth);
        }
        else
        {
            panels[1].position =
                new Vector3(tX, tY + panelHt, depth);
        }
    }
}
