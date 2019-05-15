﻿using UnityEngine;

/* <summary>
 * Manager script of the WegmattenMenu scene.
 * </scene>
 * */
public class WegmattenMenuManager : MonoBehaviour
{
    #region PRIVATE_MEMBER_VARIABLES

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        TransitionManager.Globals.InAR = true;
        TransitionManager.Globals.IsFullscreenMode = true;
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
        TransitionManager.Globals.IsFullscreenMode = fullscreen;
        SceneLoader.Instance.LoadSceneByName("Wegmatten");
    }

    #endregion //PUBLIC_METHODS
}
