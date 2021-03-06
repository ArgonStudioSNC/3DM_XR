﻿using UnityEngine;
using UnityEngine.SceneManagement;

/* <summary>
 * Manager script of the AltdorfMenu scene.
 * </scene>
 * */
public class AltdorfMenuManager : MonoBehaviour
{
    #region PRIVATE_MEMBER_VARIABLES

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        TransitionManager.IsFullscreenMode = true;
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    public void StartFullScreeenMode(bool fullscreen)
    {
        // double check the device type
        if (!fullscreen && RunningDeviceManager.GetDeviceType != RunningDeviceManager.DeviceType.PHONE)
        {
            Debug.Log("You can not use viewer mode on this type of device.");
            return;
        }

        // Set up the mobile/viewer mode and load the next scene.
        TransitionManager.IsFullscreenMode = fullscreen;
        //SceneController.Instance.LoadSceneByName("Bahnhofsareal-Altdorf-VR");
        SceneManager.LoadSceneAsync("Bahnhofsareal-Altdorf-VR");
    }

    #endregion //PUBLIC_METHODS
}
