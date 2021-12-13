using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onmouseDragtest : MonoBehaviour
{

    float rotSpeed = 20;
    // Start is called before the first frame update


    void OnMouseDrag()
    {

        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;
        transform.RotateAround(Vector3.up, -rotX);
        transform.RotateAround(Vector3.right, -rotY);

        //Debug.Log("YOU drag on the bottle");
    }
}
