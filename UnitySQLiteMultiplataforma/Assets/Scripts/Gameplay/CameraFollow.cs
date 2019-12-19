using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 distance;
    public float speed;

    private void Update()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            target.position + distance,
            speed * Time.deltaTime);
        
        transform.LookAt(target);
    }
}
