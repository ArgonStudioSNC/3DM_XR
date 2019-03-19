using UnityEngine;
using System;

/* <summary>
 * Allow to move the player when a click on nothing is detected in VR.
 * </summary>
 * */
public class Autowalk : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    [Tooltip("With this speed the player will move.")]
    public float speed;
    [Tooltip("Activate this check-box if the player shall move when the TouchEvent is triggered.")]
    public bool walkWhenTriggered;
    [Tooltip("Activate this check-box if you want to freeze the y-coordinate for the player. " +
             "For example in the case of you have no collider attached to your CardboardMain-GameObject" +
             "and you want to stay in a fixed level.")]
    public bool freezeYPosition;
    [Tooltip("This is the fixed y-coordinate.")]
    public float yOffset;
    [Tooltip("From where the order to walk or stop walk comes.")]
    public TouchEventManager touchEventManager;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private bool mIsWalking = false; // is the player currently walking
    private Transform mCameraTransform = null;
    private TransitionManager mTransitionManager = null;
    private Rigidbody mRigidbody;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mCameraTransform = Camera.main.transform;
        if (!mCameraTransform)
        {
            throw new NullReferenceException("Please add a main Camera to the scene.");
        }
        mTransitionManager = FindObjectOfType<TransitionManager>();
        if (!mTransitionManager)
        {
            throw new NullReferenceException("Can not find any TransitionManager object in the scene.");
        }
        mRigidbody = GetComponent<Rigidbody>();
        if (!mRigidbody) Debug.Log("No Rigidbody component attached to this object.");
    }

    protected void Update()
    {
        if (mTransitionManager.InAR) mIsWalking = false; // you can only walk in VR
        else
        {
            // walk when the Trigger is used 
            if (touchEventManager.IsHit && !touchEventManager.UIHit)
            {
                if (touchEventManager.objectHit == null || touchEventManager.objectHit.name != "VR_EMS/Logement") // click on no other trigger in the scene
                {
                    mIsWalking = !mIsWalking;
                    Debug.Log("isWalking=" + mIsWalking);
                }
            }

            if (mIsWalking)
            {
                // move in the direction the camera is looking at
                Vector3 direction = new Vector3(mCameraTransform.forward.x, 0, mCameraTransform.forward.z).normalized * speed * Time.deltaTime;
                Quaternion rotation = Quaternion.Euler(new Vector3(0, -transform.rotation.eulerAngles.y, 0));
                transform.Translate(rotation * direction);
            }
        }

        if (mRigidbody) mRigidbody.velocity = new Vector3(0, 0, 0); // disable collision accelerations

        if (freezeYPosition)
        {
            transform.position = new Vector3(transform.position.x, yOffset, transform.position.z); // fix the y coordinate
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
