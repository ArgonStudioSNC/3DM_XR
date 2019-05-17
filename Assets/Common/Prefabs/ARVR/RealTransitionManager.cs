/*===============================================================================
Copyright (c) 2015-2019 PTC Inc. All Rights Reserved.

Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/

using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Vuforia;

public class RealTransitionManager : MonoBehaviour, IFaderNotify
{
    #region PUBLIC_MEMBER_VARIABLES
    static public bool isFullScreenMode = true;
    #endregion PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES
    MixedRealityController.Mode currentMode = MixedRealityController.Mode.HANDHELD_AR;
    VRContentManager vrContentManager;
    Fader fader;
    bool InAR = true;
    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(ScheduleSetupMixedRealityMode);
    }

    void OnDestroy()
    {
        VuforiaARController.Instance.UnregisterVuforiaStartedCallback(ScheduleSetupMixedRealityMode);
    }

    void Start()
    {
        this.fader = FindObjectOfType<Fader>();
        this.vrContentManager = FindObjectOfType<VRContentManager>();
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    public void SwitchMode(bool toAR)
    {
        VLog.Log("cyan", "SwitchMode() called: " + (toAR ? "toAR" : "toVR"));

        this.InAR = toAR;
        this.fader.FadeOut();
    }

    #endregion // PUBLIC_METHODS


    #region IFADERNOTIFY_CALLBACK_METHODS

    void IFaderNotify.OnFadeOutFinished()
    {
        VLog.Log("cyan", "IFaderNotify.OnFadeOutFinished() called: " + gameObject.name);

        // We need to check if the video background is curently enabled
        // because Vuforia may restart the video background when the App is resumed
        // even if the app was paused in VR mode

        MixedRealityController.Mode mixedRealityMode = GetMixedRealityMode();

        if ((this.currentMode != mixedRealityMode) ||
            (this.InAR != VideoBackgroundManager.Instance.VideoBackgroundEnabled))
        {
            // mixed reality mode to switch to
            this.currentMode = mixedRealityMode;

            // When we transition to VR, we deactivate the Datasets
            // before setting the mixed reality mode.
            // so to reduce CPU usage, as tracking is not needed in this phase
            // (with AutoStopCameraIfNotRequired ON by default, camera/tracker
            //  will be turned off for performance optimization).

            if (this.currentMode == MixedRealityController.Mode.HANDHELD_VR ||
                this.currentMode == MixedRealityController.Mode.VIEWER_VR)
            {
                Debug.Log("Switching to VR: deactivating datasets");
                ActivateDataSets(false);
            }

            // As we are moving back to AR, we re-activate the Datasets,
            // before setting the mixed reality mode.
            // this will ensure that the Tracker and Camera are restarted,
            // in case they were previously stopped when moving to VR
            // before activating the AR mode
            if (this.currentMode == MixedRealityController.Mode.HANDHELD_AR ||
                this.currentMode == MixedRealityController.Mode.VIEWER_AR)
            {
                Debug.Log("Switching to AR: activating datasets");
                ActivateDataSets(true);
            }

            MixedRealityController.Instance.SetMode(this.currentMode);

            this.vrContentManager.EnableVRContent(!this.InAR);

            this.fader.FadeIn();
        }
    }

    void IFaderNotify.OnFadeInFinished()
    {
        VLog.Log("cyan", "IFaderNotify.OnFadeInFinished() called: " + gameObject.name);
    }

    #endregion // IFADERNOTIFY_CALLBACK_METHODS


    #region PRIVATE_METHODS

    void ScheduleSetupMixedRealityMode()
    {
        StartCoroutine(WaitForFrame(SetupMixedRealityMode));
    }

    IEnumerator WaitForFrame(Action setupAction)
    {
        yield return new WaitForEndOfFrame();
        setupAction();
    }

    // on Vuforia Started
    void SetupMixedRealityMode()
    {
        this.currentMode = GetMixedRealityMode();
        MixedRealityController.Instance.SetMode(this.currentMode);

        this.vrContentManager.EnableVRContent(!this.InAR);
    }

    MixedRealityController.Mode GetMixedRealityMode()
    {
        if (this.InAR)
        {
            return isFullScreenMode ?
                MixedRealityController.Mode.HANDHELD_AR :
                MixedRealityController.Mode.VIEWER_AR;
        }

        return isFullScreenMode ?
            MixedRealityController.Mode.HANDHELD_VR :
            MixedRealityController.Mode.VIEWER_VR;
    }

    void ActivateDataSets(bool enableDataset)
    {
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        // We must stop the ObjectTracker before activating/deactivating datasets
        if (objectTracker.IsActive)
            objectTracker.Stop();

        if (!objectTracker.IsActive)
        {
            IEnumerable<DataSet> datasets = objectTracker.GetDataSets();
            foreach (DataSet dataset in datasets)
            {
                // Activate or Deactivate each DataSet
                if (enableDataset)
                    objectTracker.ActivateDataSet(dataset);
                else
                    objectTracker.DeactivateDataSet(dataset);
            }
        }

        if (!objectTracker.IsActive)
            objectTracker.Start();
    }

    #endregion // PRIVATE_METHODS
}
