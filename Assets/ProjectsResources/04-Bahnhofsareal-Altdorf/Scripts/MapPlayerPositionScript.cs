using UnityEngine;

public class MapPlayerPositionScript : MonoBehaviour
{
    public Vector2 projectDimensions;
    public Transform player;

    private Rect m_mapDimensions;
    private Transform m_playerHead;
    private float m_xRatio;
    private float m_yRatio;

    private Vector2 m_playerPosition { get { return new Vector2(player.position.x, player.position.z); } }

    protected void Awake()
    {
        m_mapDimensions = transform.parent.GetComponent<RectTransform>().rect;
        m_playerHead = player.GetChild(0);

        m_xRatio = -1.0f * (2.0f / projectDimensions.x) * (m_mapDimensions.width / 2.0f);
        m_yRatio = (2.0f / projectDimensions.y) * (m_mapDimensions.height / 2.0f);

        Debug.Log(m_playerPosition.x + " " + m_yRatio);
    }

    protected void OnEnable()
    {
        Update();
    }

    protected void Update()
    {
        transform.localPosition = new Vector3(m_playerPosition.y * m_yRatio, m_playerPosition.x * m_xRatio, transform.localPosition.z);
        transform.localRotation = Quaternion.Euler(0, 0, m_playerHead.localRotation.eulerAngles.y * -1.0f - 90.0f);
    }

    public void JumpToLocation(Transform markerTransform)
    {
        Debug.Log(markerTransform.localPosition);
        player.localPosition = new Vector3(markerTransform.localPosition.y / m_xRatio, 15.0f, markerTransform.localPosition.x / m_yRatio);
    }
}
