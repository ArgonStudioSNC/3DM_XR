using UnityEngine;
using UnityEngine.UI;

public class DropdownMenu : MonoBehaviour
{
    public Transform[] closeSafeElements;

    private TouchEventManager m_touchEventManager;
    private Toggle m_masterToggle;

    protected void Awake()
    {
        m_touchEventManager = FindObjectOfType<TouchEventManager>();
        m_masterToggle = GetComponent<Toggle>();
    }

    protected void Update()
    {
        if (m_touchEventManager.IsHit)
        {
            GameObject selectedObject = m_touchEventManager.objectHit;
            if (selectedObject)
            {
                if (selectedObject.Equals(gameObject)) return;
                foreach (Transform t in closeSafeElements)
                {
                    foreach (Transform tChildren in t.GetComponentsInChildren<Transform>())
                    {
                        if (selectedObject.Equals(tChildren.gameObject)) return;
                    }
                }
            }

            m_masterToggle.isOn = false;
        }
    }
}
