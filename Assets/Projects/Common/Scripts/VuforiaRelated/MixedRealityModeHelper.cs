using UnityEngine;

public class MixedRealityModeHelper : MonoBehaviour
{
    public void HANDHELD_AR()
    {
        TransitionManagerGlobals.Instance.InAR = true;
        TransitionManagerGlobals.Instance.IsFullscreenMode = true;
    }

    public void VIEWER_AR()
    {
        try
        {
            VRCheck();
            TransitionManagerGlobals.Instance.InAR = true;
            TransitionManagerGlobals.Instance.IsFullscreenMode = false;
        }
        catch (DeviceTypeException e)
        {
            throw e;
        }
    }

    public void HANDHELD_VR()
    {
        TransitionManagerGlobals.Instance.InAR = false;
        TransitionManagerGlobals.Instance.IsFullscreenMode = true;
    }

    public void VIEWER_VR()
    {
        try
        {
            VRCheck();
            TransitionManagerGlobals.Instance.InAR = false;
            TransitionManagerGlobals.Instance.IsFullscreenMode = false;
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
