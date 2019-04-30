using System.Collections;
using UnityEngine;
using UnityEngine.XR;

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
        GameObject.Find("LoadingScreen").SetActive(true);
        StartCoroutine(LoadDevice("Vuforia"));
        TransitionManager.isFullscreenMode = true;
    }

    #endregion // MONOBEHAVIOUR_METHODS

    #region PROTECTED_METHODS

    /* <summary>
     * Load the required XR Device.
     * </summary>
     * <param name=newDevice>name of the XR Device to load</param>
     * */
    IEnumerator LoadDevice(string newDevice)
    {
        if (string.Compare(XRSettings.loadedDeviceName, newDevice, true) != 0)
        {
            Debug.Log("Loading " + newDevice + " XR Settings");
            XRSettings.LoadDeviceByName(newDevice);
            yield return null;
            XRSettings.enabled = true;
            Screen.orientation = ScreenOrientation.Portrait; // some XR Device initialization induce landscape orientation
        }
    }

    #endregion // PROTECTED_METHODS
}
