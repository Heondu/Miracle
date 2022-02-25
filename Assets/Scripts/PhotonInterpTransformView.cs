using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PhotonInterpTransformView : MonoBehaviourPun, IPunObservable
{
    private Vector3 oldPosition = Vector3.zero;
    private Quaternion oldRotation = Quaternion.Euler(Vector3.zero);
    private Vector3 nextPosition = Vector3.zero;
    private Quaternion nextRotation = Quaternion.Euler(Vector3.zero);
    private float deltaTime = 0;

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
            deltaTime = 0;
            oldPosition = transform.position;
            oldRotation = transform.rotation;
            nextPosition = (Vector3)stream.ReceiveNext();
            nextRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    #endregion

    private void Update()
    {
        if (photonView.IsMine)
            return;

        if (deltaTime > 1)
            return;

        deltaTime += Time.deltaTime * 10;

        if (oldPosition != nextPosition)
            InterpPosition();
        if (oldRotation != nextRotation)
            InterpRotation();
    }

    private void InterpPosition()
    {
        transform.position = Vector3.Lerp(oldPosition, nextPosition, deltaTime);
    }

    private void InterpRotation()
    {
        transform.rotation = Quaternion.Lerp(oldRotation, nextRotation, deltaTime);
    }
}
