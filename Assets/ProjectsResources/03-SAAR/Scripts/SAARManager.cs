using UnityEngine;
using System;
using System.Collections.Generic;

/* <summary>
 * Manager class of the SARR project in 3DM_XR application.
 * </summary>
 * */
public class SAARManager : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    static public string trackableName = null;

    public List<Transform> trackables;
    public Transform thirdDimensionMediaTrackable;
    public Transform phantomTarget;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    SAARTrackableEventHandler trackableEventHandler = null;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
#if UNITY_EDITOR
        if (trackableName == null) trackableName = trackables[0].GetComponent<SAARTrackableEventHandler>().TrackableName;
#endif
        InitTrackable();
        if (!trackableEventHandler)
        {
            throw new MissingMemberException("can not find a trackable with name " + trackableName);
        }

        trackableEventHandler.Register();
        thirdDimensionMediaTrackable.GetComponent<CustomTargetTrackableEventHandler>().UpdateTrackedImageTarget(trackableEventHandler.transform);
        trackables.Add(thirdDimensionMediaTrackable);
        phantomTarget.GetComponent<PhantomTargetBehaviour>().trackables = trackables.ToArray();
        phantomTarget.GetComponent<Renderer>().material = trackableEventHandler.phantomTargetMaterial;
    }

    protected void OnDestroy()
    {
        trackableEventHandler.Unregister();
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PRIVATE_METHODS

    private void InitTrackable()
    {
        SAARTrackableEventHandler tmpTrackableEventHandler;

        foreach (Transform t in trackables)
        {
            tmpTrackableEventHandler = t.GetComponent<SAARTrackableEventHandler>();
            if (!tmpTrackableEventHandler)
            {
                throw new NullReferenceException("gameObject " + t.name + " attached to SAARManager has no SAARTrackableEventHandler");
            }
            tmpTrackableEventHandler.Register();
            tmpTrackableEventHandler.Unregister();

            if (tmpTrackableEventHandler.TrackableName == trackableName)
            {
                trackableEventHandler = tmpTrackableEventHandler;
            }
        }
    }

    #endregion // PRIVATE_METHODS
}
