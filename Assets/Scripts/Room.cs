using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Room : MonoBehaviour
{
    public string roomName = "";
    public string roomMode = "";
    public string roomMap = "";
    public int playerCount = 0;
    public int maxPlayer = 0;

    public TextMeshProUGUI  textRoomName;
    public TextMeshProUGUI  textGameModeAndRoom;
    public TextMeshProUGUI  textPlayer;
    public Image            imageLock;
    public Image            imageUnlock;
    
    public void UpdateInfo()
    {
        string[] roomData = roomName.Split('*');
        textRoomName.text = roomData[0];
        textGameModeAndRoom.text = $"{roomMode}/{roomMap}";
        textPlayer.text = $"{playerCount}/{maxPlayer}";
        if (roomData.Length > 1)
        {
            imageLock.gameObject.SetActive(true);
            imageUnlock.gameObject.SetActive(false);
        }
        else
        {
            imageLock.gameObject.SetActive(false);
            imageUnlock.gameObject.SetActive(true);
        }
    }
}
