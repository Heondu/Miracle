using UnityEngine;

using Photon.Pun;

public class PlayerInput : MonoBehaviourPun
{
    public Vector3 Move { get; private set; }
    public bool Jump { get; private set; }
    public bool GrabL { get; private set; }
    public bool GrabR { get; private set; }
    public bool Sprint { get; private set; }
    public bool DropL { get; private set; }
    public bool DropR { get; private set; }

    private void Update()
    {
        if (photonView.IsMine)
        {
            UpdateAxis();
            UpdateJump();
            UpdateGrab();
            UpdateSprint();
            UpdateDrop();
        }
    }

    private void UpdateAxis()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Move = new Vector3(x, 0, z);
    }

    private void UpdateJump()
    {
        Jump = Input.GetButtonDown("Jump");
    }

    private void UpdateGrab()
    {
        GrabL = Input.GetMouseButton(0);
        GrabR = Input.GetMouseButton(1);
    }

    private void UpdateSprint()
    {
        Sprint = Input.GetKey(KeyCode.LeftShift);
    }

    private void UpdateDrop()
    {
        DropL = Input.GetKey(KeyCode.F) && Input.GetMouseButtonDown(0);
        DropR = Input.GetKey(KeyCode.F) && Input.GetMouseButtonDown(1);
    }
}
