using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windZoneManager : MonoBehaviour {

    public static windZoneManager instance;
    public bool visible;
    public objectTranslate[] clouds;

    [Range(0, 180)]public float windAngle;
    [HideInInspector] public Vector2 windDirection;
    public GameObject flag;

    public bool randomDirection;
    public GameObject left;
    public GameObject right;

    public float cieling, floor;
    public float bottomOfSceen = 240;
    public windZoneArea[] windZones;

    private void Start() {
        instance = this;
        windDirection = DegreeToVector2(windAngle);
        if (randomDirection) {
            windAngle = 180 * Random.Range(0, 2);
        }

        left.SetActive(windAngle != 180);
        right.SetActive(windAngle == 180);

        Vector3 flagScale = flag.transform.localScale;
        flagScale.x *= (windAngle > 100) ? -1 : 1;
        flag.transform.localScale = flagScale;

        foreach (objectTranslate cloud in clouds)
            cloud.direction = Vector3.right * (windAngle > 100 ? -1 : 1);
    }

    private void Update() {
        if (Application.isPlaying && Time.timeScale > 0) {
            windForce();
        }
    }

    // take all players, and apply the appropriate amount of force to each based off y position
    void windForce() {
        windDirection = DegreeToVector2(windAngle);
        playerController[] players = FindObjectsOfType<playerController>();
        for (int i = 0; i < players.Length; i++) {
            Vector2 newDir = players[i].transform.position;
            newDir += windDirection * getWindSpeed(newDir.y) * 0.005f;
            players[i].transform.position = newDir;
        }
    }

    float getWindSpeed(float position) {
        position -= bottomOfSceen;
        for (int i = 0; i < windZones.Length - 1; i++) {
            if (position < windZones[i + 1].height) { // we've found range
                float windZonePercentage = (position - windZones[i].height) / (windZones[i+1].height - windZones[i].height);
                return Mathf.Lerp(windZones[i].strength, windZones[i + 1].strength, windZonePercentage);
            }
        }

        return 1;
    }

    public static Vector2 RadianToVector2(float radian) {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree) {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    private void OnDrawGizmos() {
        for (int i = 0; i < windZones.Length; i++) {
            if (i != 0)
                windZones[i].height = Mathf.Max(windZones[i].height, windZones[i - 1].height);
            windZones[i].height = Mathf.Max(windZones[i].height, floor);
            windZones[i].height = Mathf.Min(windZones[i].height, cieling);
        }
        Gizmos.color = Color.blue;
        float center = (cieling + floor) / 2 + bottomOfSceen;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, center), new Vector3(4,cieling + floor,1));
        Gizmos.color = Color.red;

        for (int i = 0; i < windZones.Length; i++) {
            center = (windZones[i].height + floor) / 2 + bottomOfSceen;
            Gizmos.DrawWireCube(new Vector3(transform.position.x, center), new Vector3(4, windZones[i].height + floor, 1));
        }
    }
}


[System.Serializable]
public class windZoneArea
{
    public float height;
    public float strength;
}