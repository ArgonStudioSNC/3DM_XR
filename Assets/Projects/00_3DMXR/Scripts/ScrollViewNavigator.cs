using UnityEngine;
using UnityEngine.UI;

/* <summary>
 * Allow to jump to a object position in a scroll view
 * </summary>
 * */
public class ScrollViewNavigator : MonoBehaviour
{
    #region PRIVATE_MEMBER_VARIABLES

    private ScrollRect mScroll;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mScroll = GetComponent<ScrollRect>();
        if (!mScroll)
        {
            Debug.Log("ScrollViewNavigator should be attached to a ScrollRect object. Disabling component.");
            GetComponent<ScrollViewNavigator>().enabled = false;
            return;
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    /* <summary>
     * Jump to the position of object in parameter.
     * </summary>
     * <param name=obj>the position of object we want to jump to</param>
     * */
    public void CenterToItem(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 viewportLocalPosition = mScroll.viewport.localPosition;
        Vector2 childLocalPosition = target.localPosition;
        Vector2 result = new Vector2(
            0 - (viewportLocalPosition.x + childLocalPosition.x),
            0 - (viewportLocalPosition.y + childLocalPosition.y)
        );
        mScroll.content.localPosition = result;
    }

    #endregion // PUBLIC_METHODS
}
