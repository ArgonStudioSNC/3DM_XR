using UnityEngine;
using UnityEngine.UI;

public class VRButtonOver : MonoBehaviour
{
    public Graphic graphicElement;
    public bool cheat = false;

    private TouchEventManager m_toucheEventManager;

    protected void Awake()
    {
        m_toucheEventManager = FindObjectOfType<TouchEventManager>();
    }

    protected void Update()
    {
        if (IsGazeOver())
        {
            graphicElement.transform.localScale = new Vector3(0.8f, 0.8f, 1);
            if (m_toucheEventManager.IsHit)
            {
                Toggle toggle = GetComponent<Toggle>();
                if (toggle) toggle.isOn = !toggle.isOn;

                Button button = GetComponent<Button>();
                if (button)
                {
                    // full cheat
                    if (cheat) SceneLoader.Instance.LoadSceneByName("Bahnhofsareal-Altdorf-Menu");

                    button.onClick.Invoke();
                }
            }
        }
        else
        {
            graphicElement.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private bool IsGazeOver()
    {
        if (m_toucheEventManager.objectHit)
        {
            return m_toucheEventManager.objectHit.Equals(gameObject);
        }
        return false;
    }
}
