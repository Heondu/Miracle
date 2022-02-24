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
    [SerializeField] private Transform scoreHolder;
    [SerializeField] private GameObject scoreUIPrefab;
    private List<Player> playerList = new List<Player>();

    [Header("Death Match Mode")]
    [SerializeField] private TextMeshProUGUI playerCountText;

    public static bool IsUIControl = false;

    public void CheckGameMode(string gameMode)
    {
        GameModeIs[] gameModes = GetComponentsInChildren<GameModeIs>();
        foreach (GameModeIs gm in gameModes)
        {
            if (gm.gameMode.ToString() == gameMode)
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
        if (!PhotonNetwork.IsConnected)
            return;

        SortScore();
        CreateAndRemoveScoreUI();
        SetupScoreUI();
    }

    private void SortScore()
    {
        playerList.Clear();

        Dictionary<int, Player> playerDict = PhotonNetwork.CurrentRoom.Players;
        foreach (int key in playerDict.Keys)
        {
            playerList.Add(playerDict[key]);
        }

        for (int i = 0; i < playerList.Count; i++)
        {
            for (int j = 1; j < playerList.Count - i; j++)
            {
                if (playerList[j].GetScore() < playerList[j - 1].GetScore())
                {
                    Player temp = playerList[j - 1];
                    playerList[j - 1] = playerList[j];
                    playerList[j] = temp;
                }
            }
        }
    }

    private void CreateAndRemoveScoreUI()
    {
        ScoreText[] scoreTexts = scoreHolder.GetComponentsInChildren<ScoreText>();
        int i = 0;
        for (; i < playerList.Count; i++)
        {
            if (scoreTexts.Length <= i)
                Instantiate(scoreUIPrefab, scoreHolder);
        }

        scoreTexts = scoreHolder.GetComponentsInChildren<ScoreText>();
        for (; i < scoreTexts.Length; i++)
        {
            Destroy(scoreTexts[i].gameObject);
        }
    }

    private void SetupScoreUI()
    {
        ScoreText[] scoreTexts = scoreHolder.GetComponentsInChildren<ScoreText>();
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            scoreTexts[i].Setup((i + 1).ToString(), playerList[i].NickName, playerList[i].GetScore().ToString());
        }
    }
}
