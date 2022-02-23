using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GrabbableObject : MonoBehaviourPunCallbacks
{
    public override void OnPlayerEnteredRoom(Player other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            FixedJoint[] fixedJoints = GetComponents<FixedJoint>();
            for (int i = 0; i < fixedJoints.Length; i++)
            {
                photonView.RPC(nameof(Activate), RpcTarget.Others, fixedJoints[i].connectedBody.GetComponent<PhotonView>().ViewID);
            }
        }
    }

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!PhotonNetwork.IsMasterClient)
        {
            //rb.isKinematic = true;
        }
    }

    [PunRPC]
    public void Activate(int playerID)
    {
        PhotonView playerPV = PhotonView.Find(playerID);
        if (playerPV != null)
        {
            //GetComponent<PhotonTransformView>().enabled = false;
            if (TryGetComponent(out PhotonInterpTransformView pisv))
                pisv.enabled = false;
            if (playerPV.IsMine)
            {
                //rb.isKinematic = false;
            }

            Grab grab = playerPV.GetComponent<Grab>();
            FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = grab.rigidbody;
            fixedJoint.breakForce = Mathf.Infinity;
            grab.fixedJoint = fixedJoint;
            grab.currentGrabObj = this;
            grab.isGrabbing = true;
        }
    }

    [PunRPC]
    public void Deactivate(int playerID)
    {
        PhotonView playerPV = PhotonView.Find(playerID);
        if (playerPV != null)
        {
            //GetComponent<PhotonTransformView>().enabled = true;
            if (TryGetComponent(out PhotonInterpTransformView pisv))
                pisv.enabled = true;
            if (playerPV.IsMine)
            {
                //rb.isKinematic = true;
            }

            Grab grab = playerPV.GetComponent<Grab>();
            Destroy(grab.fixedJoint);
            grab.currentGrabObj = null;
            grab.isGrabbing = false;
        }
    }
}
