/*==============================================================================
Copyright (c) 2019 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
==============================================================================*/

using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public static class UtilityHelper
{

    public static void RotateTowardCamera(GameObject augmentation)
    {
        if (Vuforia.VuforiaManager.Instance.ARCameraTransform != null)
        {
            var lookAtPosition = Vuforia.VuforiaManager.Instance.ARCameraTransform.position - augmentation.transform.position;
            lookAtPosition.y = 0;
            var rotation = Quaternion.LookRotation(lookAtPosition);
            augmentation.transform.rotation = rotation;
        }
    }

    public static void EnableRendererColliderCanvas(GameObject gameObject, bool enable)
    {
        var rendererComponents = gameObject.GetComponentsInChildren<Renderer>(true);
        var colliderComponents = gameObject.GetComponentsInChildren<Collider>(true);
        var canvasComponents = gameObject.GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = enable;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = enable;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = enable;
    }

    public static int GetNumberOfActiveAnchors()
    {
        int numOfAnchors = 0;

        Vuforia.StateManager stateManager = Vuforia.TrackerManager.Instance.GetStateManager();

        if (stateManager != null)
        {
            foreach (Vuforia.TrackableBehaviour behaviour in stateManager.GetActiveTrackableBehaviours())
            {
                if (behaviour is Vuforia.AnchorBehaviour)
                {
                    numOfAnchors += 1;
                    Debug.Log("Anchor #" + numOfAnchors + ": " + behaviour.TrackableName);
                }
            }
        }
        return numOfAnchors;
    }

    /* <summary>
    * Load the required XR Device.
    * </summary>
    * <param name=newDevice>name of the XR Device to load</param>
    * */
    public static IEnumerator LoadDevice(string newDevice)
    {
        if (string.Compare(XRSettings.loadedDeviceName, newDevice, true) != 0)
        {
            Debug.Log("Loading " + newDevice + " XR Settings");
            XRSettings.LoadDeviceByName(newDevice);
            yield return null;
            XRSettings.enabled = true;
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }
}