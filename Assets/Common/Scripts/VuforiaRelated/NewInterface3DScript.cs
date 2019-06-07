using UnityEngine;
using Vuforia;

public class NewInterface3DScript : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES
    [Header("Position")]
    public Transform stickToGameObject;
    public bool isFacingPlayer = true;

    [Header("Camera lookup")]
    public bool lookAtCamera = true;
    public bool childrenLookAtCamera = true;

    [Header("Fade")]
    [Range(0, 2)]
    public float fadeDuration = 1.0f;
    public bool childrenFade = true;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private Camera m_Camera;
    private Vector3 m_camPos;
    private float m_buttonAlpha = 0;
    Vector3 m_fixedUIPos;
    Quaternion m_quat = Quaternion.identity;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    protected void Update()
    {
        if (m_Camera != null)
        {
            m_camPos = m_Camera.transform.position;
            // POSITION
            if (stickToGameObject)
            {
                UpdatePosition(transform);
            }

            // CAMERA LOOKUP
            if (lookAtCamera)
            {
                transform.LookAt(m_Camera.transform);
                if (childrenLookAtCamera)
                {
                    foreach (Transform t in GetComponentsInChildren<Transform>())
                    {
                        t.LookAt(m_Camera.transform);
                    }
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
        if (childrenFade)
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
        if (isFacingPlayer)
        {
            m_quat = Quaternion.AngleAxis(m_Camera.transform.rotation.eulerAngles.y, Vector3.up);
        }

        transform.position = m_camPos + m_quat * m_fixedUIPos;
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
        m_fixedUIPos = transform.position - m_Camera.transform.position;
    }

    #endregion // VUFORIA_CALLBACKS
}