// ClickToMove.cs
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class ClickToMove : MonoBehaviour
{
    public Camera camera;


    RaycastHit hitInfo = new RaycastHit();


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Debug.Log(hitInfo.point);
            }
        }
    }


}