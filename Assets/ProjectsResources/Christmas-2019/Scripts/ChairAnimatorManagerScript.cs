using System;
using UnityEngine;

public class ChairAnimatorManagerScript : MonoBehaviour
{
    public Animator chairAnimator;
    public GameObject playButton;
    [TextArea]
    public string chairDescription;
    public AnswerSliderScript answerSlider;


    private TouchEventManager m_touchEventManager;


    protected void Awake()
    {
        m_touchEventManager = FindObjectOfType<TouchEventManager>();
        if (!m_touchEventManager)
        {
            throw new NullReferenceException("Can not find any TouchEventManager Object in the scene");
        }
    }

    protected void Update()
    {
        if (m_touchEventManager.IsHit && m_touchEventManager.objectHit != null)
        {
            if (m_touchEventManager.objectHit == playButton.gameObject) PlayChair();
        }

        void PlayChair()
        {
            playButton.GetComponent<MeshRenderer>().enabled = false;
            chairAnimator.SetTrigger("Play");
            answerSlider.DisplayResult(chairDescription);
        }
    }
}