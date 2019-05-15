/*==============================================================================
Copyright (c) 2019 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
==============================================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES
    public bool IsFadingInProgress { get; private set; }
    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES
    [SerializeField] GameObject[] objectsToNotifyOfFaderEvents = null;
    BlackMaskBehaviour blackMaskBehaviour;
    float fadeStartTime;
    float fadeDuration;
    float fadeFactor;
    float fadeProgress;
    bool fadeOut;
    List<IFaderNotify> faderNotify;
    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    void Start()
    {
        this.blackMaskBehaviour = FindObjectOfType<BlackMaskBehaviour>();

        this.IsFadingInProgress = false;

        this.faderNotify = new List<IFaderNotify>();

        foreach (GameObject gameObj in this.objectsToNotifyOfFaderEvents)
        {
            var notifyObj = gameObj.GetComponent<IFaderNotify>();
            if (notifyObj != null && gameObj.activeInHierarchy)
            {
                this.faderNotify.Add(notifyObj);
            }
        }
    }

    void Update()
    {
        if (this.IsFadingInProgress)
        {
            if (this.fadeProgress < 0 || this.fadeProgress > 1)
            {
                this.IsFadingInProgress = false;

                this.fadeProgress = Mathf.Clamp01(this.fadeProgress);

                NotifyFaderEventHandlerObjects();
            }
            else
            {
                this.fadeProgress = (Time.time - this.fadeStartTime) / this.fadeDuration;

                if (this.fadeOut)
                {
                    this.fadeFactor = Mathf.SmoothStep(0, 1, this.fadeProgress);
                }
                else
                {
                    this.fadeFactor = Mathf.SmoothStep(1, 0, this.fadeProgress);
                }

                this.fadeFactor = Mathf.Clamp01(this.fadeFactor);


                if (this.blackMaskBehaviour)
                {
                    this.blackMaskBehaviour.SetFadeFactor(this.fadeFactor);
                }
            }
        }

    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS
    public void FadeOut(float duration = 1.5f)
    {
        // Class member this.fadeOut is set to true (fading out) or false (fading in)
        // when a new fade begins. Before queuing up a new fade, we check to make
        // sure that the new fade is not the same as this.fadeOut. If it is the same,
        // then we ignore the request.

        // If fadeOut is false (fade in), then queue up a fade-out, otherwise ignore.
        if (!this.fadeOut)
        {
            StartCoroutine(TriggerFade(duration, true));
        }
    }

    public void FadeIn(float duration = 1.5f)
    {
        // Class member this.fadeOut is set to true (fading out) or false (fading in)
        // when a new fade begins. Before queuing up a new fade, we check to make
        // sure that the new fade is not the same as this.fadeOut. If it is the same,
        // then we ignore the request.

        // If fadeOut is true (fade out), then queue up a fade-in, otherwise ignore.
        if (this.fadeOut)
        {
            StartCoroutine(TriggerFade(duration, false));
        }
    }
    #endregion // PUBLIC_METHODS


    #region PRIVATE_METHODS
    IEnumerator TriggerFade(float duration, bool isFadeOut)
    {
        VLog.Log("yellow", "Queueing Fade " + (isFadeOut ? "Out" : "In"));

        yield return new WaitUntil(() => !this.IsFadingInProgress);

        if (!this.IsFadingInProgress)
        {
            VLog.Log("yellow", "Starting Fade " + (isFadeOut ? "Out" : "In"));
            this.fadeStartTime = Time.time;
            this.fadeOut = isFadeOut;
            this.fadeDuration = duration;
            this.IsFadingInProgress = true;
        }
    }

    void NotifyFaderEventHandlerObjects()
    {
        foreach (IFaderNotify notifyObj in this.faderNotify)
        {
            if (this.fadeOut)
            {
                notifyObj.OnFadeOutFinished();
            }
            else
            {
                notifyObj.OnFadeInFinished();
            }
        }
    }
    #endregion // PRIVATE_METHODS
}
