using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class CanvasFader : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES
    [Range(0, 2)]
    public float fadeDuration = 1.0f;
    public Graphic[] graphics;

    public Toggle map;
    public Toggle Param;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private float m_alpha = 0;
    private Camera m_camera;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    protected void LateUpdate()
    {
        // FADE
        float currentAlpha = m_alpha;
        m_alpha = ComputeAlphaValue(m_alpha);

        if (currentAlpha != m_alpha)
        {
            foreach (Graphic g in graphics)
            {
                Color color = g.color;
                g.color = new Color(color.r, color.g, color.b, m_alpha);
            }

            if (m_alpha == 0)
            {
                resetParameters();
            }
        }

    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PRIVATE_METHODS

    private float ComputeAlphaValue(float alpha)
    {
        float cameraAngle = m_camera.transform.rotation.eulerAngles.x;

        if (cameraAngle < 180 && cameraAngle > 30)
        {
            if (alpha < 1)
                alpha += Time.deltaTime / fadeDuration;
        }
        else
        {
            if (alpha > 0)
                alpha -= Time.deltaTime / fadeDuration;
        }

        return Mathf.Clamp01(alpha);
    }

    private void resetParameters()
    {
        map.isOn = false;
        Param.isOn = false;
    }

    #endregion // PRIVATE_METHODS


    #region VUFORIA_CALLBACKS

    void OnVuforiaStarted()
    {
        m_camera = DigitalEyewearARController.Instance.PrimaryCamera ?? Camera.main;
    }

    #endregion // VUFORIA_CALLBACKS
}