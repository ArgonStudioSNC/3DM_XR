using System;
using System.Collections;
using UnityEngine;
using Vuforia;

public class InitGroundplane : MonoBehaviour
{
    #region MONOBEHAVIOUR_METHODS

    protected void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(ScheduleSetupMixedRealityMode);
    }

    protected void OnDestroy()
    {
        VuforiaARController.Instance.UnregisterVuforiaStartedCallback(ScheduleSetupMixedRealityMode);
    }

    #endregion // MONOBEHAVIOUR_METHODS


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

        MixedRealityController.Instance.SetMode(MixedRealityController.Mode.HANDHELD_AR_DEVICETRACKER);
    }

    #endregion // PRIVATE_METHODS
}
