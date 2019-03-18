using Vuforia;
using UnityEngine;

public class SAARTrackableEventHandler : ExtendedDefaultTrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES

    public Material phantomTargetMaterial;

    public string TrackableName
    {
        get
        {
            if (mTrackableBehaviour) return mTrackableBehaviour.TrackableName;
            else return GetComponent<TrackableBehaviour>().TrackableName;
        }
    }

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PROTECTED_METHODS

    protected override void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
    }

    protected override void OnDestroy()
    {
    }

    #endregion // PROTECTED_METHODS


    #region PUBLIC_METHODS

    public void Register()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        OnTrackingLost();
    }

    public void Unregister()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // PUBLIC_METHODS
}
