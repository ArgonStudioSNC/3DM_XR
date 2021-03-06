using UnityEngine;
using System;

/* <summary>
 * Manager class attached to the WegmattenManager GameObject.
 * Act like a spate machine to hold the variables of the Wegmatten project. Variables are Trees (on/off), Gabarits(on/off) and project (id).
 * </summary>
 * */
public class WegmattenManager : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    [Tooltip("GameObject holding the models for alternative projects in AR")]
    public GameObject alternativesProjectsAR;
    [Tooltip("GameObject holding the models for alternative projects in VR")]
    public GameObject alternativesProjectsVR;
    [Tooltip("Toggle 3D GameObject to enable trees in viewer mode")]
    public GameObject trees3DToggle;
    [Tooltip("Toggle 3D GameObject to enable gabarits in viewer mode")]
    public GameObject gabarits3DToggle;
    [Tooltip("Trees model in AR mode")]
    public GameObject treesAR;
    [Tooltip("Gabarits model in AR mode")]
    public GameObject gabaritsAR;

    public int CurrentProject { get { return mCurrentProjectID; } set { mCurrentProjectID = value; } }
    public bool EnableTrees { get { return mEnableTrees; } set { mEnableTrees = value; } }
    public bool EnableGabarits { get { return mEnableGabarits; } set { mEnableGabarits = value; } }

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private TouchEventManager mTouchEventManager;

    // Wegmatten project main variables
    private bool mEnableTrees = false;
    private bool mEnableGabarits = false;
    private int mCurrentProjectID = 0;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mTouchEventManager = FindObjectOfType<TouchEventManager>();
        if (!mTouchEventManager)
        {
            throw new NullReferenceException("Can not find any TouchEventManager Object in the scene");
        }
    }

    protected void Update()
    {
        // if a touch is detected, check if the interaction modify the Wegmatten variables
        if (mTouchEventManager.IsHit)
        {
            GameObject obj = mTouchEventManager.objectHit;
            if (obj == null)
            {
                return;
            }
            else
                if (obj == alternativesProjectsVR || (obj == alternativesProjectsAR && !TransitionManager.IsFullscreenMode)) // alternative project hit (in VR or in AR and viewer mode)
            {
                mCurrentProjectID = (mCurrentProjectID + 1) % 2;
            }
            else if (obj == trees3DToggle) // trees3DToggle hit
            {
                mEnableTrees = !mEnableTrees;
            }
            else if (obj == gabarits3DToggle) // gabarits3DToggle hit
            {
                mEnableGabarits = !mEnableGabarits;
            }
        }

        // render the scene according to the current variables values
        // ======================
        foreach (Transform child in alternativesProjectsAR.transform)
        {
            child.gameObject.SetActive(false); // clean all
        }
        alternativesProjectsAR.transform.GetChild(mCurrentProjectID).gameObject.SetActive(true); // activate the correct project in AR

        foreach (Transform child in alternativesProjectsVR.transform)
        {
            child.gameObject.SetActive(false);
        }
        alternativesProjectsVR.transform.GetChild(mCurrentProjectID).gameObject.SetActive(true);

        treesAR.SetActive(mEnableTrees);

        gabaritsAR.SetActive(mEnableGabarits);
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    /*<summary>
     * Switch to the next project in the list. Actually on 2 projects (Logement / EMS).
     * </summary>
     * */
    public void SwitchProject()
    {
        mCurrentProjectID = (mCurrentProjectID + 1) % 2;
    }

    /*<summary>
     * Turn trees on or off.
    * </summary>
    * */
    public void SwitchTrees()
    {
        mEnableTrees = !mEnableTrees;
    }

    /*<summary>
     * Turn gabarits on or off.
    * </summary>
    * */
    public void SwitchGabarits()
    {
        mEnableGabarits = !mEnableGabarits;
    }

    #endregion // PUBLIC_METHODS
}
