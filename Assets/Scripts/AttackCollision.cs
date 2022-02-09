using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    [SerializeField] private int damage;

    private Entity owner;
    private bool isActive = false;
    private bool didAttack = false;

    public void Setup(Entity owner, bool isActive)
    {
        this.owner = owner;
        this.isActive = isActive;
        didAttack = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive)
            Attack(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isActive)
            Attack(collision.gameObject);
    }

    private void Attack(GameObject other)
    {
        if (didAttack)
            return;

        Entity target = other.GetComponentInParent<Entity>();
        if (target == null || target == owner)
            return;

        target.TakeDamage(damage);
        didAttack = true;
    }
}
