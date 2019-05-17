/*==============================================================================
Copyright (c) 2019 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
==============================================================================*/

using UnityEngine;

public class VRContentManager : MonoBehaviour
{
    [SerializeField] GameObject[] VROnlyObjects = null;
    [SerializeField] Animator astronaut = null;
    [SerializeField] Animator drone = null;

    public void EnableVRContent(bool enable)
    {
        foreach (var vrObj in this.VROnlyObjects)
        {
            vrObj.SetActive(enable);
        }

        // Start Astronaut and Drone animations in VR mode
        if (enable)
        {
            if (this.astronaut)
            {
                this.astronaut.SetBool("IsDrilling", enable);
            }

            if (this.drone)
            {
                this.drone.SetBool("IsScanning", enable);
                this.drone.SetBool("IsShowingLaser", enable);
                this.drone.SetBool("IsFacingObject", enable);
            }
        }
    }
}
