using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Collider[] colliders;
    [SerializeField] private AttackCollision attackCollision;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Activate()
    {
        SetAllCollisionEnabled(false);
        rigidbody.isKinematic = true;
    }

    public void Deactivate()
    {
        SetAllCollisionEnabled(true);
        rigidbody.isKinematic = false;
    }

    private void SetAllCollisionEnabled(bool value)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = value;
        }
    }

    public void SetAttackCollisionActive(Entity owner, bool value)
    {
        attackCollision.Setup(owner, value);
    }
}
