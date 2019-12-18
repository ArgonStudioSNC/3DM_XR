using System;
using UnityEngine;

public class ChairAnimatorManagerScript : MonoBehaviour
{
    public Transform chairTransform;
    [TextArea]
    public string chairDescription;
    public AnswerSliderScript answerSlider;


    private TouchEventManager m_touchEventManager;
    private Animator m_chairAnimator;


    protected void Awake()
    {
        m_touchEventManager = FindObjectOfType<TouchEventManager>();
        if (!m_touchEventManager)
        {
            throw new NullReferenceException("Can not find any TouchEventManager Object in the scene");
        }
        m_chairAnimator = chairTransform.GetComponent<Animator>();
    }

    protected void OnBecameVisible()
    {
        foreach (Renderer r in chairTransform.GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }
    }

    protected void Update()
    {
        if (m_touchEventManager.IsHit && m_touchEventManager.objectHit != null)
        {
            if (m_touchEventManager.objectHit == gameObject) PlayChair();
        }

        void PlayChair()
        {
            GetComponent<MeshRenderer>().enabled = false;
            m_chairAnimator.SetTrigger("Play");
            answerSlider.DisplayResult(chairDescription);

            foreach (Renderer r in chairTransform.GetComponentsInChildren<Renderer>())
            {
                r.enabled = true;
            }
        }
    }
}