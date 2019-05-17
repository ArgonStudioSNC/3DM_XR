using UnityEngine;
using System;
using Vuforia;

/* <summary>
 * Define the behaviour of the Reticle objects.
 * Reticle is only visible in Viewer mode. The pointer changes when entering a clickable area.
 * </summary>
 * */
public class ReticleBehaviour : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    public float Scale = 0.012f; // relative to viewport width

    [Header("Reticle change on focuse")]
    [Tooltip("Give the texture of the unfocused reticle.")]
    public Texture reticleTexture;
    [Tooltip("Give the texture of the focused reticle.")]
    public Texture reticleFocusedTexture;
    [Tooltip("Give a list of every clickable GameObjects.")]
    public GameObject[] clickableObjects;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private Transform m_reticle;
    private TouchEventManager m_touchEventManager = null;
    private Renderer m_reticleRenderer;
    private BackgroundPlaneBehaviour m_backgroundPlaneBehaviour;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        m_reticle = transform;
        m_touchEventManager = FindObjectOfType<TouchEventManager>();

        if (!m_touchEventManager)
        {
            throw new NullReferenceException("Can not find any TouchEventManager in the scene.");
        }

        m_reticleRenderer = GetComponent<Renderer>();
    }

    protected void OnDestroy()
    {
        VuforiaARController.Instance.UnregisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    protected void Update()
    {
        ScaleReticle();

        bool focused = false;
        if (m_touchEventManager.objectHit)
        {
            foreach (GameObject go in clickableObjects)
            {
                if (m_touchEventManager.objectHit == go) focused = true;
            }
        }
        // switch texture type
        if (m_reticleRenderer) m_reticleRenderer.material.mainTexture = focused ? reticleFocusedTexture : reticleTexture;
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PRIVATE_METHODS

    private void ScaleReticle()
    {
        Camera cam = DigitalEyewearARController.Instance.PrimaryCamera ?? Camera.main;

        if (cam.projectionMatrix.m00 > 0 || cam.projectionMatrix.m00 < 0)
        {
            // We adjust the reticle distance from camera
            if (VideoBackgroundManager.Instance.VideoBackgroundEnabled)
            {
                // When the frustum skewing is applied (e.g. in AR view),
                // we shift the Reticle at the background depth,
                // so that the reticle appears in focus in stereo view
                if (m_backgroundPlaneBehaviour)
                {
                    float bgDepth = m_backgroundPlaneBehaviour.transform.localPosition.z;

                    m_reticle.localPosition = (bgDepth > cam.nearClipPlane) ?
                        Vector3.forward * bgDepth :
                        Vector3.forward * (cam.nearClipPlane + 0.5f);
                }
            }
            else
            {
                // if the frustum is not skewed, then we apply a default depth (which works nicely in VR view)
                m_reticle.localPosition = Vector3.forward * (cam.nearClipPlane + 0.5f);
            }

            // We scale the reticle to be a small % of viewport width
            float localDepth = m_reticle.localPosition.z;
            float tanHalfFovX = 1.0f / cam.projectionMatrix[0, 0];
            float tanHalfFovY = 1.0f / cam.projectionMatrix[1, 1];
            float maxTanFov = Mathf.Max(tanHalfFovX, tanHalfFovY);
            float viewWidth = 2 * maxTanFov * localDepth;
            m_reticle.localScale = new Vector3(Scale * viewWidth, Scale * viewWidth, 1);
        }
    }

    #endregion // PRIVATE_METHODS


    #region VUFORIA_CALLBACK_METHODS

    void OnVuforiaStarted()
    {
        m_backgroundPlaneBehaviour = FindObjectOfType<BackgroundPlaneBehaviour>();
    }

    #endregion // VUFORIA_CALLBACK_METHODS
}
