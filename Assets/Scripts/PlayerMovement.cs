using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Vector3 startPoint, endPoint;
    [SerializeField] Vector3 direction;
    [SerializeField] bool isTouch;
    [SerializeField] Map map;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isTouch = true;
            startPoint = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isTouch = false;
        }
        if (isTouch)
        {
            endPoint = Input.mousePosition;
            Vector3 dir = (endPoint - startPoint).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            angle = (angle + 360) % 360;
            if ((angle >= 315 && angle < 360) || (angle >= 0 && angle < 45))
            {
                direction = Vector3.right;
            }
            else if (angle >= 45 && angle < 135)
            {
                direction = Vector3.forward;
            }
            else if (angle >= 135 && angle < 225)
            {
                direction = Vector3.left;
            }
            else if (angle >= 225 && angle < 315)
            {
                direction = Vector3.back;
            }
            transform.Translate(direction * speed * Time.deltaTime);
        }

    }


}

