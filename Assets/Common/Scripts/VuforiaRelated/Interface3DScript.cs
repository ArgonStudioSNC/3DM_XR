using UnityEngine;
using Vuforia;

public class Interface3DScript : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES
    [Header("Position")]
    public float verticalDelta = -1.0f;
    public float forwardDist = 0.5f;

    [Header("Scale")]
    public float width = 0.5f; // relative to viewport
    public float aspectRatio = 1.0f;

    [Header("Rotation")]
    public bool rotateChildren = false;

    [Header("Fade")]
    [Range(0, 2)]
    public float fadeDuration = 1.0f;
    public bool fadeChildren = true;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private Camera m_Camera;
    private Vector3 m_camPos;
    private float m_buttonAlpha = 0;
    //private bool m_isFacingPlayer = true;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    protected void Update()
    {
        if (m_Camera != null)
        {
            m_camPos = m_Camera.transform.position;
            // POSITION
            UpdatePosition(transform);
            // SCALE
            Scale(transform);

            // ROTATION
            LookAtCamera(transform);
            if (rotateChildren)
            {
                foreach (Transform t in GetComponentsInChildren<Transform>())
                {
                    LookAtCamera(t);
                }
            }
        }
    }

    protected void LateUpdate()
    {
        // FADE
        m_buttonAlpha = ComputeAlphaValue(m_buttonAlpha);
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) UpdateRendererAlpha(renderer);
        if (fadeChildren)
        {
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                UpdateRendererAlpha(r);
            }
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PRIVATE_METHODS

    private void UpdatePosition(Transform transform)
    {
        // Update position of GameObject to always be about "verticalDelta" meters above the camera
        Vector3 camForwardDir = m_Camera.transform.forward;
        Vector3 camUpDir = m_Camera.transform.up;
        Vector3 lookUpDir = (camUpDir + camForwardDir) / 2;
        Vector3 forwardDir = new Vector3(lookUpDir.x, 0, lookUpDir.z);
        forwardDir.Normalize();

        transform.position = m_camPos + verticalDelta * Vector3.up + forwardDist * forwardDir;
    }

    private void Scale(Transform transform)
    {
        // Apply scale
        Vector3 toCameraVec = m_camPos - transform.position;
        float camDist = toCameraVec.magnitude;
        if (camDist > m_Camera.nearClipPlane)
        {
            // Adjust the scale
            float tanHalfFovX = 1.0f / m_Camera.projectionMatrix[0, 0];
            float sx = width * tanHalfFovX * camDist;
            float sy = sx * aspectRatio;
            transform.localScale = new Vector3(sx, sy, 1);
        }
    }

    private void LookAtCamera(Transform transform)
    {
        // Apply rotation
        Vector3 toCameraVec = m_camPos - transform.position;
        float camDist = toCameraVec.magnitude;
        if (camDist > m_Camera.nearClipPlane)
        {
            // Orient the button surface to face the viewer (like a billboard)
            transform.rotation = Quaternion.LookRotation(m_Camera.transform.forward, m_Camera.transform.up);
        }
    }

    private float ComputeAlphaValue(float alpha)
    {
        Vector2 vp = m_Camera.WorldToViewportPoint(transform.position);
        if (vp.x > 0.2f && vp.x < 0.8f && vp.y > 0.2f && vp.y < 0.8f)
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

    private void UpdateRendererAlpha(Renderer renderer)
    {
        Color color = renderer.material.GetColor("_BaseColor");
        renderer.material.SetColor("_BaseColor", new Color(color.r, color.g, color.b, m_buttonAlpha));
    }

    #endregion // PRIVATE_METHODS


    #region VUFORIA_CALLBACKS

    void OnVuforiaStarted()
    {
        m_Camera = DigitalEyewearARController.Instance.PrimaryCamera ?? Camera.main;
    }

    #endregion // VUFORIA_CALLBACKS
}