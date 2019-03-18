using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class TestsManager : MonoBehaviour
{
    public GameObject[] imageTarget;
    public Text text;

    private int currentID = 0;

    // Use this for initialization
    void Start()
    {
        //  UpdateTarget();
    }

    // Update is called once per frame
    void Update()
    {
        ImageTargetBehaviour imageTargetBehaviour = imageTarget[currentID].GetComponent<ImageTargetBehaviour>();
        if (imageTargetBehaviour) text.text = imageTargetBehaviour.ImageTarget.Name;
    }

    public void Next()
    {
        currentID = (currentID + 1) % imageTarget.Length;
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        foreach (GameObject go in imageTarget)
        {
            go.GetComponent<TrackableBehaviour>().UnregisterTrackableEventHandler(go.GetComponent<ExtendedDefaultTrackableEventHandler>());
        }
        imageTarget[currentID].GetComponent<TrackableBehaviour>().RegisterTrackableEventHandler(imageTarget[currentID].GetComponent<ExtendedDefaultTrackableEventHandler>());
    }
}
