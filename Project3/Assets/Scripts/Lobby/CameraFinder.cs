using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFinder : MonoBehaviour
{
    public Canvas myCanvas; 
    public GameObject mainCamera; 

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        // myCanvas = 
        myCanvas.worldCamera = mainCamera.GetComponent<Camera>(); 
    }


}
