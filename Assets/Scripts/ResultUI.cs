using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText;

    private void Start()
    {
        winnerText.text = $"Winner is {PlayerPrefs.GetString("Winner")}!";
    }
}
