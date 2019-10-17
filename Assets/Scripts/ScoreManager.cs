using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ScoreManager : NetworkBehaviour
{
    public Text player1Score;
    public Text player2Score;

    //192.168.43.1
    //localhost

    public void UpdateScoreboard()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length >= 2)
        {
            player1Score.text = players[0].GetComponent<PlayerController>().score.ToString();
            player2Score.text = players[1].GetComponent<PlayerController>().score.ToString();
        }
    }

    public void ResetScoreboard()
    {
        player1Score.text = "0";
        player2Score.text = "0";
    }
}
