using UnityEngine;

/* <summary>
 * Manager class of the MainMenu screen of 3DM_XR application.
 * Load the Vuforia XR SDK after the splash screen to avoid the splash-screen to be displayed in landscape orientation.
 * </summary>
 * */
public class MenuManager : MonoBehaviour
{
    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        StartCoroutine(UtilityHelper.LoadDevice("Vuforia"));
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
