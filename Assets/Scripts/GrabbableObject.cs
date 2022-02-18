using UnityEngine;
using Photon.Pun;

public class GrabbableObject : MonoBehaviourPun
{
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
            GetComponent<PhotonInterpTransformView>().enabled = false;
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
            GetComponent<PhotonInterpTransformView>().enabled = true;
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
