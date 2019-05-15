using System;

/* <summary>
 * Define actions to trigger when the device detects a big roll acceleration.
 * RollLeft returns to WegmattenMenu. RollRight switches between AR and VR mode.
 * </summary>
 * */
public class WegmattenGyroAction : GyroActionManager
{
    #region PRIVATE_MEMBER_VARIABLES

    private TransitionManager mTransitionManager = null;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mTransitionManager = FindObjectOfType<TransitionManager>();
        if (!mTransitionManager) throw new NullReferenceException("Can not find any TransitionManager Object.");

        disableTime = 1.5f;
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PROTECTED_METHODS

    // <summary>
    // Override what to do on a RollLeftAction Event
    // </summary>
    override protected void RollLeftAction()
    {
        // only in viewer mode
        if (!TransitionManager.Globals.IsFullscreenMode)
        {
            SceneLoader.Instance.LoadSceneByName("Wegmatten-Menu");
        }
    }

    // <summary>
    // Override what to do on a RollRightAction Event
    // </summary>
    override protected void RollRightAction()
    {
        if (!TransitionManager.Globals.IsFullscreenMode)
        {
            mTransitionManager.SwitchMode(!TransitionManager.Globals.InAR);
        }
    }

    #endregion // PROTECTED_METHODS
}
