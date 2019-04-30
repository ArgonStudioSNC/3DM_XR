using UnityEngine;

public class MapScaler : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    public float shiftX = -220f;
    public float shiftY = -15f;
    public float safeZoneX = 1780f;
    public float safeZoneY = 2784f;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private RectTransform mParentRectTransform;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region UNITY_MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mParentRectTransform = transform.parent.gameObject.GetComponent<RectTransform>();
        scaleMap();
    }

    protected void Update()
    {
#if UNITY_EDITOR
        scaleMap();
#endif
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PRIVATE_METHODS

    private void scaleMap()
    {
        float scaling = Mathf.Min(mParentRectTransform.rect.width / safeZoneX, mParentRectTransform.rect.height / safeZoneY);

        gameObject.transform.localScale = new Vector2(scaling, scaling);
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(shiftX * scaling, shiftY * scaling);
    }

    #endregion // PRIVATE_METHODS
}
