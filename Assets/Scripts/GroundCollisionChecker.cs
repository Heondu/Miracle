using UnityEngine;

public class GroundCollisionChecker : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        playerController.isGrounded = true;
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
