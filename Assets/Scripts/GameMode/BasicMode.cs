using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class BasicMode : GameMode, IPunObservable
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
    }

    private void AddDeathCount(Entity player)
    {
        PhotonNetwork.LocalPlayer.AddScore(-1);
        player.RestoreHP();
        player.Root.position = new Vector3(0, 5, 0);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            PhotonNetwork.LocalPlayer.AddScore(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            PhotonNetwork.LocalPlayer.AddScore(-1);

        UpdateScoreUI();

        if (PhotonNetwork.IsMasterClient)
        {
            elapsedTime += Time.deltaTime;
            UIManager.Instance.UpdateTimeText(limitTime - elapsedTime);
        }

        if (limitTime - elapsedTime <= 0)
        {
            photonView.RPC(nameof(LeaveRoom), RpcTarget.All);
        }
    }

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
