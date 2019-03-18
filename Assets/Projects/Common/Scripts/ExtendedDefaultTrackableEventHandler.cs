using UnityEngine;

public class ExtendedDefaultTrackableEventHandler : DefaultTrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES

    // is the target currently detected
    [HideInInspector]
    public bool targetDetected = false;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private AudioSource mAudio;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region PROTECTED_METHODS

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        var lightComponents = GetComponentsInChildren<Light>(true);
        // Enable light:
        foreach (var component in lightComponents)
            component.enabled = true;

        mAudio = GetComponent<AudioSource>();
        if (mAudio) mAudio.Play();

        targetDetected = true;
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();

        var lightComponents = GetComponentsInChildren<Light>(true);
        // Disable light:
        foreach (var component in lightComponents)
            component.enabled = false;

        if (mAudio) mAudio.Stop();

        targetDetected = false;
    }

    #endregion // PROTECTED_METHODS
}
