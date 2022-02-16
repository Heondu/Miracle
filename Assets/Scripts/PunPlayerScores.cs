using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PunPlayerScores : MonoBehaviour
{
    public const string PlayerScoreProp = "score";
}

public static class ScoreExtensions
{
    public static void SetScore(this Player player, int newScore)
    {
        Hashtable score = new Hashtable();
        score[PunPlayerScores.PlayerScoreProp] = newScore;

        player.SetCustomProperties(score);
    }

    public static void AddScore(this Player player, int addScore)
    {
        int current = player.GetScore();
        current += addScore;

        Hashtable score = new Hashtable();
        score[PunPlayerScores.PlayerScoreProp] = current;

        player.SetCustomProperties(score);
    }

    public static int GetScore(this Player player)
    {
        object score;
        if (player.CustomProperties.TryGetValue(PunPlayerScores.PlayerScoreProp, out score))
        {
            return (int)score;
        }

        return 0;
    }
}
