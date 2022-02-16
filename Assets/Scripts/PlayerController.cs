using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Transform root;
    [SerializeField] private Rigidbody pelvis;
    [SerializeField] private Animator animator;
    [SerializeField] private Grab grabL;
    [SerializeField] private Grab grabR;

    [HideInInspector] public bool isGrounded;

    private PlayerInput playerInput;

    private Vector3 nextPosition;
    private Quaternion nextRotation;

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if(stream.IsWriting)
        //{
        //    stream.SendNext(root.position);
        //    stream.SendNext(root.rotation);
        //}
        //else
        //{
        //    root.position = (Vector3)stream.ReceiveNext();
        //    root.rotation = (Quaternion)stream.ReceiveNext();
        //}
    }

    #endregion

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            UpdateRotate();
            UpdateJump();
            UpdateGrab();
            UpdateAnim();
        }
        else
        {
            //InterpPosition();
            //InterpRotation();
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            UpdateMove();
        }
    }

    private void UpdateRotate()
    {
        if (playerInput.Move != Vector3.zero)
            Rotate(playerInput.Move);
            //photonView.RPC(nameof(Rotate), RpcTarget.All, playerInput.Move);
    }

    [PunRPC]
    private void Rotate(Vector3 direction)
    {
        root.forward = Vector3.Slerp(root.forward, direction, Time.deltaTime * rotationSpeed);
    }

    private void UpdateMove()
    {
        if (playerInput.Move != Vector3.zero)
            Move(playerInput.Move.normalized);
            //photonView.RPC(nameof(Move), RpcTarget.All, playerInput.Move.normalized);
    }

    [PunRPC]
    private void Move(Vector3 direction)
    {
        pelvis.AddForce(pelvis.position + direction * speed);
    }

    private void UpdateJump()
    {
        if (playerInput.Jump && isGrounded)
            Jump();
            //photonView.RPC(nameof(Jump), RpcTarget.All);
    }

    [PunRPC]
    private void Jump()
    {
        pelvis.AddForce(new Vector3(0, jumpForce, 0));
        isGrounded = false;
    }

    private void UpdateAnim()
    {
        animator.SetFloat("movement", playerInput.Move.magnitude);

        animator.SetBool("isLeftHandUp", playerInput.GrabL);
        animator.SetBool("isRightHandUp", playerInput.GrabR);

        animator.SetBool("isHandsUp", playerInput.HandsUp);
    }

    private void UpdateGrab()
    {
        if (playerInput.GrabL)
            grabL.Activate();
        else
            grabL.Deactivate();

        if (playerInput.GrabR)
            grabR.Activate();
        else
            grabR.Deactivate();
    }

    private void InterpPosition()
    {
        root.position = Vector3.Slerp(root.position, nextPosition, Time.deltaTime * 2);
    }

    private void InterpRotation()
    {
        root.rotation = Quaternion.Slerp(root.rotation, nextRotation, Time.deltaTime * 2);
    }
}