using UnityEngine;
using UnityEngine.UI;

public class ProjectPanelScript : MonoBehaviour
{
    public Text titleText;
    public Text studentNameText;
    public Text descriptionText;
    public GameObject UIMask;

    private SAARMenuManager mSAARMenuManager;

    void Start()
    {
        mSAARMenuManager = FindObjectOfType<SAARMenuManager>();
    }

    public void Setup(string title, string studentName, string description, string trackableName)
    {
        titleText.text = title;
        studentNameText.text = studentName;
        descriptionText.text = description;
        SAARManager.trackableName = trackableName;
        mSAARMenuManager.ProjectPanelEnable(true);
    }
}
