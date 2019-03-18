using UnityEngine;
using System;

/* <summary>
 * Attach this to an object with a Renderer component to enable it only when no trackable is found.
 * </summary>
 * */
public class PhantomTargetBehaviour : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    public Transform[] trackables;
    public Light targetLight;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private Renderer mRenderer;
    private TransitionManager mTransitionManager;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mRenderer = GetComponent<Renderer>();
        if (!mRenderer)
        {
            Debug.Log("Can not find any Renderer component attached to the PhantomTarget. Disabling component.");
            GetComponent<PhantomTargetBehaviour>().enabled = false;
            return;
        }
        mTransitionManager = FindObjectOfType<TransitionManager>();
        if (!mTransitionManager)
        {
            throw new NullReferenceException("Can not find any TransitionManager in the scene.");
        }
    }

    protected void Update()
    {
        // only render the phantom target if no Trackable is found
        if (mRenderer) mRenderer.enabled = mTransitionManager.InAR && !currentlyTracking();
        if (targetLight) targetLight.enabled = mTransitionManager.InAR && !currentlyTracking();
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PRIVATE_METHODS

    private bool currentlyTracking()
    {
        foreach (Transform t in trackables)
        {
            if (t.GetComponent<ExtendedDefaultTrackableEventHandler>().targetDetected) return true;
        }
        return false;
    }

    #endregion // PRIVATE_METHODS
}
