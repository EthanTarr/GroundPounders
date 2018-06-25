using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSphericalSpawner : playerSpawner {

    public float radius;
    [Range(0, 360)]
    public float offset;

    protected override void Awake() {
        instance = this;

        int[] randomPosition = new int[(int)numOfPlayers];
        randomizePlayerOrder(randomPosition);

        Vector3[] pos = new Vector3[(int)numOfPlayers];

        for (int i = 0; i < numOfPlayers; i++) {
            pos[i] = transform.position + (Vector3)DegreeToVector2(offset + (width * i) / (numOfPlayers - 1)) * radius;
        }

        spawnPlayers(pos);
    }

    protected override void OnDrawGizmos() {
        if (showSpawners)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position + (Vector3)DegreeToVector2(offset) * (radius - 1), transform.position + (Vector3)DegreeToVector2(offset) * (radius + 1));
            Gizmos.DrawLine(transform.position + (Vector3)DegreeToVector2(offset + width) * (radius - 1), transform.position + (Vector3)DegreeToVector2(offset + width) * (radius + 1));
            
            for (int i = 0; i < numOfPlayers; i++)  {
                Gizmos.color = Color.blue;
                //pos[i] = transform.position - Vector3.right * (width / 2 - width / (numOfPlayers - 1) * randomPosition[i]);
                Gizmos.DrawWireSphere(transform.position + (Vector3)DegreeToVector2(offset + (width * i) / (numOfPlayers - 1)) * radius, 0.5f);
            }
        }
    }

    public static Vector2 RadianToVector2(float radian) {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree) {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
}
