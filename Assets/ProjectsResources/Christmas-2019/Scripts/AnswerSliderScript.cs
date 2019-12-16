using UnityEngine;
using UnityEngine.UI;

public class AnswerSliderScript : MonoBehaviour
{
    public float delay = 1.8f;


    private string m_sliderString;
    private Text m_sliderText;
    private Animator m_animator;


    protected void Awake()
    {
        m_sliderText = GetComponentInChildren<Text>();
        m_animator = GetComponent<Animator>();
    }

    protected void Update()
    {
        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            m_sliderText.text = m_sliderString;
        }
    }


    public void DisplayResult(string newText)
    {
        m_animator.SetBool("isOpen", false);
        m_sliderString = newText;
        Invoke("DelayedOpen", delay);
    }

    public void Close()
    {
        m_animator.SetBool("isOpen", false);
    }


    private void DelayedOpen()
    {
        m_animator.SetBool("isOpen", true);
    }
}
