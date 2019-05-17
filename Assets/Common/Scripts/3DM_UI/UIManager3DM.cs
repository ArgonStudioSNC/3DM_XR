using UnityEngine;
using UnityEngine.UI;
using System;
using Vuforia;

/* <summary>
 * Manager class attached to the UIManager3DM.
 * Act like a state machine to keep the 3DM menu consistent. Notable variables are: isUIEnable, isMorePanelEnable.
 * Extends DoubleTapHandler to provide double tap actions.
 * </summary>
 * */
public class UIManager3DM : DoubleTapHandler
{
    #region PUBLIC_MEMBER_VARIABLES

    [Tooltip("Top menu bar GameObject")]
    public GameObject topOptionsBar;
    [Tooltip("Drop down menu with the projects options")]
    public GameObject moreMenu;
    [Tooltip("Raycast mask to block the screen")]
    public GameObject UIMask;
    [Tooltip("Other project specific UI components")]
    public GameObject[] UIContent;

    public bool isUIEnable { get; set; }
    public bool isMorePanelEnable { get; set; }
    public bool isFlashEnable { get; set; }

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private TouchEventManager m_touchEventManager;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    new protected virtual void Start()
    {
        m_touchEventManager = FindObjectOfType<TouchEventManager>();
        if (!m_touchEventManager)
        {
            throw new NullReferenceException("Can not find any TouchEventManager object in the scene.");
        }

        ResetForAR();

        base.Start(); // Start DoubleTabHandler
    }

    new protected virtual void Update()
    {
        // enable-disable the UI components based on the state variables.
        SetGraphicsActive(topOptionsBar, isUIEnable);
        foreach (GameObject go in UIContent)
        {
            SetGraphicsActive(go, isUIEnable);
        }
        SetGraphicsActive(moreMenu, isMorePanelEnable);
        UIMask.SetActive(isMorePanelEnable);

        base.Update();
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    /* <summary>
     * Enable-disable the MorePanel by switching the isModePanelEnable variable.
     * </summary>
     * */
    public void OnOffMorePanel()
    {
        isMorePanelEnable = !isMorePanelEnable;
    }

    /* <summary>
     * Enable-disable the UI by switching the isUIEnable variable.
     * </summary>
     * */
    public void OnOffUI()
    {
        isUIEnable = !isUIEnable;
    }

    /* <summary>
     * Enable-disable the flash light by switching the isFlashEnable variable.
     * </summary>
     * */
    public void OnOffFlash()
    {
        SetFlash(!isFlashEnable);
    }

    /* <summary>
     * Reset the state variables for AR mode.
     * </summary>
     * */
    public void ResetForAR()
    {
        isUIEnable = true;
        isMorePanelEnable = false;
        SetFlash(false);
    }

    /* <summary>
     * Reset the state variables for VR mode.
     * </summary>
     * */
    public void ResetForVR()
    {
        isUIEnable = false;
        isMorePanelEnable = false;
        SetFlash(false);
    }

    #endregion // PUBLIC_METHODS


    #region PROTECTED_METHODS

    // override the Tap function to redefine what is considered a tap by the DoucheTapHandler
    override protected bool Tap()
    {
        return TransitionManager.IsFullscreenMode && TransitionManager.InAR && Input.GetButtonDown("Fire1") && !m_touchEventManager.UIHit && !m_touchEventManager.raycastHit.collider;
    }

    override protected void OnDoubleTap()
    {
        isUIEnable = !isUIEnable; // on double tap, disable UI
        isMorePanelEnable = false;
    }

    #endregion //PROTECTED_METHODS


    #region PRIVATE_METHODS

    // disable all Graphic components of a GameObject and his children. Scripts keep on working.
    private void SetGraphicsActive(GameObject go, bool active)
    {
        foreach (Graphic graphic in go.GetComponentsInChildren<Graphic>())
        {
            graphic.enabled = active;
        }
    }

    private void SetFlash(bool state)
    {
        isFlashEnable = state;
        CameraDevice.Instance.SetFlashTorchMode(isFlashEnable);
    }

    #endregion // PRIVATE_METHODS
}
