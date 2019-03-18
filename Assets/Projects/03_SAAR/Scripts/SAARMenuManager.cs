using UnityEngine;
using UnityEngine.UI;

/* <summary>
 * </summary>
 * */
public class SAARMenuManager : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    public GameObject projectPanel;
    public GameObject UIMask;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private Animator[] animators;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        projectPanel.SetActive(true);
        animators = FindObjectsOfType<Animator>();
        ProjectPanelEnable(false);
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    public void AnimationsEnable(bool reverse)
    {
        foreach (Animator anim in animators)
        {
            anim.enabled = reverse;
        }
    }

    public void ProjectPanelEnable(bool reverse)
    {
        SetGraphicsActive(projectPanel, reverse);
        UIMask.SetActive(reverse);
        AnimationsEnable(!reverse);
    }

    #endregion // PUBLIC_METHODS


    #region PRIVATE_METHODS

    // disable all Graphic components of a GameObject and his children. Scripts keep on working.
    private void SetGraphicsActive(GameObject go, bool active)
    {
        foreach (Graphic graphic in go.GetComponentsInChildren<Graphic>())
        {
            graphic.enabled = active;
        }
    }

    #endregion // PRIVATE_METHODS
}
