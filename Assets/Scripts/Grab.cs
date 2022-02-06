using UnityEngine;

public class Grab : MonoBehaviour
{
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
            fixedJoint = grabbableObj.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = rigidbody;
            fixedJoint.breakForce = 9001;
            isGrabbing = true;
        }
    }

    public void Deactivate()
    {
        if (fixedJoint != null)
            Destroy(fixedJoint);
        isGrabbing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        grabbableObj = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        grabbableObj = null;
    }
}
