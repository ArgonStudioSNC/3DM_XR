using UnityEngine;
using Vuforia;

public static class DeviceTrackerHelper
{
    public static RotationalDeviceTracker SetupRotationalDeviceTracker()
    {
        Debug.Log("Transitioning to RotationalDeviceTracker mode");
        ITrackerManager trackerManager = TrackerManager.Instance;

        // we check if the requested DeviceTracker isn't already setup
        RotationalDeviceTracker tracker = trackerManager.GetTracker<RotationalDeviceTracker>();
        if (tracker == null)
        {
            // if an other DeviceTracker is already running, turn it off
            DeviceTracker currentTracker = trackerManager.GetTracker<PositionalDeviceTracker>();
            if (currentTracker != null) currentTracker.Stop();

            // to change DeviceTracker, CameraDevice has to be Deinit
            CameraDevice.Instance.Stop();
            CameraDevice.Instance.Deinit();

            if (currentTracker != null) trackerManager.DeinitTracker<PositionalDeviceTracker>();

            // init the DeviceTracker and restart the CameraDevice
            tracker = trackerManager.InitTracker<RotationalDeviceTracker>();;

            CameraDevice.Instance.Init();
        }

        return tracker;
    }

    public static PositionalDeviceTracker SetupPositionalDeviceTracker()
    {
        Debug.Log("Transitioning to PositionalDeviceTracker mode");
        ITrackerManager trackerManager = TrackerManager.Instance;

        // we check if the requested DeviceTracker isn't already setup
        PositionalDeviceTracker tracker = trackerManager.GetTracker<PositionalDeviceTracker>();
        if (tracker == null)
        {
            // if an other DeviceTracker is already running, turn it off
            DeviceTracker currentTracker = trackerManager.GetTracker<RotationalDeviceTracker>();
            if (currentTracker != null) currentTracker.Stop();

            // to change DeviceTracker, CameraDevice has to be Deinit
            CameraDevice.Instance.Stop();
            CameraDevice.Instance.Deinit();

            if (currentTracker != null) trackerManager.DeinitTracker<RotationalDeviceTracker>();

            // init the DeviceTracker and restart the CameraDevice
            tracker = trackerManager.InitTracker<PositionalDeviceTracker>();

            if (trackerManager.GetTracker<PositionalDeviceTracker>() == null)
            {
                throw new DeviceTrackerException("Can't initialize PositionalDeviceTracker : check your phone compatibility");
            }

            CameraDevice.Instance.Init();
        }
        return tracker;
    }
}
