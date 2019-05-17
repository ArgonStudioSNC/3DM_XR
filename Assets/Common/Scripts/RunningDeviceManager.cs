using UnityEngine;

/* <summary>
 * Helper function which guess on what kind of device the application is running.
 * Uses the screen resolution and screen dpi to get the screen diagonal size in inches. All device under 7 inches are PHONEs.
 * </summary>
 * */
public static class RunningDeviceManager
{
    #region PUBLIC_MEMBER_VARIABLES

    // return the type of device
    public static DeviceType GetDeviceType { get { return mDeviceType != DeviceType.UNKNOWN ? mDeviceType : compute(); } }

    // possible type of device
    public enum DeviceType
    {
        UNKNOWN,
        PHONE,
        TABLET
    }

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private static DeviceType mDeviceType = DeviceType.UNKNOWN;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region PRIVATE_METHODS

    private static DeviceType compute()
    {
        var mWidth = Screen.width;
        var mHeight = Screen.height;
        var hypotenuse = Mathf.Sqrt(mWidth * mWidth + mHeight * mHeight);
        var mDiagonalInches = hypotenuse / Screen.dpi;
        mDeviceType = mDiagonalInches > 7 ? DeviceType.TABLET : DeviceType.PHONE;

#if UNITY_EDITOR // in Unity editor, the type of device is set to PHONE
        mDeviceType = DeviceType.PHONE;
        Debug.Log("Editor mode: automatic PHONE DeviceType");
#endif

        return mDeviceType;
    }

    #endregion // PRIVATE_METHODS
}
