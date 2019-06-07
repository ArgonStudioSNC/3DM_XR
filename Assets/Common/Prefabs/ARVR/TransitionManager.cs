using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/* <summary>
 * This manager class is attached to a ARVR GameObject and handles the different Mixed Reality mode of the current scene.
 * The class switch between Device-AR, Device-VR, Viewer-AR and Viewer_VR modes.
 * </summary>
 * */
public class TransitionManager : MonoBehaviour, IFaderNotify
{
    #region PUBLIC_MEMBER_VARIABLES

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private MixedRealityController.Mode m_currentMode = MixedRealityController.Mode.HANDHELD_AR;
    private MixedRealityContentManager m_mixedRealityContentManager;
    private Fader m_fader;
    private bool m_positionalTrackingCompiliant = false;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(ScheduleSetupMixedRealityMode);
    }

    protected void OnDestroy()
    {
        VuforiaARController.Instance.UnregisterVuforiaStartedCallback(ScheduleSetupMixedRealityMode);
    }

    protected void Start()
    {
        m_fader = GetComponent<Fader>();
        m_mixedRealityContentManager = GetComponent<MixedRealityContentManager>();
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    public void SwitchMode(bool toAR)
    {
        VLog.Log("cyan", "SwitchMode() called: " + (toAR ? "toAR" : "toVR"));

        InAR = toAR;
        m_mixedRealityContentManager.ShowUI(false);
        m_fader.FadeOut();
    }

    public static bool InAR { get { return Globals.Instance.InAR; } set { Globals.Instance.InAR = value; } }

    public static bool IsFullscreenMode { get { return Globals.Instance.IsFullscreenMode; } set { Globals.Instance.IsFullscreenMode = value; } }

    #endregion // PUBLIC_METHODS


    #region IFADERNOTIFY_CALLBACK_METHODS

    void IFaderNotify.OnFadeOutFinished()
    {
        VLog.Log("cyan", "IFaderNotify.OnFadeOutFinished() called: " + gameObject.name);

        // We need to check if the video background is curently enabled
        // because Vuforia may restart the video background when the App is resumed
        // even if the app was paused in VR mode

        MixedRealityController.Mode mixedRealityMode = GetMixedRealityMode();

        if ((m_currentMode != mixedRealityMode) ||
            (InAR != VideoBackgroundManager.Instance.VideoBackgroundEnabled))
        {
            // mixed reality mode to switch to
            m_currentMode = mixedRealityMode;

            // When we transition to VR, we deactivate the Datasets
            // before setting the mixed reality mode.
            // so to reduce CPU usage, as tracking is not needed in this phase
            // (with AutoStopCameraIfNotRequired ON by default, camera/tracker
            //  will be turned off for performance optimization).

            if (m_currentMode == MixedRealityController.Mode.HANDHELD_VR ||
                m_currentMode == MixedRealityController.Mode.VIEWER_VR)
            {
                Debug.Log("Switching to VR: deactivating datasets");
                ActivateDataSets(false);
            }

            // As we are moving back to AR, we re-activate the Datasets,
            // before setting the mixed reality mode.
            // this will ensure that the Tracker and Camera are restarted,
            // in case they were previously stopped when moving to VR
            // before activating the AR mode
            if (m_currentMode == MixedRealityController.Mode.HANDHELD_AR ||
                m_currentMode == MixedRealityController.Mode.VIEWER_AR ||
                m_currentMode == MixedRealityController.Mode.VIEWER_AR_DEVICETRACKER)
            {
                Debug.Log("Switching to AR: activating datasets");
                ActivateDataSets(true);
            }

            // Setup the correct DeviceTracker
            UpdateDeviceTracker();

            Camera.main.GetComponent<Camera>().clearFlags = InAR ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox;
            MixedRealityController.Instance.SetMode(m_currentMode);
            m_mixedRealityContentManager.UpdateContent(m_currentMode);

            m_fader.FadeIn();
        }
    }

    void IFaderNotify.OnFadeInFinished()
    {
        VLog.Log("cyan", "IFaderNotify.OnFadeInFinished() called: " + gameObject.name);
        m_mixedRealityContentManager.ShowUI(true);
    }

    #endregion // IFADERNOTIFY_CALLBACK_METHODS


    #region PRIVATE_METHODS

    private void ScheduleSetupMixedRealityMode()
    {
        StartCoroutine(WaitForFrame(SetupMixedRealityMode));
    }

    IEnumerator WaitForFrame(Action setupAction)
    {
        yield return new WaitForEndOfFrame();
        setupAction();
    }

    // on Vuforia Started
    private void SetupMixedRealityMode()
    {
        UpdateDeviceTracker();
        Camera.main.GetComponent<Camera>().clearFlags = InAR ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox;
        m_currentMode = GetMixedRealityMode();

        MixedRealityController.Instance.SetMode(m_currentMode);
        m_mixedRealityContentManager.UpdateContent(m_currentMode);
    }

    /* <summary>
    * Return the current mixed reality mode.
    * </summary>
    * */
    private MixedRealityController.Mode GetMixedRealityMode()
    {
        if (InAR)
        {
            if (m_positionalTrackingCompiliant)
            {
                return IsFullscreenMode ?
                    MixedRealityController.Mode.HANDHELD_AR_DEVICETRACKER : MixedRealityController.Mode.VIEWER_AR_DEVICETRACKER;
            }

            return IsFullscreenMode ?
                MixedRealityController.Mode.HANDHELD_AR : MixedRealityController.Mode.VIEWER_AR;
        }

        return IsFullscreenMode ?
            MixedRealityController.Mode.HANDHELD_VR : MixedRealityController.Mode.VIEWER_VR;
    }

    private void ActivateDataSets(bool enableDataset)
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

    private void UpdateDeviceTracker()
    {
        if (m_positionalTrackingCompiliant)
        {
            if (InAR)
            {
                try
                {
                    PositionalDeviceTracker t = DeviceTrackerHelper.SetupPositionalDeviceTracker();
                    t.Start();

                }
                catch (DeviceTrackerException e)
                {
                    m_positionalTrackingCompiliant = false;
                    Debug.Log(e.ToString());
                }
            }
            else
            {
                DeviceTrackerHelper.SetupRotationalDeviceTracker().Start();
            }
        }
    }

    #endregion // PRIVATE_METHODS


    #region PRIVATE_CLASSES

    private sealed class Globals
    {
        private Globals()
        {
            IsFullscreenMode = true;
            InAR = true;
        }

        public static Globals Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {

            }
            internal static readonly Globals instance = new Globals();
        }

        public bool IsFullscreenMode { get; set; } // Device or Viewer mode
        public bool InAR { get; set; }
    }

    #endregion // PRIVATE_CLASSES
}

