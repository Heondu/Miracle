using UnityEngine;
using Photon.Pun;

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

    private void AddDeathCount()
    {
        deathCount++;
        Debug.Log(deathCount);
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        elapsedTime += Time.deltaTime;
        UIManager.Instance.UpdateTimeText(limitTime - elapsedTime);
    }
}
