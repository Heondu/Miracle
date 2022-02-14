using UnityEngine;
using Photon.Pun;

public class CopyLimb : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private Transform targetLimb;
    private ConfigurableJoint configurableJoint;

    private Quaternion targetInitialRotation;

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(configurableJoint.targetRotation);
        }
        else
        {
            configurableJoint.targetRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    #endregion

    private void Awake()
    {
        configurableJoint = GetComponent<ConfigurableJoint>();
    }

    private void Start()
    {
        targetInitialRotation = targetLimb.transform.localRotation;
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        configurableJoint.targetRotation = CopyRotation();
    }

    private Quaternion CopyRotation()
    {
        return Quaternion.Inverse(targetLimb.localRotation) * targetInitialRotation;
    }
}
