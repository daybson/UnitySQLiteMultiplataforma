using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Character;
    public float distance;
    public float heightY;

    public float speedRotation;
    public float speedHeight;


    void Start()
    {
        
    }

   void Update()
    {
        float currentYAngle = transform.eulerAngles.y;
        float desiredYAngle = Character.transform.eulerAngles.y;

        float currentHeight = transform.position.y;
        float desiredHeight = Character.transform.position.y + heightY;


        currentYAngle = Mathf.LerpAngle(currentYAngle, desiredYAngle, speedRotation * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, desiredHeight, speedHeight * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentYAngle, 0);

        Vector3 relativePosition = Character.position;
        Vector3 orbitalPosition = currentRotation * -Vector3.forward * distance;
        relativePosition += orbitalPosition;

        transform.position = new Vector3(relativePosition.x, currentHeight, relativePosition.z);
        transform.LookAt(Character);
    }
}
