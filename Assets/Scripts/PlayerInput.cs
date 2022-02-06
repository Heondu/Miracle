using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 Move { get; private set; }
    public bool Jump { get; private set; }
    public bool GrabL { get; private set; }
    public bool GrabR { get; private set; }
    public bool HandsUp { get; private set; }

    private void Update()
    {
        UpdateAxis();
        UpdateJump();
        UpdateGrab();
        UpdateHandsUp();
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

    private void UpdateHandsUp()
    {
        HandsUp = Input.GetKey(KeyCode.LeftShift);
    }
}
