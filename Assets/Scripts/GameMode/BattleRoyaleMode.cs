using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class BattleRoyaleMode : GameMode, IPunObservable
{
    private bool isAlone = true;

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
        }
        else
        {
        }
    }

    #endregion

    public override void Init(Entity player)
    {
        base.Init(player);

        player.onDeath.AddListener(OnDeath);
    }

    private void OnDeath(Entity player)
    {
        //player.gameObject.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

    public void Update()
    {
        UIManager.Instance.UpdatePlayerCountText(PhotonNetwork.PlayerList.Length);

        if (PhotonNetwork.PlayerList.Length > 1)
        {
            isAlone = false;
        }

        if (!isAlone && PhotonNetwork.PlayerList.Length == 1)
        {
            PlayerPrefs.SetString("Winner", PhotonNetwork.NickName);
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("Result DeathMatch");
        }
    }
}
