using UnityEngine;

public class ChristmasTrackableEventHandler : CustomTargetTrackableEventHandler
{
    #region PUBLIC_VARIABLES

    public AnswerSliderScript answerSliderScript;

    #endregion // PUBLIC_VARIABLES


    #region PROTECTED_METHODS

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        answerSliderScript.Close();
        Animator[] animators = GetComponentsInChildren<Animator>();

        foreach (Animator animator in animators)
        {
            animator.Play("idle", -1, 0f);
        }
    }

    #endregion // PROTECTED_METHODS
}
