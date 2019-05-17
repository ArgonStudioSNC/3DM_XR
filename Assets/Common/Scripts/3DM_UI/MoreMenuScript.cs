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



    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Update()
    {
        if (!ARMenuObjects || !VRMenuObjects)
        {
            Debug.Log("Please attach all the required game objects. Disabling component");
            GetComponent<MoreMenuScript>().enabled = false;
            return;
        }
        ARMenuObjects.SetActive(TransitionManager.InAR);
        VRMenuObjects.SetActive(!TransitionManager.InAR);
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
