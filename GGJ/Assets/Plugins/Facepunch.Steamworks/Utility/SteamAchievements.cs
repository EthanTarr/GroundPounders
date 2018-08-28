using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Linq;
using Facepunch.Steamworks;

public class SteamAchievements : MonoBehaviour {

    public uint AppId;
    private Facepunch.Steamworks.Client client;
    public static SteamAchievements instance;

	// Use this for initialization
	void Start () {
        if (instance != null)
            Destroy(this.gameObject);

        instance = this;
        GameObject.DontDestroyOnLoad(gameObject);

        if (AppId == 0)
            throw new System.Exception("You need to set the AppId to your game");

        Facepunch.Steamworks.Config.ForUnity(Application.platform.ToString());

        try {
            System.IO.File.WriteAllText("steam_appid.txt", AppId.ToString());
        } catch (System.Exception e) {
            Debug.LogWarning("Couldn't write steam_appid.txt: " + e.Message);
        }

        // Create the client
        client = new Facepunch.Steamworks.Client(AppId);

        if (!client.IsValid) {
            client = null;
            Debug.LogWarning("Couldn't initialize Steam");
            return;
        }
        Debug.Log("Steam Initialized: " + client.Username + " / " + client.SteamId);
    }
	
	// Update is called once per frame
	void Update () {
        if (client == null)
            return;

        try {
            UnityEngine.Profiling.Profiler.BeginSample("Steam Update");
            //getAchievementCount();
            client.Update();
        } finally {
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }

    [ContextMenu("print achievements")]
    public void GetAchievementCount() {
        if (client == null) { return; }

        foreach (Achievement ach in client.Achievements.All) {
            print("" + ach.Id + ":" + ach.Name + "/" + ach.Description + "/" + ach.State + "/" + ach.UnlockTime + "/" + ach.GlobalUnlockedPercentage);
        }

    }

    [ContextMenu("test achievements")]
    public void TestTriggerAchievement() {
        TriggerAchievement("Test");
        GetAchievementCount();
    }

    public void TriggerAchievement(string achievementId) {
        if (client != null) {
            client.Achievements.Trigger(achievementId);
            client.Stats.StoreStats();
            print("Got achievievement: " + achievementId);
        }
    }

    private void OnDestroy() {
        if (client != null) {
            client.Dispose();
            client = null;
        }
    }
}
