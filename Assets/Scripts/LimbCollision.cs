using UnityEngine;

public class LimbCollision : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private void OnCollisionEnter(Collision collision)
    {
        playerController.isGrounded = true;
    }
}
