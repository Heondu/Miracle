using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class Entity : MonoBehaviourPunCallbacks, IPunObservable
{
    private Status status;
    public Status Status => status;
    
    [SerializeField] private HPViewer hpViewer;
    [SerializeField] private Transform root;
    public Transform Root => root;
    [SerializeField] private float dieY;

    public UnityEvent<Entity> onDeath = new UnityEvent<Entity>();

    public static GameObject LocalPlayerInstance;

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(status.CurrentHP);
        }
        else
        {
            status.CurrentHP = (float)stream.ReceiveNext();
        }
    }

    #endregion

    private void Awake()
    {
        status = GetComponent<Status>();
        status.onModifierUpdate.AddListener(hpViewer.UpdateHP);

        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }
    }

    private void Update()
    {
        if (root.position.y <= dieY)
        {
            onDeath.Invoke(this);
        }
    }

    public void TakeDamage(float damage)
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("OnDamage", RpcTarget.All, damage);
        }
        else
        {
            OnDamage(damage);
        }
    }

    [PunRPC]
    public void OnDamage(float damage)
    {
        if (status.CurrentHP == 0)
        {
            onDeath.Invoke(this);
        }

        status.CurrentHP -= (int)damage;
    }

    public void RestoreHP()
    {
        status.CurrentHP = status.MaxHP;
    }
}