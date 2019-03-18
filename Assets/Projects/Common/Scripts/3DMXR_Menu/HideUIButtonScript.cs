using UnityEngine;
using UnityEngine.UI;
using System;

/* <summary>
 * Script attached to the HideUIButton Object of the 3DM menu.
 * Keep the button sync with the UIManager3DM.
 * </summary>
 * */
public class HideUIButtonScript : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    // button image references
    public Image imageComponent;
    public Sprite OnSprite;
    public Sprite OffSprite;

    // button text references
    public Text textComponent;
    public string OnText;
    public string OffText;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private UIManager3DM mUIManager3DM;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mUIManager3DM = FindObjectOfType<UIManager3DM>();
        if (!mUIManager3DM)
        {
            throw new NullReferenceException("Can not find any UIManager3D in the scene.");
        }
    }

    protected void Update()
    {
        // sync the logo and text with the isUIEnable variable off UIManager3DM
        if (imageComponent) imageComponent.sprite = mUIManager3DM.isUIEnable ? OffSprite : OnSprite;
        if (textComponent) textComponent.text = mUIManager3DM.isUIEnable ? OffText : OnText;
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
