using UnityEngine;

/* <summary>
 * Manager class of the MainMenu screen of 3DM_XR application.
 * Load the Vuforia XR SDK after the splash screen to avoid the splash-screen to be displayed in landscape orientation.
 * </summary>
 * */
public class MenuManager : MonoBehaviour
{
    #region MONOBEHAVIOUR_METHODS

    protected void Awake()
    {
        TransitionManagerGlobals.Instance.Reset();
    }

    protected void Start()
    {
        GameObject.Find("LoadingScreen").SetActive(true);
        StartCoroutine(UtilityHelper.LoadDevice("Vuforia"));
        TransitionManager.Globals.Reset();
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
