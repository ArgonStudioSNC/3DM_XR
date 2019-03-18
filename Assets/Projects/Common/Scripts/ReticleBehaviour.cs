using UnityEngine;
using System;

/* <summary>
 * Define the behaviour of the Reticle objects.
 * Reticle is only visible in Viewer mode. The pointer changes when entering a clickable area.
 * </summary>
 * */
public class ReticleBehaviour : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    [Tooltip("Give the texture of the unfocused reticle.")]
    public Texture reticle;
    [Tooltip("Give the texture of the focused reticle.")]
    public Texture reticleFocused;
    [Tooltip("Give a list of every clickable GameObjects.")]
    public GameObject[] clickableObjects;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private TouchEventManager mTouchEventManager = null;
    private Renderer mReticleRenderer;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    void Start()
    {
        mTouchEventManager = FindObjectOfType<TouchEventManager>();
        if (!mTouchEventManager)
        {
            throw new NullReferenceException("Can not find any TouchEventManager in the scene.");
        }
        mReticleRenderer = GetComponent<Renderer>();
        if (!mReticleRenderer)
        {
            Debug.Log("Can not find any Renderer component attached to the Reticle. Disabling component.");
            GetComponent<ReticleBehaviour>().enabled = false;
            return;
        }
    }

    void Update()
    {
        mReticleRenderer.enabled = !TransitionManager.isFullscreenMode; // only enable in viewer mode

        bool focused = false;
        if (mTouchEventManager.objectHit)
        {
            foreach (GameObject go in clickableObjects)
            {
                if (mTouchEventManager.objectHit == go) focused = true;
            }
        }
        // switch texture type
        mReticleRenderer.material.mainTexture = focused ? reticleFocused : reticle;
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
