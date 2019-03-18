using UnityEngine;

/* <summary>
 * This class uses the device gyroscope rotation RATE to trigger RollLeft and RollRight actions.
 * </summary>
 * */
public class GyroActionManager : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    [Tooltip("Sensitivity of the roll left action (0 to 4)")]
    [Range(0, 4)]
    public float rollLeftActionSensitivity = 2;
    [Tooltip("Sensitivity of the roll right action (0 to 4)")]
    [Range(0, 4)]
    public float rollRightActionSensitivity = 2;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PROTECTED_MEMBER_VARIABLES

    // how long are the roll actions disabled after one action occur
    protected float disableTime = 1;

    #endregion // PROTECTED_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private float mRollRate;
    private float timer;
    private bool IsGyroActionActive { get { return timer > disableTime; } }

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Update()
    {
        if (Input.deviceOrientation != DeviceOrientation.LandscapeLeft) timer = 0; // only works in LandscapeLeft orientation
        else timer = timer + Time.deltaTime;

        if (IsGyroActionActive)
        {
            mRollRate = DeviceRotation.GetRollRate(); // get the Device rotation rate (roll)
            if (mRollRate > rollLeftActionSensitivity)
            {
                timer = 0;
                RollLeftAction();
            }

            if (mRollRate < (-1 * rollRightActionSensitivity))
            {
                timer = 0;
                RollRightAction();
            }
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PROTECTED_METHODS

    /* <summary>
     * Called when a roll left action is detected.
     * </summary>
     * */
    protected virtual void RollLeftAction()
    {
        // empty
    }

    /* <summary>
     * Called when a roll right action is detected.
     * </summary>
     * */
    protected virtual void RollRightAction()
    {
        // empty
    }
    #endregion // PROTECTED_METHODS
}
