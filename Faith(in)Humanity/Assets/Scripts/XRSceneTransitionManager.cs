using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

[DisallowMultipleComponent]
public class XRSceneTransitionManager : MonoBehaviour
{
    public static XRSceneTransitionManager Instance;

    public Material transitionMaterial;
    public float transitionSpeed = 1.0f;
    public string initialScene;
    public bool isLoading { get; private set; } = false;

    Scene xrScene;
    Scene currentScene;
    float currentFade = 0.0f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Detected that singleton SceneTransitionManager has already been created. Deleting this instance.");
            //there can be only one!
            Destroy(this.gameObject);
            return;
        }

        xrScene = SceneManager.GetActiveScene();
        SceneManager.sceneLoaded += OnNewSceneAdded;

        if(!Application.isEditor)
        {
            TransitionTo(initialScene);
        }
    }

    void OnNewSceneAdded(Scene newScene, LoadSceneMode mode)
    {
        if (newScene != xrScene)
        {
            SceneManager.SetActiveScene(newScene);
            currentScene = newScene;
            ConfigureNewXRScene(xrScene, currentScene);
        }
    }

    public void TransitionTo(string scene)
    {
        if(!isLoading)
        {
            StartCoroutine(Load(scene));
        }
    }

    IEnumerator Load(string scene)
    {
        isLoading = true;
        yield return StartCoroutine(Fade(1.0f));
        yield return StartCoroutine(UnloadCurrent());

        yield return StartCoroutine(LoadNewScene(scene));
        yield return StartCoroutine(Fade(0.0f));
        isLoading = false;
    }

    IEnumerator UnloadCurrent()
    {
        AsyncOperation unload = SceneManager.UnloadSceneAsync(currentScene);
        while(!unload.isDone) 
            yield return null;
    }

    IEnumerator LoadNewScene(string name)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        while (!load.isDone)
            yield return null;
    }

    public static void ConfigureNewXRScene(Scene xrScene, Scene newScene)
    {
        GameObject[] xrObjects = xrScene.GetRootGameObjects();
        GameObject[] newSceneObjects = newScene.GetRootGameObjects();

        GameObject xrRig = xrObjects.First((obj) => { return obj.CompareTag("XRRig"); });
        GameObject sceneControllerObj = newSceneObjects.First((obj) => { return obj.CompareTag("XRSceneController"); });

        XRSceneController sceneController = sceneControllerObj.GetComponent<XRSceneController>();

        if (sceneController)
        {
            Debug.Log("ARE we here");
            sceneController.Init();
            Debug.Log("dhvb");

            Transform xrRigOrigin = sceneController.GetXRRigOrigin();
            if (xrRig && xrRigOrigin)
            {
                xrRig.transform.position = xrRigOrigin.transform.position;
                xrRig.transform.rotation = xrRigOrigin.transform.rotation;
            }
        }

        
    }

    IEnumerator Fade(float dst)
    {
        while(!Mathf.Approximately(currentFade, dst))
        {
            currentFade = Mathf.MoveTowards(currentFade, dst, transitionSpeed * Time.deltaTime);
            transitionMaterial.SetFloat("_FadeAmount", currentFade);
            yield return null;
        }
        transitionMaterial.SetFloat("_FadeAmount", dst);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

}
