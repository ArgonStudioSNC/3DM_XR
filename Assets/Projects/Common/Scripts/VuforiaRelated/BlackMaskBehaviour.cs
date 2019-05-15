using UnityEngine;
using Vuforia;
using System;

/* <summary>
 * Attach this script to a BlackMask object. Allow to obstruct the field of view of main camera and transition with fade in / fade out.
 * </summary>
 * */
public class BlackMaskBehaviour : MonoBehaviour
{
    #region PRIVATE_MEMBER_VARIABLES

    private float m_fadeFactor;
    private Camera m_camera;
    private Renderer m_renderer;

    #endregion //PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        m_camera = Camera.main;
        if (!m_camera) throw new NullReferenceException("Please add a main camera to the scene.");

        m_renderer = GetComponent<Renderer>();
        if (!m_renderer)
        {
            Debug.Log("Can not find any renderer attached. Disabling component");
            GetComponent<BlackMaskBehaviour>().enabled = false;
            return;
        }

        SetFadeFactor(0);

        VuforiaARController.Instance.RegisterVuforiaInitializedCallback(OnVuforiaStarted);
    }

    protected void Update()
    {
        float fovX = 2.0f * Mathf.Atan(1.0f / m_camera.projectionMatrix[0, 0]);
        float fovY = 2.0f * Mathf.Atan(1.0f / m_camera.projectionMatrix[1, 1]);

        // Set black mask position at near clip plane
        float near = m_camera.nearClipPlane;
        transform.localPosition = 1.05f * Vector3.forward * near;
        transform.localScale = new Vector3(
            16.0f * near * Mathf.Tan(fovX / 2),
            16.0f * near * Mathf.Tan(fovY / 2),
            1);

        // Update black mask transparency
        // black mask becomes fully opaque (black) at half transition (0.5)
        // then, beyond 0.5, the black mask plane gradually becomes transparent again (until 1.0).
        m_renderer.material.SetFloat("_Alpha", m_fadeFactor);
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    /* <summary>
     * Use this function to assign a new fade factor to the BlackMask. This can be used to do smooth transitions with fade in/out.
     * </summary>
     * */
    public void SetFadeFactor(float ff)
    {
        m_fadeFactor = Mathf.Clamp01(ff);
    }

    #endregion // PUBLIC_METHODS


    #region VUFORIA_CALLBACKS

    void OnVuforiaStarted()
    {
        m_camera = DigitalEyewearARController.Instance.PrimaryCamera ?? Camera.main;
    }

    #endregion // VUFORIA_CALLBACKS
}
