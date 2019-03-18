using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DraggablePanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    #region PUBLIC_MEMBER_VARIABLES

    public Transform UIMask;
    public Transform mainContent;
    public Image arrowImage;
    public Sprite upArrow;
    public Sprite downArrow;
    public float accelerationRate;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private float mCurrentPosition;
    private float mMaxPosition;
    private float mTmpPosition;
    private Image mUIMaskImage;
    private int mPointerDownPosY;
    private SAARMenuManager mSAARMenuManager;
    private CanvasScaler mScaler;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mMaxPosition = Mathf.Abs(mainContent.GetComponent<RectTransform>().rect.y);
        mCurrentPosition = mMaxPosition;
        mUIMaskImage = UIMask.GetComponent<Image>();
        mSAARMenuManager = FindObjectOfType<SAARMenuManager>();
        mScaler = GetComponentInParent<CanvasScaler>();
    }

    protected void Update()
    {
        arrowImage.sprite = mCurrentPosition > 0.0f ? downArrow : upArrow;
        UIMask.gameObject.SetActive(mCurrentPosition < mMaxPosition);
        Color color = mUIMaskImage.color;
        color.a = (1 - (mCurrentPosition / mMaxPosition)) / 2.0f;
        mUIMaskImage.color = color;
        transform.localPosition = new Vector2(0.0f, mCurrentPosition);
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    public void OnBeginDrag(PointerEventData eventData)
    {
        mTmpPosition = mCurrentPosition;
        mSAARMenuManager.AnimationsEnable(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        mTmpPosition += ((mScaler.referenceResolution.y / Screen.height) * eventData.delta.y);
        mCurrentPosition = Mathf.Clamp(mTmpPosition, 0.0f, mMaxPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (mCurrentPosition > mMaxPosition / 2.0f) StartCoroutine(CloseAnimation()); else StartCoroutine(OpenAnimation());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            if (mCurrentPosition == 0) mCurrentPosition = mMaxPosition;
            else if (mCurrentPosition == mMaxPosition) mCurrentPosition = 0;
            mSAARMenuManager.AnimationsEnable(mCurrentPosition == mMaxPosition);
        }
    }

    public void ClosePanel()
    {
        if (mCurrentPosition == 0) mCurrentPosition = mMaxPosition;
        mSAARMenuManager.AnimationsEnable(mCurrentPosition == mMaxPosition);
    }

    #endregion // PUBLIC_METHODS


    #region PRIVATE_METHODS

    private IEnumerator OpenAnimation()
    {
        mTmpPosition = mCurrentPosition;
        float speed = -250;
        for (mTmpPosition = mCurrentPosition; mTmpPosition > 0; mTmpPosition += speed * Time.deltaTime)
        {
            speed -= Time.deltaTime * accelerationRate * 5000;
            mCurrentPosition = Mathf.Clamp(mTmpPosition, 0.0f, mMaxPosition);
            yield return null;
        }
        mCurrentPosition = 0;
    }

    private IEnumerator CloseAnimation()
    {
        mTmpPosition = mCurrentPosition;
        float speed = 250;
        for (mTmpPosition = mCurrentPosition; mTmpPosition < mMaxPosition; mTmpPosition += speed * Time.deltaTime)
        {
            speed += Time.deltaTime * accelerationRate * 5000;
            mCurrentPosition = Mathf.Clamp(mTmpPosition, 0.0f, mMaxPosition);
            yield return null;
        }
        mCurrentPosition = mMaxPosition;
        mSAARMenuManager.AnimationsEnable(true);
    }

    #endregion // PRIVATE_METHODS
}