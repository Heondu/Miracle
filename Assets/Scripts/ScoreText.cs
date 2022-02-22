using UnityEngine;
using TMPro;
using Photon.Pun;

public class ScoreText : MonoBehaviourPun
{
    [SerializeField] private TextMeshProUGUI textRanking;
    [SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] private TextMeshProUGUI textDeathCount;

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(textRanking.text);
            stream.SendNext(textNickname.text);
            stream.SendNext(textDeathCount.text);
        }
        else
        {
            Setup((string)stream.ReceiveNext(), (string)stream.ReceiveNext(), (string)stream.ReceiveNext());
        }
    }

    #endregion

    public void Setup(string ranking, string nickname, string deathCount)
    {
        textRanking.text = ranking;
        textNickname.text = nickname;
        textDeathCount.text = deathCount;
    }
}
