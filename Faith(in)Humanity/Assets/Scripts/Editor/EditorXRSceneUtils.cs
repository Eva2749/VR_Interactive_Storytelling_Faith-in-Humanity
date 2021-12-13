using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public static class EditorXRSceneUtils
{
    public static void LoadXRScene(string scene)
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        Scene xrScene = EditorSceneManager.OpenScene("Assets/Scenes/XR.unity", OpenSceneMode.Single);
        Scene newScene = EditorSceneManager.OpenScene("Assets/Scenes/" + scene + ".unity", OpenSceneMode.Additive);

        XRSceneTransitionManager.ConfigureNewXRScene(xrScene, newScene);
    }
}
