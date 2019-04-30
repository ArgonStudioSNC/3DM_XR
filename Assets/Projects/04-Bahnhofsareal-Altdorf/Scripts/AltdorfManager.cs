using UnityEngine;

/* <summary>
 * Manager class attached to the AltdorfManager GameObject.
 * Act like a spate machine to hold the variables of the Altdorf project. Variables are project (id).
 * </summary>
 * */
public class AltdorfManager : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    [Tooltip("GameObject holding the models for alternative projects in AR")]
    public GameObject alternativesProjectsAR;

    public int CurrentProject { get { return mCurrentProjectID; } set { mCurrentProjectID = value; } }

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private TouchEventManager mTouchEventManager;

    // Wegmatten project main variables
    private int mCurrentProjectID = 0;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        Debug.Log("VERSION: " + Application.version);
        //FindObjectOfType<UIManager3DM>().ResetForVR();
        FindObjectOfType<TransitionManager>().TransitionToAR(false);
    }

    protected void Update()
    {
        /*
        // render the scene according to the current variables values
        // ======================
        foreach (Transform child in alternativesProjectsAR.transform)
        {
            child.gameObject.SetActive(false); // clean all
        }
        alternativesProjectsAR.transform.GetChild(mCurrentProjectID).gameObject.SetActive(true); // activate the correct project in AR
        */
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    /*<summary>
     * Switch to the next project in the list. Actually on 2 projects (Logement / EMS).
     * </summary>
     * */
    public void SwitchProject()
    {
        mCurrentProjectID = (mCurrentProjectID + 1) % 2;
    }

    #endregion // PUBLIC_METHODS
}
