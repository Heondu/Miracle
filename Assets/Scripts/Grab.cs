using UnityEngine;
using Photon.Pun;

public class Grab : MonoBehaviourPun
{
    [SerializeField] private string[] ignoreTag;
    [SerializeField] private PlayerWeapon playerWeapon;

    public bool isGrabbing = false;
    public bool haveWeapon = false;
    public new Rigidbody rigidbody;
    private GrabbableObject grabbableObj;
    public GrabbableObject currentGrabObj;
    public FixedJoint fixedJoint;
    private GameObject weaponObj;
    private PlayerController playerController;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        GetComponentInParent<Entity>().onDeath.AddListener(OnPlayerDeath);
        playerController = GetComponentInParent<PlayerController>();
    }

    public void Activate()
    {
        if (isGrabbing || haveWeapon)
            return;

        if (weaponObj != null)
            PickupWeapon();

        if (grabbableObj != null)
        {
            grabbableObj.GetComponent<PhotonView>().RPC(nameof(grabbableObj.Activate), RpcTarget.All, photonView.ViewID);
            if (grabbableObj.CompareTag("Ground"))
                playerController.isGrounded = true;
        }
    }

    private void PickupWeapon()
    {
        Weapon weapon = weaponObj.GetComponentInParent<Weapon>();
        if (weapon != null && weapon.CanPickup())
        {
            playerWeapon.PickupWeapon(weapon);
        }
    }

    public void Deactivate()
    {
        if (isGrabbing)
        {
            currentGrabObj.GetComponent<PhotonView>().RPC(nameof(currentGrabObj.Deactivate), RpcTarget.All, photonView.ViewID);
        }
    }

    private void OnPlayerDeath(Entity player)
    {
        Deactivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root == transform.root)
            return;

        if (other.CompareTag("Weapon"))
            weaponObj = other.gameObject;

        other.gameObject.TryGetComponent(out grabbableObj);
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject == grabbableObj)
            grabbableObj = null;
        //if (other.gameObject == weaponObj)
            weaponObj = null;
    }
}
