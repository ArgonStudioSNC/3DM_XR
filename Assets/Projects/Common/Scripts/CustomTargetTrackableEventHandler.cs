using UnityEngine;

public class CustomTargetTrackableEventHandler : ExtendedDefaultTrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES

    [Tooltip("Transform of the ImageTarget that should be tracked by the Argon ImageTarget")]
    public Transform trackedImageTarget;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private AudioSource mAudioSource;
    private Transform mArContent;

    private Vector3 mPosition;
    private Quaternion mRotation;
    private Vector3 mScale;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected override void Start()
    {
        base.Start();
        mAudioSource = GetComponent<AudioSource>();

        UpdateTrackedImageTarget();
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PROTECTED_METHODS

    protected override void OnTrackingFound()
    {
        mArContent = trackedImageTarget.Find("ARContent");
        if (!mArContent)
        {
            Debug.Log("No child with name ARContent found in GameObject " + trackedImageTarget.gameObject.name);
            base.OnTrackingFound();
            return;
        }
        mArContent.SetParent(transform);
        mArContent.localPosition = mPosition;
        mArContent.localRotation = mRotation;
        mArContent.localScale = mScale;

        // copy audio
        AudioSource audio = trackedImageTarget.gameObject.GetComponent<AudioSource>();
        if (audio) mAudioSource.clip = audio.clip;

        base.OnTrackingFound();
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();

        if (mArContent)
        {
            mArContent.SetParent(trackedImageTarget);
            mArContent.localPosition = new Vector3(0f, 0f, 0f);
            mArContent.localRotation = Quaternion.identity;
            mArContent.localScale = new Vector3(1f, 1f, 1f);
        }

        if (mAudioSource) mAudioSource.clip = null;
    }

    #endregion // PROTECTED_METHODS


    #region PUBLIC_METHODS

    public void UpdateTrackedImageTarget(Transform imageTarget = null)
    {
        if (imageTarget) trackedImageTarget = imageTarget;
        if (!trackedImageTarget)
        {
            Debug.Log("No trackedImageTarget attached to Argon imageTarget");
            return;
        }
        float scale = transform.localScale.x;
        mPosition = new Vector3((trackedImageTarget.localPosition.x - transform.localPosition.x) / scale, (trackedImageTarget.localPosition.y - transform.localPosition.y) / scale, (trackedImageTarget.localPosition.z - transform.localPosition.z) / scale);
        mRotation = Quaternion.Euler(trackedImageTarget.localRotation.eulerAngles.x - transform.localRotation.eulerAngles.x, trackedImageTarget.localRotation.eulerAngles.y - transform.localRotation.eulerAngles.y, trackedImageTarget.localRotation.eulerAngles.z - transform.localRotation.eulerAngles.z);
        mScale = new Vector3(trackedImageTarget.localScale.x / transform.localScale.x, trackedImageTarget.localScale.y / transform.localScale.y, trackedImageTarget.localScale.z / transform.localScale.z);
    }

    #endregion // PUBLIC_METHODS
}
