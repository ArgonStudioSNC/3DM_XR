using UnityEngine;
using System;

/* <summary>
 * Script for toggle-like component with a texture.
 * Sync the object with the variables in the scene manager.
 * </summary>
 * */
public class ToggleTexture : MonoBehaviour
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
    public Texture toggleOnTexture;
    public Texture toggleOffTexture;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private WegmattenManager mWegmattenManager;
    private Material mButtonMaterial;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mWegmattenManager = FindObjectOfType<WegmattenManager>();
        if (!mWegmattenManager)
        {
            throw new NullReferenceException("Can't find the object WegmattenManager");
        }
        mButtonMaterial = GetComponent<Renderer>().material;
        if (!mButtonMaterial) Debug.Log("ToggleTexture script can't find any Renderer component.");
    }

    // Use this for initialization
    protected void Update()
    {
        switch (variableName)
        {
            case WegmattenVariable.TREES:
                mButtonMaterial.mainTexture = mWegmattenManager.EnableTrees ? toggleOnTexture : toggleOffTexture;
                break;
            case WegmattenVariable.GABARITS:
                mButtonMaterial.mainTexture = mWegmattenManager.EnableGabarits ? toggleOnTexture : toggleOffTexture;
                break;
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
