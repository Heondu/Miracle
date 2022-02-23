using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPositionClamper : MonoBehaviour
{
    [SerializeField] private float limitY;
    [SerializeField] private Vector3 offset;
    private Vector3 originPos;

    private void Start()
    {
        originPos = transform.position;
    }

    private void Update()
    {
        if (transform.position.y <= limitY)
        {
            transform.position = originPos + offset;
        }
    }
}
