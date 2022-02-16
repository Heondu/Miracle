using UnityEngine;
using TMPro;
using Photon.Pun;

public class ScoreText : MonoBehaviourPun
{
    private TextMeshProUGUI scoreText;

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(scoreText.text);
        }
        else
        {
            scoreText.text = (string)stream.ReceiveNext();
        }
    }

    #endregion

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }
}
