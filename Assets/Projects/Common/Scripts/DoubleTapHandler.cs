using UnityEngine;

/* <summary>
 * Helper function to add action on DoubleTap.
 * Override Tap to define your custom Tap action. Override OnDoubleTap to define the DoubleTap action.
 * </summary>
 * */
public class DoubleTapHandler : MonoBehaviour
{
    #region PRIVATE_MEMBERS_VARIABLES

    private delegate bool TapFunction();
    private const float DOUBLE_TAP_MAX_DELAY = 0.5f;//seconds
    private int mTapCount = 0;
    private float mTimeSinceLastTap = 0;

    #endregion //PRIVATE_MEMBERS_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTapCount = 0;
        mTimeSinceLastTap = 0;
    }

    protected virtual void Update()
    {
        if (mTapCount == 1)
        {
            mTimeSinceLastTap += Time.deltaTime;
            if (mTimeSinceLastTap > DOUBLE_TAP_MAX_DELAY)
            {
                // reset touch count and timer
                mTapCount = 0;
                mTimeSinceLastTap = 0;
            }
        }
        else if (mTapCount == 2)
        {
            // we got a double tap
            OnDoubleTap();

            // reset touch count and timer
            mTimeSinceLastTap = 0;
            mTapCount = 0;
        }

        IncrementTap(Tap);
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PROTECTED_METHODS

    /* <summary>
     * Boolean function defining what is considered a touch action.
     * </summary>
     * */
    protected virtual bool Tap()
    {
        return Input.GetButtonDown("Fire1");
    }

    /* <summary>
     * Boolean function defining what do when a double tap occur.
     * </summary>
     * */
    protected virtual void OnDoubleTap()
    {
        // empty
    }

    #endregion //PROTECTED_METHODS


    #region PRIVATE_METHODS

    private void IncrementTap(TapFunction func)
    {
        if (func()) mTapCount++;
    }

    #endregion //PRIVATE_METHODS
}
