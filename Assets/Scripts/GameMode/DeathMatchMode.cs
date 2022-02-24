using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class DeathMatchMode : GameMode, IPunObservable
{
    [SerializeField] private float limitTime;
    private float elapsedTime;

    private int deathCount;

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(limitTime - elapsedTime);
        }
        else
        {
            float leftTime = (float)stream.ReceiveNext();
            UIManager.Instance.UpdateTimeText(leftTime);
        }
    }

    #endregion

    public override void Init(Entity player)
    {
        base.Init(player);
        PhotonNetwork.LocalPlayer.SetScore(0);
        player.onDeath.AddListener(AddDeathCount);
        //photonView.RPC(nameof(UpdateScoreUI), RpcTarget.All);
    }

    private void AddDeathCount(Entity player)
    {
        PhotonNetwork.LocalPlayer.AddScore(1);
        player.Setup();
        //photonView.RPC(nameof(UpdateScoreUI), RpcTarget.All);
    }

    public void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            elapsedTime += Time.deltaTime;
            UIManager.Instance.UpdateTimeText(limitTime - elapsedTime);
        }

        if (limitTime - elapsedTime <= 0)
        {
            photonView.RPC(nameof(LeaveRoom), RpcTarget.All);
        }

        UpdateScoreUI();
    }

    [PunRPC]
    private void UpdateScoreUI()
    {
        UIManager.Instance.UpdateScoreText();
    }

    [PunRPC]
    private void LeaveRoom()
    {
        PlayerPrefs.SetString("Winner", GetWinner().NickName);
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Result Basic");
    }

    private Player GetWinner()
    {
        Player player = null;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (player == null)
                player = p;

            if (player.GetScore() < p.GetScore())
                player = p;
        }

        return player;
    }
}
