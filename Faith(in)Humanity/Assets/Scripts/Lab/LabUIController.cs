using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LabUIController : MonoBehaviour
{
    //controll UI display or not
    public bool UIappear;
    public bool choice1made;
    public bool choice2made;

    public GameObject labUI;
    public GameObject user;

    private void Update()
    {

        if (user == null)
        {
            user = GameObject.FindWithTag("user");
            //exit the function only if find the user 
            if (user == null) return;
        }

        if (UIappear)
        {
            labUI.transform.position = user.transform.position + user.transform.forward;
            labUI.transform.LookAt(user.transform);
            labUI.SetActive(true);
        }
        else
        {
            labUI.SetActive(false);
        }
    }

}
