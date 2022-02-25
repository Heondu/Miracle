using UnityEngine;
using TMPro;
using Photon.Pun;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText;

    private void Start()
    {
        winnerText.text = $"The winner is {PlayerPrefs.GetString("Winner")}!";
    }
}
