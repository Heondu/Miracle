using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Weapon : MonoBehaviourPunCallbacks
{
    [SerializeField] private Collider[] colliders;
    [SerializeField] private AttackCollision attackCollision;
    private new Rigidbody rigidbody;
    private Transform owner;

    public override void OnPlayerEnteredRoom(Player other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (owner != null)
                photonView.RPC(nameof(Pick), RpcTarget.Others, owner.GetComponent<PhotonView>().ViewID);
        }
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        //if (!PhotonNetwork.IsMasterClient)
        //    rigidbody.isKinematic = true;
    }

    public void Activate()
    {
        SetAllCollisionEnabled(false);
        //if (photonView.IsMine)
            rigidbody.isKinematic = true;
    }

    public void Deactivate()
    {
        SetAllCollisionEnabled(true);
        //if (photonView.IsMine)
            rigidbody.isKinematic = false;
    }

    private void SetAllCollisionEnabled(bool value)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = value;
        }
    }

    public void SetAttackCollisionActive(Entity owner, bool value)
    {
        attackCollision.Setup(owner, value);
    }

    [PunRPC]
    public void Pick(int id)
    {
        PhotonView pv = PhotonView.Find(id);
        if (pv != null)
        {
            //GetComponent<PhotonTransformView>().enabled = false;
            GetComponent<PhotonInterpTransformView>().enabled = false;
            Activate();
            owner = pv.transform;
            transform.SetParent(owner);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one * pv.GetComponent<PlayerWeapon>().scaleFactor;
        }
    }

    [PunRPC]
    public void Drop()
    {
        Deactivate();
        owner = null;
        transform.SetParent(null, true);
        //GetComponent<PhotonTransformView>().enabled = true;
        GetComponent<PhotonInterpTransformView>().enabled = true;
    }

    public bool CanPickup()
    {
        return owner == null;
    }
}
