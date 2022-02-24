using UnityEngine;
using Photon.Pun;

public class PlayerWeapon : MonoBehaviourPun
{
    public float scaleFactor;
    private Weapon currentWeapon;
    public Weapon Weapon => currentWeapon;
    private Grab grab;
    public Transform socket;

    private void Awake()
    {
        grab = GetComponent<Grab>();
        GetComponentInParent<Entity>().onDeath.AddListener(OnPlayerDeath);
    }

    public void PickupWeapon(Weapon weapon)
    {
        if (weapon == currentWeapon)
            return;

        if (weapon != currentWeapon)
            ChangeWeapon();

        grab.haveWeapon = true;
        currentWeapon = weapon;
        weapon.GetComponent<PhotonView>().RPC(nameof(weapon.Pick), RpcTarget.All, photonView.ViewID);
    }

    public void DropWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.GetComponent<PhotonView>().RPC(nameof(currentWeapon.Drop), RpcTarget.All);
            grab.haveWeapon = false;
            Invoke("ClearWeapon", 1);
        }
    }

    public void ChangeWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.GetComponent<PhotonView>().RPC(nameof(currentWeapon.Drop), RpcTarget.All);
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

    private void OnPlayerDeath(Entity player)
    {
        DropWeapon();
    }
}
