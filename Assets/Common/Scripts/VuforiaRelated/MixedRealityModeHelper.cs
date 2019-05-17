using UnityEngine;

public class MixedRealityModeHelper : MonoBehaviour
{
    public void HANDHELD_AR()
    {
        TransitionManager.InAR = true;
        TransitionManager.IsFullscreenMode = true;
    }

    public void VIEWER_AR()
    {
        try
        {
            VRCheck();
            TransitionManager.InAR = true;
            TransitionManager.IsFullscreenMode = false;
        }
        catch (DeviceTypeException e)
        {
            throw e;
        }
    }

    public void HANDHELD_VR()
    {
        TransitionManager.InAR = false;
        TransitionManager.IsFullscreenMode = true;
    }

    public void VIEWER_VR()
    {
        try
        {
            VRCheck();
            TransitionManager.InAR = false;
            TransitionManager.IsFullscreenMode = false;
        }
        catch (DeviceTypeException e)
        {
            throw e;
        }
    }

    public void HANDHELD_AR(string sceneName)
    {
        HANDHELD_AR();
        SceneLoader.Instance.LoadSceneByName(sceneName);
    }


    public void VIEWER_AR(string sceneName)
    {
        try
        {
            VIEWER_AR();
            SceneLoader.Instance.LoadSceneByName(sceneName);
        }
        catch (DeviceTypeException e)
        {
            throw e;
        }
    }

    public void HANDHELD_VR(string sceneName)
    {
        HANDHELD_VR();
        SceneLoader.Instance.LoadSceneByName(sceneName);
    }

    public void VIEWER_VR(string sceneName)
    {
        try
        {
            VIEWER_VR();
            SceneLoader.Instance.LoadSceneByName(sceneName);
        }
        catch (DeviceTypeException e)
        {
            throw e;
        }
    }

    private void VRCheck()
    {
        if (RunningDeviceManager.GetDeviceType != RunningDeviceManager.DeviceType.PHONE)
        {
            throw new DeviceTypeException("You can not use viewer mode on this type of device.");
        }
    }
}
