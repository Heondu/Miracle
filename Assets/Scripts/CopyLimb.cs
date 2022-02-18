using UnityEngine;
using Photon.Pun;

public class CopyLimb : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private Transform targetLimb;
    private ConfigurableJoint configurableJoint;
    private new Rigidbody rigidbody;

    private Quaternion targetInitialRotation;

    #region IPunObservable implementation
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
        }
        else
        {
        }
    }
    
    #endregion

    private void Awake()
    {
        configurableJoint = GetComponent<ConfigurableJoint>();
        rigidbody = GetComponent<Rigidbody>();

        if (!photonView.IsMine)
            rigidbody.isKinematic = true;
    }

    private void Start()
    {
        targetInitialRotation = targetLimb.transform.localRotation;
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        CopyRotation();
    }

    private void CopyRotation()
    {
        configurableJoint.targetRotation = Quaternion.Inverse(targetLimb.localRotation) * targetInitialRotation;
    }
}
