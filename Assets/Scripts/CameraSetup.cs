using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class CameraSetup : MonoBehaviourPun
{
    [SerializeField] private Transform root;

    private void Start()
    {
        if (photonView.IsMine)
        {
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
            followCam.Follow = root;
            followCam.LookAt = root;

            Camera.main.GetComponent<MakeTransparent>().Setup(root);
        }
    }
}
