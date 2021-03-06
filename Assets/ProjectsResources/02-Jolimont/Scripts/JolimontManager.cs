using UnityEngine;
using UnityEngine.UI;
using System;

/* <summary>
 * Manager class of the Jolimont project in 3DM_XR application.
 * Allow to change the sun position and the illumination by switching between different direct lights in the scene.
 * </summary>
 * */
public class JolimontManager : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    [Tooltip("Text component where the sun position is displayed.")]
    public Text sunPositionText;
    [Tooltip("GameObject containing the varying direct lights.")]
    public GameObject lights;

    #endregion PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private Transform mLightTransform;
    private int mLightSize; // number of different direct lights
    private int mCurrentLightId = 0;
    private string[] mSunTexts = new string[] {
      "20 Mars - 09h30",
      "23 septembre - 14h30",
      "21 juin - 12h00",
      "21 décembre - 12h00"
    };

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS
    
    protected void Start()
    {
        if (!lights)
        {
            throw new NullReferenceException("No lights gameObject attach to JolimontManager");
        }
        mLightTransform = lights.transform;
        mLightSize = mLightTransform.childCount;
        EnableLight(mCurrentLightId);
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    /* <summary>
     * Increment or decrement the ID of the current direct light
     * </summary>
     * <param name=reverse>true increment. false decrement.</param>
     * */
    public void IncrementSunPosition(bool reverse)
    {
        mCurrentLightId = (reverse ? mCurrentLightId + 1 : mCurrentLightId - 1 + mLightSize) % mLightSize;
        EnableLight(mCurrentLightId);
    }

    #endregion // PUBLIC_METHODS


    #region PRIVATE_METHODS

    // enable the direct light with the given lightID
    private void EnableLight(int lightID)
    {
        // disable all lights
        foreach (Transform child in mLightTransform)
        {
            child.gameObject.SetActive(false);
        }

        mLightTransform.GetChild(lightID).gameObject.SetActive(true);

        UpdateText();
    }

    // update the text with the correct date and time
    private void UpdateText()
    {
        if (sunPositionText) sunPositionText.text = mSunTexts[mCurrentLightId];
    }

    #endregion // PRIVATE_METHODS
}
