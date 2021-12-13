
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace LevelUp.GrabInteractions
{
    public class PaperController : MonoBehaviour
    {
        //accessing Grab interactable 
        XRGrabInteractable m_GrabInteractable;

        [Tooltip("The Transform that the object will return to")]
        //retun position 
        [SerializeField] Vector3 returnToPosition;
        //reset delay time, how long before it returns
        [SerializeField] float resetDelayTime;
        //should this go back to place or no
        protected bool shouldReturnHome { get; set; }
        //when we take our hands away from this
        bool isController;
        public AudioSource PaperSound; 

        // Start is called before the first frame update
        //asigning variables 
        //setting the return position as the start position of the object
        void Awake()
        {
            m_GrabInteractable = GetComponent<XRGrabInteractable>();
            returnToPosition = this.transform.position;
            //when we let go yeet it back
            shouldReturnHome = true;
        }

        private void OnEnable()
        {
            m_GrabInteractable.selectExited.AddListener(OnSelectExit); 
            m_GrabInteractable.selectEntered.AddListener(OnSelect);
        }

        private void OnDisable()
        {
            m_GrabInteractable.selectExited.RemoveListener(OnSelectExit);
            m_GrabInteractable.selectEntered.RemoveListener(OnSelect);
        }

        private void OnSelect(SelectEnterEventArgs arg0) => CancelInvoke("ReturnHome");
        private void OnSelectExit(SelectExitEventArgs arg0) => Invoke(nameof(ReturnHome), resetDelayTime);

        protected virtual void ReturnHome()
        {
            
            if (shouldReturnHome)
                transform.position = returnToPosition;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (ControllerCheck(other.gameObject))
                return;

            var socketInteractor = other.gameObject.GetComponent<XRSocketInteractor>();

            if (socketInteractor == null)
                shouldReturnHome = true;

            else if (socketInteractor.CanSelect(m_GrabInteractable)) 
            {
                PaperSound.Play();
                Debug.Log("me here");
                shouldReturnHome = false;
            }
            else
                shouldReturnHome = true; //The socket interactor exists and CANNOT select the Grab object
        }

        private void OnTriggerExit(Collider other)
        {
            if (ControllerCheck(other.gameObject))
                return;

            shouldReturnHome = true;
        }

        bool ControllerCheck(GameObject collidedObject)
        {
            //first check that this is not the collider of a controller
            isController = collidedObject.gameObject.GetComponent<XRBaseController>() != null ? true : false;
            return isController;
        }
    }
}