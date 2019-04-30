using UnityEngine;
using UnityEngine.UI;
using System;

/* <summary>
 * Script for OptionText GameObject.
 * Sync the text with the current project being displayed.
 * </summary>
 * */
public class OptionTextScript : MonoBehaviour
{
    #region PRIVATE_MEMBER_VARIABLES

    private WegmattenManager mWegmanttenManager;
    private string[] mProjectsTexts = new string[] {
      "Wegmatten Plus",
      "Wohnen Wegmatten"
    };

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mWegmanttenManager = FindObjectOfType<WegmattenManager>();
        if (!mWegmanttenManager)
        {
            throw new NullReferenceException("Can't find object of type WegmattenManager");
        }
    }

    protected void Update()
    {
        GetComponent<Text>().text = mProjectsTexts[mWegmanttenManager.CurrentProject]; // update the text
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
