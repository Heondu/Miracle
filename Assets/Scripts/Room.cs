using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public string roomName = "";
    public int playerCount = 0;
    public int maxPlayer = 0;

    [System.NonSerialized]
    public Text roomDataTxt;

    private void Awake()
    {
        roomDataTxt = GetComponentInChildren<Text>();
    }
    
    public void UpdateInfo()
    {
        roomDataTxt.text = string.Format(" {0} [{1} / {2}]", roomName, playerCount.ToString("00"), maxPlayer.ToString("00"));
    }
}
