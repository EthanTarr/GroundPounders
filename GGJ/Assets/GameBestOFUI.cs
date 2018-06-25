using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBestOFUI : MonoBehaviour {

    public void increaseGames(int increment) {
        GameManager.instance.increaseGames(increment);
    }
}
