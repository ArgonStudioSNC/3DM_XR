using UnityEngine;
using UnityEngine.UI;
using System;

/* <summary>
 * Script for OptionText GameObject.
 * Sync the text with the current project being displayed.
 * </summary>
 * */
public class OptionTextScript04 : MonoBehaviour
{
    #region PRIVATE_MEMBER_VARIABLES

    private AltdorfManager mAltdorfManager;
    private string[] mProjectsTexts = new string[] {
      "Quartiergestaltungsplan",
      "Alternativvariante"
    };

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mAltdorfManager = FindObjectOfType<AltdorfManager>();
        if (!mAltdorfManager)
        {
            throw new NullReferenceException("Can't find object of type AltdorfManager");
        }
    }

    protected void Update()
    {
        GetComponent<Text>().text = mProjectsTexts[mAltdorfManager.CurrentProject]; // update the text
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
