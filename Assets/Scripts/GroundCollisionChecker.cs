using UnityEngine;

public class GroundCollisionChecker : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerController.isGrounded = true;
    }
}
