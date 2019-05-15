using UnityEngine;
using UnityEngine.UI;
using System;

/* <summary>
 * Script attached to the MoreButton object of the 3DM menu.
 * Keep the button sync with the UIManager3DM.
 * </summary>
 * */
public class MoreButtonScript : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    // button image references
    public Sprite On;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private UIManager3DM mUIManager3DM;
    private Image mImage;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mUIManager3DM = FindObjectOfType<UIManager3DM>();
        if (!mUIManager3DM)
        {
            throw new NullReferenceException("Can not find any UIManager3D in the scene.");
        }

        mImage = gameObject.GetComponent<Image>();
        if (!mImage)
        {
            Debug.Log("Can not find any Image component attached to the MoreButton. Disabling component.");
            GetComponent<MoreButtonScript>().enabled = false;
            return;
        }
    }

    protected void Update()
    {
        if (mImage)
        {
            mImage.transform.rotation = mUIManager3DM.isMorePanelEnable ? Quaternion.Euler(new Vector3(0, 0, 90)) : Quaternion.Euler(new Vector3(0, 0, 0));
        }

    }

    #endregion // MONOBEHAVIOUR_METHODS
}
