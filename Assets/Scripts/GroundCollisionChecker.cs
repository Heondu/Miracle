using UnityEngine;

public class GroundCollisionChecker : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private void OnTriggerEnter(Collider other)
    {
        playerController.isGrounded = true;
    }
}
