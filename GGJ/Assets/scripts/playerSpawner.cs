using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSpawner : MonoBehaviour
{

    public static playerSpawner instance;
    public float width;
    public float numOfPlayers = 2;
    public GameObject playerPrefab;

    public bool showSpawners;
    public bool lobby;

    public Color[] characterColors;

    //onlineManagement
    public playerController[] players;
    public int queuedPlayer;

    protected virtual void Awake()
    { // this might throw things for a loop if its start instead of Awake. keep an eye out to see if stuff happens
        instance = this;


        int[] randomPosition = new int[(int)numOfPlayers];
        randomizePlayerOrder(randomPosition);
        Vector3[] pos = new Vector3[(int)numOfPlayers];
        for (int i = 0; i < numOfPlayers; i++) {
             pos[i] = transform.position - Vector3.right * (width / 2 - width / (numOfPlayers - 1) * randomPosition[i]);
        }
        spawnPlayers(pos);
    }

    protected void randomizePlayerOrder(int[] randomPosition) {
        numOfPlayers = GameManager.instance.numOfPlayers;
        players = new playerController[(int)numOfPlayers];
        if (numOfPlayers > 0)
        {
            for (int i = 0; i < numOfPlayers; i++)
            {
                randomPosition[i] = i;
            }

            for (int i = 0; i < 5; i++)
            {
                int swap1 = Random.Range(0, (int)numOfPlayers);
                int swap2 = Random.Range(0, (int)numOfPlayers);
                int temp = randomPosition[swap1];
                randomPosition[swap1] = randomPosition[swap2];
                randomPosition[swap2] = temp;
            }
        }
    }

    protected void spawnPlayers(Vector3[] pos) {
        for (int i = 0; i < numOfPlayers; i++)
        {

            GameObject player = Instantiate(GameManager.instance.selectedCharacters[i], pos[i], transform.rotation);

            if (TerrainGenerator.instance.shape == Shape.Sphere)
            {
                player.GetComponent<playerController>().centerOfGravity = TerrainGenerator.instance.transform;
                player.GetComponent<playerController>().gravityStrength = 30;
            }

            player.GetComponent<playerController>().playerNum = i;
            //player.GetComponent<playertest>().fullColor = characterColors[i];
            player.GetComponent<SpriteRenderer>().color = characterColors[i];

            if (!lobby)
            {
                player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
            players[i] = player.GetComponent<playerController>();
            if (i < controllerHandler.controlOrder.Count)
            {
                player.GetComponent<playerController>().playerControl = controllerHandler.controlOrder[i];
            }
            else
            {
                switch (i)
                {
                    case 0:
                        player.GetComponent<playerController>().playerControl = "WASD";
                        break;

                    case 1:
                        player.GetComponent<playerController>().playerControl = "Arrow";
                        break;

                    case 2:
                        player.GetComponent<playerController>().playerControl = "Joy1";
                        break;

                    case 3:
                        player.GetComponent<playerController>().playerControl = "Joy2";
                        break;
                }
            }
        }
    }

    public void activatePlayers() {
        foreach (playerController player in players) {
            if (player.GetComponent<Rigidbody2D>() != null)
                player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    protected virtual void OnDrawGizmos()
    {

        if (showSpawners)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(width, 1, 1));

            for (int i = 0; i < numOfPlayers; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position - Vector3.right * (width / 2 - width / (numOfPlayers - 1) * i), 0.5f);
            }
        }
    }
}
