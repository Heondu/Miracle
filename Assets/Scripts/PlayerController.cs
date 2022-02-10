using UnityEngine;
using UnityEngine.Events;

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

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(grabL);
            stream.SendNext(grabR);
        }
        else
        {
            this.grabL=(Grab)stream.ReceiveNext();
            this.grabR=(Grab)stream.ReceiveNext();
        }
    }

    #endregion

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if(photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        UpdateRotate();
        UpdateJump();
        UpdateGrab();
        UpdateAnim();
    }

    private void FixedUpdate()
    {
        UpdateMove();
    }

    private void UpdateRotate()
    {
        if (playerInput.Move != Vector3.zero)
            Rotate();
    }

    private void Rotate()
    {
        root.forward = Vector3.Slerp(root.forward, playerInput.Move, Time.deltaTime * rotationSpeed);
    }

    private void UpdateMove()
    {
        if (playerInput.Move != Vector3.zero)
            Move();
    }

    private void Move()
    {
        pelvis.AddForce(pelvis.position + playerInput.Move.normalized * speed);
    }

    private void UpdateJump()
    {
        if (playerInput.Jump && isGrounded)
            Jump();
    }

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
}