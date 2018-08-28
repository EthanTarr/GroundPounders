using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Keeps track of and handles all the achievements for the game
[RequireComponent(typeof(SteamAchievements))]
public class AchievemtManager : MonoBehaviour {

    private SteamAchievements steamAch;
    public static AchievemtManager instance;

    //ACHIEVEMENT NAMES
    public static string LOW_ROUND_COUNT = "25 games";
    public static string MID_ROUND_COUNT = "50 games";
    public static string HIGH_ROUND_COUNT = "100 games";
    public static string TUTORIAL = "complete tutorial";
    public static string DOG_MODE = "dog mode";
    public static string INTERCEPTION = "50 bumps";
    public static string ALLMAPS = "all maps";

    // for achievments based around playing a certain number of rounds
    [Space]
    public int totalRoundsPlayed;
    public int testRoundsAchievement = 10;
    public int lowRoundAchievment = 25;
    public int medianRoundAchievement = 50;
    public int highRoundAchievement = 100;

    // for achievements based around playing all the maps
    [Space]
    public bool playedCherry;
    public bool playedCity;
    public bool playedTron;
    public bool playedIceberg;
    public bool playedSpace;

    // achievement around finding dog mode
    [Space]
    public bool foundDogMode;

    // achievement for completeing tutorial
    [Space]
    public bool trainingGrounds;

    // getting intercept kills
    [Space]
    public int intercepts;
    public int interceptAchievement = 50;

    public float totalNumberOfGames;

    // Use this for initialization
    void Start () {
        if (instance != null) Destroy(this.gameObject);

        steamAch = GetComponentInParent<SteamAchievements>();
        totalRoundsPlayed = PlayerPrefs.GetInt("rounds", 0);
        intercepts = PlayerPrefs.GetInt("intercepts", 0);

        playedCherry = PlayerPrefs.GetInt("cherry", 0) == 1;
        playedCity = PlayerPrefs.GetInt("city", 0) == 1;
        playedTron = PlayerPrefs.GetInt("tron", 0) == 1;
        playedIceberg = PlayerPrefs.GetInt("iceberg", 0) == 1;
        playedSpace = PlayerPrefs.GetInt("space", 0) == 1;

        instance = this;
	}

    public void updateRoundCount() {
        totalRoundsPlayed++;
        if (totalRoundsPlayed > testRoundsAchievement)
            steamAch.TriggerAchievement("10 rounds");

        if (totalRoundsPlayed > lowRoundAchievment)
            steamAch.TriggerAchievement(LOW_ROUND_COUNT);

        if (totalRoundsPlayed > medianRoundAchievement)
            steamAch.TriggerAchievement(MID_ROUND_COUNT);

        if (totalRoundsPlayed > highRoundAchievement)
            steamAch.TriggerAchievement(HIGH_ROUND_COUNT);

         PlayerPrefs.SetInt("rounds", totalRoundsPlayed);
    }

    public void updateMapStats(int loadedLevel) {
        print("loded level is : " + loadedLevel);
        switch (loadedLevel) {
            case 3:
                playedCherry = true;
                break;
            case 4:
                playedCity = true;
                break;
            case 5:
                playedTron = true;
                break;
            case 6:
                playedIceberg = true;
                break;
            case 7:
                playedSpace = true;
                break;
        }
    }

    public void updateInterceptCount() {
        intercepts++;
        if(intercepts > interceptAchievement)
            steamAch.TriggerAchievement(INTERCEPTION);
        PlayerPrefs.SetInt("intercepts", intercepts);

        PlayerPrefs.SetInt("cherry", playedCherry ? 1 : 0);
        PlayerPrefs.SetInt("city", playedCity ? 1 : 0);
        PlayerPrefs.SetInt("tron", playedTron ? 1 : 0);
        PlayerPrefs.SetInt("iceberg", playedIceberg ? 1 : 0);
        PlayerPrefs.SetInt("space", playedSpace ? 1 : 0);
    }

    public void checkMaps() {
        if (playedCherry && playedCity && playedTron && playedIceberg && playedSpace) {
            steamAch.TriggerAchievement(ALLMAPS);
        }
    }

    public void gameUpdate() {

    }
}
