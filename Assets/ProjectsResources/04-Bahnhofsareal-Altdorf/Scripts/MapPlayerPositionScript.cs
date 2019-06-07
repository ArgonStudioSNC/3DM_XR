using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class MapPlayerPositionScript : MonoBehaviour
{
    public Vector2 projectDimensions;
    public Transform player;

    private Rect m_mapDimensions;
    private Camera m_camera;
    private float m_xRatio;
    private float m_yRatio;

    private Vector2 m_playerPosition { get { return new Vector2(player.position.x, player.position.z); } }

    protected void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    protected void Update()
    {
        if (m_mapDimensions != transform.parent.GetComponent<RectTransform>().rect)
        {
            SetupScale();
        }

        transform.localPosition = new Vector3(m_playerPosition.y * m_yRatio, m_playerPosition.x * m_xRatio, transform.localPosition.z);
        transform.localRotation = Quaternion.Euler(0, 0, m_camera.transform.rotation.eulerAngles.y * -1.0f - 90.0f);
    }

    public void JumpToLocation(Transform markerTransform)
    {
        Vector3 posRelativeToMap = transform.parent.InverseTransformPoint(markerTransform.position);
        player.localPosition = new Vector3(posRelativeToMap.y / m_xRatio, 13.0f, posRelativeToMap.x / m_yRatio);

        DropdownMenu dd = FindObjectOfType<DropdownMenu>();
        if (dd) dd.transform.GetComponent<Toggle>().isOn = false;
    }

    private void SetupScale()
    {
        m_mapDimensions = transform.parent.GetComponent<RectTransform>().rect;

        m_xRatio = -1.0f * (2.0f / projectDimensions.x) * (m_mapDimensions.width / 2.0f);
        m_yRatio = (2.0f / projectDimensions.y) * (m_mapDimensions.height / 2.0f);
    }

    #region VUFORIA_CALLBACKS

    void OnVuforiaStarted()
    {
        m_camera = DigitalEyewearARController.Instance.PrimaryCamera ?? Camera.main;
    }

    #endregion // VUFORIA_CALLBACKS
}
