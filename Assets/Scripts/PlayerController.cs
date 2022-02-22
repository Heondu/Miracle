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

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
        }
        else
        {
        }
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
            if (!UIManager.IsUIControl)
            {
                UpdateRotate();
                UpdateJump();
                UpdateGrab();
            }
            UpdateAnim();
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine && !UIManager.IsUIControl)
        {
            UpdateMove();
        }
    }

    private void UpdateRotate()
    {
        if (playerInput.Move != Vector3.zero)
            Rotate(playerInput.Move);
    }

    private void Rotate(Vector3 direction)
    {
        root.forward = Vector3.Slerp(root.forward, direction, Time.deltaTime * rotationSpeed);
    }

    private void UpdateMove()
    {
        if (playerInput.Move != Vector3.zero)
            Move(playerInput.Move.normalized);
    }

    private void Move(Vector3 direction)
    {
        pelvis.AddForce(pelvis.position + direction * speed);
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
        if (!UIManager.IsUIControl)
        {
            animator.SetFloat("movement", playerInput.Move.magnitude);

            animator.SetBool("isLeftHandUp", playerInput.GrabL);
            animator.SetBool("isRightHandUp", playerInput.GrabR);

            animator.SetBool("isHandsUp", playerInput.HandsUp);
        }
        else
        {
            animator.SetFloat("movement", 0);
        }
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