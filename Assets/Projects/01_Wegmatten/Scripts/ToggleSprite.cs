using UnityEngine;
using UnityEngine.UI;
using System;

/* <summary>
 * Script for toggle-like component with a sprite.
 * Sync the object with the variables in the scene manager.
 * </summary>
 * */
public class ToggleSprite : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    /* <summary>
     * Different project-relative variables held by the manager
     * </summary>
     * */
    public enum WegmattenVariable
    {
        none,
        TREES,
        GABARITS
    }

    [Tooltip("Choose the variable in WegmattenManager this toggle should sync with.")]
    public WegmattenVariable variableName;
    public Sprite toggleOnSprite;
    public Sprite toggleOffSprite;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private WegmattenManager mWegmattenManager;
    private Image mButtonImage;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mWegmattenManager = FindObjectOfType<WegmattenManager>();
        if (!mWegmattenManager)
        {
            throw new NullReferenceException("Can't find the object WegmattenManager");
        }
        mButtonImage = GetComponent<Image>();
        if (!mButtonImage) Debug.Log("ToggleSprite script can't find any Image component.");
    }

    protected void Update()
    {
        if (mButtonImage)
        {
            switch (variableName)
            {
                case WegmattenVariable.TREES:
                    mButtonImage.sprite = mWegmattenManager.EnableTrees ? toggleOnSprite : toggleOffSprite;
                    break;
                case WegmattenVariable.GABARITS:
                    mButtonImage.sprite = mWegmattenManager.EnableGabarits ? toggleOnSprite : toggleOffSprite;
                    break;
            }
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
