using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.XR;
using System.Collections.Generic;

/* <summary>
 * Attach this script to register and handle the touch events on the screen.
 * </summary>
 * */
public class TouchEventManager : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    public bool IsHit { get { return Input.GetButtonDown("Fire1"); } }
    public bool UIHit { get; set; }
    public RaycastHit raycastHit { get { return m_raycastHit; } }
    public GameObject objectHit
    {
        get
        {
            if (UIHit)
            {
                if (TransitionManager.IsFullscreenMode) return EventSystem.current.currentSelectedGameObject;

                return m_raycastResults[0].gameObject;
            }
            return m_raycastHit.collider ? m_raycastHit.collider.gameObject : null;
        }
    }

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private Transform m_camera;
    private RaycastHit m_raycastHit;
    private List<RaycastResult> m_raycastResults = new List<RaycastResult>();

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        m_camera = Camera.main.transform;
        if (!m_camera)
        {
            throw new NullReferenceException("Please attach a Camera to the scene.");
        }
    }

    protected void Update()
    {
        if (TransitionManager.IsFullscreenMode) // in Device mode, use pointer position
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // touchscreen version
                UIHit = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId); // is it an UI interaction
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                Physics.Raycast(ray, out m_raycastHit);
            }
            else
            {
                // mouse version
                UIHit = EventSystem.current.IsPointerOverGameObject();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out m_raycastHit);
            }
        }
        else // in Viewer mode, use Camera gaze
        {
            Vector2 lookPosition;
            lookPosition.x = XRSettings.eyeTextureWidth / 2;
            lookPosition.y = XRSettings.eyeTextureHeight / 2;
            PointerEventData ped = new PointerEventData(EventSystem.current);
            ped.position = lookPosition;
            EventSystem.current.RaycastAll(ped, m_raycastResults);

            UIHit = m_raycastResults.Count != 0;
            
            Ray cameraGaze = new Ray(m_camera.position, m_camera.forward);
            Physics.Raycast(cameraGaze, out m_raycastHit, Mathf.Infinity);
        }

        if (IsHit) Debug.Log("Click action \nUIHit=" + UIHit + " | objectHit=" + (objectHit ? objectHit.name : "null"));
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
