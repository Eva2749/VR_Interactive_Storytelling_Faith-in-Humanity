using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullLever : MonoBehaviour
{
    public Transform start;
    public Transform end;

    public void PullLeverAmount(float t)
    {
        transform.rotation = Quaternion.Slerp(start.rotation, end.rotation, t);
    }
}
