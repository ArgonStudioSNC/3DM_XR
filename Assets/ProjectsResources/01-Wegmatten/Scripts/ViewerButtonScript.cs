using UnityEngine;
using UnityEngine.UI;

/* <summary>
 * Script for ViewerButton GameObject.
 * Enable or disable the button giving access to stereoscopic viewer mode. Stereoscopic mode is only available for PHONE devices.
 * </summary>
 * */
public class ViewerButtonScript : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    [Tooltip("Text that should be displayed if the button is disabled.")]
    [TextArea(3, 10)]
    public string disableText;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        // Checks what kind of Device is running the application.
        if (RunningDeviceManager.GetDeviceType != RunningDeviceManager.DeviceType.PHONE)
        {
            GetComponent<Button>().interactable = false;
            GetComponentInChildren<Text>().text = disableText;
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
