using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq; 

//In unity we can only add one of these to a game object
[DisallowMultipleComponent]

//This is responisible for managing transitions to and from other scenes
//Added to the XR scene


public class devXRSceneTransitionManager : MonoBehaviour
{
    public Material transitionMaterial;
    public float transitionSpeed = 1.0f;

    public string initialScene;

    public static devXRSceneTransitionManager Instance;
    public GameObject LoadingCanvasVisual; 

    //autoproperty 
	//public variable that only this class can change, but everyone can access and read 
    public bool isLoading { get; private set; } = false; 

    Scene xrScene;
    Scene currentScene;
    float currentTransitionAmount = 0.0f;

    private void Awake()
    {
        if(Instance ==null)
        {
            Instance = this; 

        }
        else
        {
            //one at a time and so if another one exists just yeet it out
            Debug.LogWarning("Detected rouge XRSceneTransitionManager... Deleting it...");
            Destroy(this.gameObject);
            return;
        }

        xrScene = SceneManager.GetActiveScene();
        SceneManager.sceneLoaded += OnNewSceneAdded;

        if (!Application.isEditor)
        {
            TransitionTo(initialScene);

        }


    }
    public void TransitionTo(string scene)
    {
        if (!isLoading)
        {
            StartCoroutine(Load(scene)); 
        }

    }

    //get the currently loaded scene
    void OnNewSceneAdded(Scene newScene, LoadSceneMode mode)
    {
        if (newScene != xrScene)
        {
            currentScene = newScene;
            SceneManager.SetActiveScene(currentScene);
            PlaceXRRig(xrScene, currentScene);
        }

    }




    //So we don't halt our entire application while we are transitioning from scene to scene
    //We will create coroutines
    IEnumerator Load(string scene)
    {
        isLoading = true;
        //nesting coroutines
        // yield return new WaitForSeconds(4);
        
        yield return StartCoroutine(Fade(1.0f));
        yield return StartCoroutine(UnloadCurrentScene());
        yield return new WaitForSeconds(4); 

        yield return StartCoroutine(LoadNewScene(scene));
        LoadingCanvasVisual.SetActive(false); 
        yield return StartCoroutine(Fade(0.0f));
        

        isLoading = false;
        



    }
    IEnumerator UnloadCurrentScene()
    {
        AsyncOperation unload = SceneManager.UnloadSceneAsync(currentScene);
        //Checking once a frame 
        while (!unload.isDone)
        {
            yield return null;

        }

    }
    IEnumerator LoadNewScene(string scene)
    {

        AsyncOperation load = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        //Checking once a frame 
        while (!load.isDone)
        {
            yield return null;
            LoadingCanvasVisual.SetActive(true); 

        }
    }


    //This function helps us sync the XRRig across scenes, so when we load them, the XRRing also loads with them
    static public void PlaceXRRig(Scene xrScene, Scene newScene)
    {
        //search this for xrrig 
        GameObject[] xrObjects = xrScene.GetRootGameObjects();
        //search this for xrorigin
        GameObject[] newSceneObjects = newScene.GetRootGameObjects();

        //Trying to find the first instance of a game where the comparison tag hits true
        //Then we return that matched object
        GameObject xrRig = xrObjects.First((obj) => { return obj.CompareTag("XRRig"); });
        GameObject xrRigOrigin = newSceneObjects.First((obj) => { return obj.CompareTag("XRRigOrigin"); });

        //if statement making sure we find these 2 things

        if (xrRig && xrRigOrigin)
        {
            //adjusting the rig's postition and rotation to that of the origin's
            xrRig.transform.position = xrRigOrigin.transform.position;
            xrRig.transform.rotation = xrRigOrigin.transform.rotation;
        }



    }

    IEnumerator Fade(float target)
    {
        while(!Mathf.Approximately(currentTransitionAmount, target))
        {
            currentTransitionAmount = Mathf.MoveTowards(currentTransitionAmount, target, transitionSpeed *Time.deltaTime);
            transitionMaterial.SetFloat("_FadeAmount", currentTransitionAmount);


            yield return null; 
        }
        transitionMaterial.SetFloat("_FadeAmount", target);
    }
}
