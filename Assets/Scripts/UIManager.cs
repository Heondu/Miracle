using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    [Header("Basic Mode")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Death Match Mode")]
    [SerializeField] private TextMeshProUGUI playerCountText;

    public void CheckGameMode(EGameMode gameMode)
    {
        GameModeIs[] gameModes = GetComponentsInChildren<GameModeIs>();
        foreach (GameModeIs gm in gameModes)
        {
            if (gm.gameMode == gameMode)
            {
                gm.gameObject.SetActive(true);
            }
            else
            {
                gm.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateTimeText(float time)
    {
        int m = (int)(time % 3600) / 60;
        int s = (int)(time % 3600) % 60;
        timeText.text = $"{m} : {s}";
    }

    public void UpdatePlayerCountText(int count)
    {
        playerCountText.text = count.ToString();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "";
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            scoreText.text += $"{p.GetScore()}. {p.NickName}\n";
        }
    }
}
