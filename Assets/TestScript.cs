using UnityEngine;

public class TestScript : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
        {
            FindObjectOfType<RealTransitionManager>().SwitchMode(false);
        }
    }
}
