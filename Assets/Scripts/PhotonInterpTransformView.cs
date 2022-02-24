using System.Collections;
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
        {
            InterpTransform();
        }
    }

    private void InterpPosition()
    {
        if (Mathf.Abs(Vector3.Magnitude(transform.position - nextPosition)) < 0.1f)
            transform.position = nextPosition;

        transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * 10);
    }

    private void InterpRotation()
    {
        if (Mathf.Abs(Vector3.Magnitude(transform.eulerAngles - nextRotation.eulerAngles)) < 0.1f)
            transform.rotation = nextRotation;

        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Time.deltaTime * 10);
    }

    private void InterpTransform()
    {
        InterpPosition();
        InterpRotation();
    }
}
