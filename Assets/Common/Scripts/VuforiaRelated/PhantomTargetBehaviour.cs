using UnityEngine;

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

    private Renderer m_renderer;
    private Fader m_fader;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        m_fader = FindObjectOfType<Fader>();
        m_renderer = GetComponent<Renderer>();
        if (!m_renderer)
        {
            Debug.Log("Can not find any Renderer component attached to the PhantomTarget. Disabling component.");
            GetComponent<PhantomTargetBehaviour>().enabled = false;
            return;
        }
    }

    protected void Update()
    {
        bool enable = true;
        if (m_fader != null)
        {
            enable = enable && !m_fader.IsFadingInProgress;
        }
        enable = enable && TransitionManager.InAR && !currentlyTracking();

        // only render the phantom target if no Trackable is found
        if (m_renderer) m_renderer.enabled = enable;
        if (targetLight) targetLight.enabled = enable;
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
