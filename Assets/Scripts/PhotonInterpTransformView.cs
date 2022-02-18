using UnityEngine;
using Photon.Pun;

public class PhotonInterpTransformView : MonoBehaviourPun, IPunObservable
{
    private Vector3 nextPosition;
    private Quaternion nextRotation;

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            nextPosition = (Vector3)stream.ReceiveNext();
            nextRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    #endregion

    private void Update()
    {
        if (!photonView.IsMine)
            InterpTransform();
    }

    private void InterpPosition()
    {
        transform.position = Vector3.Slerp(transform.position, nextPosition, Time.deltaTime * 2);
    }

    private void InterpRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, nextRotation, Time.deltaTime * 2);
    }

    private void InterpTransform()
    {
        InterpPosition();
        InterpRotation();
    }
}
