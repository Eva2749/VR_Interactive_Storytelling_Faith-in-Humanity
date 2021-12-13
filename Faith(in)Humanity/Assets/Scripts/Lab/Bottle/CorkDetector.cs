using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorkDetector : MonoBehaviour
{
    public bool corkOut = false;


    public void CorkIn()
    {
        corkOut = false;
    }

    public void CorkOut()
    {
        corkOut = true;
    }
}
