using UnityEngine;
using UnityEngine.EventSystems;
using System;

/* <summary>
 * Attach this script to register and handle the touch events on the screen.
 * </summary>
 * */
public class TouchEventManager : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    public bool IsHit { get { return Input.GetButtonDown("Fire1"); } }
    public bool UIHit { get; set; }
    public RaycastHit raycastHit { get { return mRaycastHit; } }
    public GameObject objectHit { get { return !UIHit && mRaycastHit.collider ? mRaycastHit.collider.gameObject : null; } set { } }

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private Transform mCamera;
    private RaycastHit mRaycastHit;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mCamera = Camera.main.transform;
        if (!mCamera)
        {
            throw new NullReferenceException("Please attach a Camera to the scene.");
        }
    }

    protected void Update()
    {
        if (TransitionManager.Globals.IsFullscreenMode) // in Device mode, use pointer position
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // touchscreen version
                UIHit = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId); // is it an UI interaction
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                Physics.Raycast(ray, out mRaycastHit);
            }
            else
            {
                // mouse version
                UIHit = EventSystem.current.IsPointerOverGameObject();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out mRaycastHit);
            }
        }
        else // in Viewer mode, use Camera gaze
        {
            UIHit = false; // no UI in viewer mode
            Ray cameraGaze = new Ray(mCamera.position, mCamera.forward);
            Physics.Raycast(cameraGaze, out mRaycastHit, Mathf.Infinity);
        }

        if (!UIHit)
        {
            objectHit = mRaycastHit.collider ? mRaycastHit.collider.gameObject : null;
        }

        if (IsHit) Debug.Log("Click action \nUIHit=" + UIHit + " | objectHit=" + (objectHit ? objectHit.name : "null"));
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
