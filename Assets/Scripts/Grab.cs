using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private string[] ignoreTag;
    [SerializeField] private PlayerWeapon playerWeapon;

    private bool isGrabbing = false;
    private new Rigidbody rigidbody;
    private GameObject grabbableObj;
    private FixedJoint fixedJoint;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Activate()
    {
        if (isGrabbing == false && grabbableObj != null)
        {
            if (CheckWeapon())
                return;

            GrabObject();
        }
    }

    private bool CheckWeapon()
    {
        Weapon weapon = grabbableObj.GetComponentInParent<Weapon>();
        if (weapon != null)
        {
            playerWeapon.PickupWeapon(weapon);
            return true;
        }
        return false;
    }

    private void GrabObject()
    {
        fixedJoint = grabbableObj.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = rigidbody;
        fixedJoint.breakForce = 9001;
        isGrabbing = true;
    }

    public void Deactivate()
    {
        if (fixedJoint != null)
            Destroy(fixedJoint);
        isGrabbing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < ignoreTag.Length; i++)
        {
            if (other.CompareTag(ignoreTag[i]))
                return;
        }
        if (other.transform.root == transform.root)
            return;

        grabbableObj = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        grabbableObj = null;
    }
}
