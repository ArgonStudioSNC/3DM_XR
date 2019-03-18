using UnityEngine;
using System;

/* <summary>
 * Script attached to the MoreMenu object of the 3DM menu.
 * Keep the content of MoreMenu sync with the state of the TransitionManager (AR or VR).
 * </summary>
 * */
public class MoreMenuScript : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    // references to the AR-VR menu GameObjects
    public GameObject ARMenuObjects;
    public GameObject VRMenuObjects;

    #endregion PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private TransitionManager mTransitionManager;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mTransitionManager = FindObjectOfType<TransitionManager>();
        if (!mTransitionManager)
        {
            throw new NullReferenceException("Can not find any TransitionManager object in the scene.");
        }
    }

    protected void Update()
    {
        if (!ARMenuObjects || !VRMenuObjects)
        {
            Debug.Log("Please attach all the required game objects. Disabling component");
            GetComponent<MoreMenuScript>().enabled = false;
            return;
        }
        ARMenuObjects.SetActive(mTransitionManager.InAR);
        VRMenuObjects.SetActive(!mTransitionManager.InAR);
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
