using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private float scaleFactor;
    private Weapon currentWeapon;
    public Weapon Weapon => currentWeapon;

    public void PickupWeapon(Weapon weapon)
    {
        if (weapon == currentWeapon)
            return;

        DropWeapon();

        weapon.Activate();

        currentWeapon = weapon;
        weapon.transform.SetParent(transform);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
        weapon.transform.localScale = Vector3.one * scaleFactor;
    }

    public void DropWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Deactivate();
            currentWeapon.transform.SetParent(null, true);
            Invoke("ClearWeapon", 1);
        }
    }

    private void ClearWeapon()
    {
        currentWeapon = null;
    }

    public bool HaveWeapon()
    {
        return currentWeapon;
    }
}
