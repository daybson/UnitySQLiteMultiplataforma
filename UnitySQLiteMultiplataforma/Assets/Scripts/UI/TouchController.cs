using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public Transform ImageCenter;
    public Transform ImageBorder;

    private Vector2 imageCenter2d;

    public Vector2 Direction { get; protected set; }

    void Start()
    {
        imageCenter2d = new Vector2(ImageCenter.position.x, ImageCenter.position.y);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Direction = (touch.position - imageCenter2d).normalized;
                ImageBorder.position = imageCenter2d + Direction * 50;
            }
            else
            {
                Direction = Vector2.zero;
                ImageBorder.position = imageCenter2d;
            }
        }
        else
        {
            Direction = Vector2.zero;
            ImageBorder.position = imageCenter2d;
        }
    }
}
