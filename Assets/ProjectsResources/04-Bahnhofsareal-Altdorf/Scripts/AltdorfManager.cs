using UnityEngine;

/* <summary>
 * Manager class attached to the AltdorfManager GameObject.
 * Act like a spate machine to hold the variables of the Altdorf project. Variables are project (id).
 * </summary>
 * */
public class AltdorfManager : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Awake()
    {
        StartCoroutine(UtilityHelper.LoadDevice("Vuforia"));
        TransitionManager.InAR = false;
    }

    protected void Start()
    {
        Debug.Log("VERSION: " + Application.version);
    }

    protected void Update()
    {

    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    #endregion // PUBLIC_METHODS
}
