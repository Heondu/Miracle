using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private PlayerWeapon playerWeaponL;
    [SerializeField] private PlayerWeapon playerWeaponR;
    [SerializeField] private AttackCollision attackCollisionL;
    [SerializeField] private AttackCollision attackCollisionR;

    private Entity entity;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    private void Update()
    {
        if (UIManager.IsUIControl)
            return;

        if (Input.GetKey(KeyCode.F))
        {
            if (Input.GetMouseButtonDown(0))
                playerWeaponL.DropWeapon();
            else if (Input.GetMouseButtonDown(1))
                playerWeaponR.DropWeapon();
        }
    }

    public void ActivateLeftAttackCollision()
    {
        SetAttackCollision(true, true);
    }

    public void DeactivateLeftAttackCollision()
    {
        SetAttackCollision(true, false);
    }

    public void ActivateRightAttackCollision()
    {
        SetAttackCollision(false, true);
    }

    public void DeactivateRightAttackCollision()
    {
        SetAttackCollision(false, false);
    }

    private void SetAttackCollision(bool isLeft, bool isEnabled)
    {
        if (isLeft)
        {
            if (playerWeaponL.HaveWeapon())
                playerWeaponL.Weapon.SetAttackCollisionActive(entity, isEnabled);
            else
                attackCollisionL.Setup(entity, isEnabled);
        }
        else
        {
            if (playerWeaponR.HaveWeapon())
                playerWeaponR.Weapon.SetAttackCollisionActive(entity, isEnabled);
            else
                attackCollisionR.Setup(entity, isEnabled);
        }
    }

    public PlayerWeapon GetPlayerWeapon(int id)
    {
        if (id == 0)
            return playerWeaponL;
        else if (id == 1)
            return playerWeaponR;

        return null;
    }
}
