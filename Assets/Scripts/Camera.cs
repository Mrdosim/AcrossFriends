using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    Vector3 offSetVal;

    private void Start()
    {
        offSetVal = transform.position - target.position;
    }

    private void Update()
    {
        Vector3 cameraPos = target.position + offSetVal;

        transform.position = Vector3.Lerp(transform.position, cameraPos, smoothing * Time.deltaTime);
    }
}
