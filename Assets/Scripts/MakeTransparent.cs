using System.Collections.Generic;
using UnityEngine;

public class MakeTransparent : MonoBehaviour
{
    private Transform target;
    private List<Transparent> current = new List<Transparent>();
    private List<Transparent> already = new List<Transparent>();

    public void Setup(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        if (target == null)
            return;

        MakeObjectSolid();
        GetAllTransparentObject();
        MakeObjectTransparent();
    }

    private void GetAllTransparentObject()
    {
        current.Clear();

        float cameraPlayerDistance = Vector3.Magnitude(Camera.main.transform.position - target.position);
        Ray rayForward = new Ray(Camera.main.transform.position, target.transform.position - Camera.main.transform.position);
        Ray rayBackward = new Ray(Camera.main.transform.position, Camera.main.transform.position - target.transform.position);
        RaycastHit[] hitForwards = Physics.RaycastAll(rayForward, cameraPlayerDistance);
        RaycastHit[] hitBackwards = Physics.RaycastAll(rayBackward, cameraPlayerDistance);

        foreach (RaycastHit hit in hitForwards)
        {
            if (hit.collider.TryGetComponent(out Transparent transparentObj))
            {
                if (!current.Contains(transparentObj))
                    current.Add(transparentObj);
            }
        }

        foreach (RaycastHit hit in hitBackwards)
        {
            if (hit.collider.TryGetComponent(out Transparent transparentObj))
            {
                if (!current.Contains(transparentObj))
                    current.Add(transparentObj);
            }
        }
    }

    private void MakeObjectTransparent()
    {
        for (int i = 0; i < current.Count; i++)
        {
            Transparent obj = current[i];

            if (!already.Contains(obj))
            {
                obj.ShowTransparent();
                already.Add(obj);
            }
        }
    }

    private void MakeObjectSolid()
    {
        for (int i = 0; i < already.Count; i++)
        {
            Transparent obj = already[i];

            if (!current.Contains(obj))
            {
                obj.ShowSolid();
                already.Remove(obj);
            }
        }
    }
}
