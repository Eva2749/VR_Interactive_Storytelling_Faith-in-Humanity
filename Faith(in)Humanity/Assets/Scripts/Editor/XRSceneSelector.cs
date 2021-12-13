using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Linq;

public static class XRSceneSelector
{
    [MenuItem("XR Scenes/Lobby")]
    static void OpenArea1()
    {
        EditorXRSceneUtils.LoadXRScene("Lobby");
    }

    [MenuItem("XR Scenes/Corridor")]
    static void OpenArea2()
    {
        EditorXRSceneUtils.LoadXRScene("Corridor");
    }
    [MenuItem("XR Scenes/Lab")]
    static void OpenArea3()
    {
        EditorXRSceneUtils.LoadXRScene("Lab");
    }

    [MenuItem("XR Scenes/Outside")]
    static void OpenArea4()
    {
        EditorXRSceneUtils.LoadXRScene("Outside");
    }
    [MenuItem("XR Scenes/Outside Ending")]
    static void OpenArea5()
    {
        EditorXRSceneUtils.LoadXRScene("Outside Ending");
    }

    [MenuItem("XR Scenes/Spaceship Ending")]
    static void OpenArea6()
    {
        EditorXRSceneUtils.LoadXRScene("Spaceship Ending");
    }
    [MenuItem("XR Scenes/Final Choice Scene")]
    static void OpenArea7()
    {
        EditorXRSceneUtils.LoadXRScene("FinLobby");
    }
    [MenuItem("XR Scenes/Credits")]
    static void OpenArea8()
    {
        EditorXRSceneUtils.LoadXRScene("Credits");
    }
}
