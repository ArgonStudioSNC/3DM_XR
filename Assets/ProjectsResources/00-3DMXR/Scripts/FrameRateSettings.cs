using UnityEngine;

public class FrameRateSettings : MonoBehaviour
{
    public int targetFrameRate = 30;

    protected void Start()
    {
        Debug.Log("Setting frame rate to " + targetFrameRate + "fps");
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 1;
    }
}
