/*===============================================================================
Copyright (c) 2015-2018 PTC Inc. All Rights Reserved.

Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/* <summary>
 * This manager class is attached to a TransitionManager GameObject and handles the different Mixed Reality mode of the current scene.
 * The class switch between Device-AR, Device-VR, Viewer-AR and Viewer_VR modes. It enable the rendering of the correct game objects and makes smooth transition between the modes.
 * </summary>
 * */
public class TransitionManager : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    static public bool isFullscreenMode = true; // Device or Viewer mode
    public bool InAR { get { return mTransitionCursor <= 0.66f; } }

    [Tooltip("Give all GameObject only visible in AR mode.")]
    public GameObject[] AROnlyObjects;
    [Tooltip("Give all GameObject only visible in VR mode.")]
    public GameObject[] VROnlyObjects;
    [Tooltip("How long should the transition between them modes last.")]
    [Range(0.1f, 5.0f)]
    public float transitionDuration = 1.5f; // seconds
    [Tooltip("Divider GameObject to separate the 2 eyes in Viewer mode.")]
    public Canvas stereoViewDivider;
    [Tooltip("Canvas to disable during the transitions.")]
    public Canvas mainCanvas;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private MixedRealityController.Mode mCurrentMode = MixedRealityController.Mode.HANDHELD_AR;
    private BlackMaskBehaviour mBlackMaskBehaviour;

    private float mTransitionCursor = 0; // we start in AR
    private float mCurrentTime;
    private bool mPlaying;
    private bool mBackward;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(SetupMixedRealityMode);
    }

    protected void OnDestroy()
    {
        VuforiaARController.Instance.UnregisterVuforiaStartedCallback(SetupMixedRealityMode);
    }

    protected void Start()
    {
        mBlackMaskBehaviour = FindObjectOfType<BlackMaskBehaviour>();
        if (!mBlackMaskBehaviour)
        {
            Debug.Log("Can not find any BlackMask object in the scene.");
        }
        SetBlackMaskVisible(false, 0);
        mCurrentTime = Time.realtimeSinceStartup;
    }

    protected void Update()
    {
        float time = Time.realtimeSinceStartup;
        float deltaTime = Mathf.Clamp01(time - mCurrentTime);
        mCurrentTime = time;

        // We need to check if the video background is currently enabled
        // because Vuforia may restart the video background when the App is resumed
        // even if the app was paused in VR mode
        MixedRealityController.Mode mixedRealityMode = GetMixedRealityMode();

        if ((mCurrentMode != mixedRealityMode) ||
            (InAR != VideoBackgroundManager.Instance.VideoBackgroundEnabled))
        {
            // mixed reality mode to switch to
            mCurrentMode = mixedRealityMode;

            // When we transition to VR, we deactivate the Datasets
            // before setting the mixed reality mode.
            // so to reduce CPU usage, as tracking is not needed in this phase
            // (with AutoStopCameraIfNotRequired ON by default, camera/tracker
            //  will be turned off for performance optimization).
            if (mCurrentMode == MixedRealityController.Mode.HANDHELD_VR
                || mCurrentMode == MixedRealityController.Mode.VIEWER_VR)
            {
                Debug.Log("Switching to VR: deactivate datasets");
                ActivateDatasets(false);
            }

            // As we are moving back to AR, we re-activate the Datasets,
            // before setting the mixed reality mode.
            // this will ensure that the Tracker and Camera are restarted,
            // in case they were previously stopped when moving to VR
            // before activating the AR mode
            if (mCurrentMode == MixedRealityController.Mode.HANDHELD_AR
                || mCurrentMode == MixedRealityController.Mode.VIEWER_AR)
            {
                Debug.Log("Switching to AR: activate datasets");
                ActivateDatasets(true);
            }

            MixedRealityController.Instance.SetMode(mCurrentMode);
            UpdateVisibleObjects();
        }

        if (mPlaying)
        {
            float fadeFactor = 0;
            if (mTransitionCursor < 0.33f)
            {
                // fade to full black in first part of transition
                fadeFactor = Mathf.SmoothStep(0, 1, mTransitionCursor / 0.33f);
            }
            else if (mTransitionCursor < 0.66f)
            {
                // between 33% and 66% we stay in full black
                fadeFactor = 1;
            }
            else // > 0.66
            {
                // between 66% and 100% we fade out
                fadeFactor = Mathf.SmoothStep(1, 0, (mTransitionCursor - 0.66f) / 0.33f);
            }
            SetBlackMaskVisible(true, fadeFactor);

            float delta = (mBackward ? -1 : 1) * deltaTime / transitionDuration;
            mTransitionCursor += delta;

            if (mTransitionCursor <= 0 || mTransitionCursor >= 1)
            {
                // Done: stop animated transition
                mTransitionCursor = Mathf.Clamp01(mTransitionCursor);
                mPlaying = false;
                mainCanvas.gameObject.SetActive(true);
                SetBlackMaskVisible(false, 0);
            }
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    /* <summary>
     * Start a transition between AR and VR modes.
     * </summary>
     * <param name=reverse>true for VR to AR transition. false for AR to VR transition<param>
     * */
    public void TransitionToAR(bool reverse)
    {
        if (!mPlaying) // don't restart playing during a transition
        {
            mPlaying = true;
            mBackward = reverse;
            mainCanvas.gameObject.SetActive(false);
            mTransitionCursor = mBackward ? 1 : 0;
        }
    }

    /* <summary>
     * Return the current mixed reality mode.
     * </summary>
     * */
    public MixedRealityController.Mode GetMixedRealityMode()
    {
        if (InAR)
        {
            return isFullscreenMode ?
                MixedRealityController.Mode.HANDHELD_AR : MixedRealityController.Mode.VIEWER_AR;
        }

        return isFullscreenMode ?
            MixedRealityController.Mode.HANDHELD_VR : MixedRealityController.Mode.VIEWER_VR;
    }

    #endregion // PUBLIC_METHODS


    #region PRIVATE_METHODS

    private void SetupMixedRealityMode()
    {
        mCurrentMode = GetMixedRealityMode();
        MixedRealityController.Instance.SetMode(mCurrentMode);

        UpdateVisibleObjects();
    }

    private void ActivateDatasets(bool enableDataset)
    {
        // disable-enable dataset
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        IEnumerable<DataSet> datasets = objectTracker.GetDataSets();

        foreach (DataSet dataset in datasets)
        {
            if (enableDataset)
                objectTracker.ActivateDataSet(dataset);
            else
                objectTracker.DeactivateDataSet(dataset);
        }
    }

    private void UpdateVisibleObjects()
    {
        // update the rendered object when transitioning between AR and VR
        foreach (var go in AROnlyObjects)
        {
            go.SetActive(InAR);
        }

        foreach (var go in VROnlyObjects)
        {
            go.SetActive(!InAR);
        }

        Camera.main.GetComponent<Camera>().clearFlags = InAR ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox;

        if (isFullscreenMode)
        {
            Screen.orientation = ScreenOrientation.Portrait;
            if (stereoViewDivider) stereoViewDivider.enabled = false;
            mainCanvas.enabled = true; // enable UI in Device mode
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            if (stereoViewDivider) stereoViewDivider.enabled = true;
            mainCanvas.enabled = false; // no UI in Viewer mode
        }
    }

    private void SetBlackMaskVisible(bool visible, float fadeFactor)
    {
        // enable the BlackMask and set the fadeFactor for smooth transition
        if (mBlackMaskBehaviour && mBlackMaskBehaviour.enabled)
        {
            mBlackMaskBehaviour.GetComponent<Renderer>().enabled = visible;
            mBlackMaskBehaviour.SetFadeFactor(fadeFactor);
        }
    }

    #endregion // PRIVATE_METHODS
}
