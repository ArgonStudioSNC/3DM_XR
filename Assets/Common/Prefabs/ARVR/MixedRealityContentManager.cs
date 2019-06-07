using UnityEngine;
using Vuforia;

/* <summary>
 * Handles the activation of the content between different modes. 
 * </summary>
 * */
public class MixedRealityContentManager : MonoBehaviour
{
    [SerializeField] Transform[] UIContent = null;

    [Header("GameObjects visibility")]
    [Tooltip("Give all game objects visible in HANDHELD_AR mode.")]
    [SerializeField] Transform[] handheldARContent = null;
    [Tooltip("Give all game objects visible in VIEWER_AR mode.")]
    [SerializeField] Transform[] viewerARContent = null;
    [Tooltip("Give all game objects visible in HANDHELD_VR mode.")]
    [SerializeField] Transform[] handheldVRContent = null;
    [Tooltip("Give all game objects visible in VIEWER_VR mode.")]
    [SerializeField] Transform[] viewerVRContent = null;


    #region PUBLIC_MEMBER_VARIABLES

    public void UpdateContent(MixedRealityController.Mode mode)
    {
        VLog.Log("cyan", "MixedRealityContentManager.UpdateContent() called: " + gameObject.name);

        EnableContentSet(handheldARContent, false);
        EnableContentSet(viewerARContent, false);
        EnableContentSet(handheldVRContent, false);
        EnableContentSet(viewerVRContent, false);

        switch (mode)
        {
            case MixedRealityController.Mode.HANDHELD_AR:
            case MixedRealityController.Mode.HANDHELD_AR_DEVICETRACKER:
                EnableContentSet(handheldARContent, true);
                break;
            case MixedRealityController.Mode.VIEWER_AR:
            case MixedRealityController.Mode.VIEWER_AR_DEVICETRACKER:
                EnableContentSet(viewerARContent, true);
                break;
            case MixedRealityController.Mode.HANDHELD_VR:
                EnableContentSet(handheldVRContent, true);
                break;
            case MixedRealityController.Mode.VIEWER_VR:
                EnableContentSet(viewerVRContent, true);
                break;
        }
    }

    public void ShowUI(bool enable)
    {
        foreach (Transform t in UIContent)
        {
            UtilityHelper.EnableRendererColliderCanvas(t.gameObject, enable);
        }
    }

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_METHODS

    private void EnableContentSet(Transform[] contentSet, bool enable)
    {
        foreach (var transform in contentSet)
        {
            transform.gameObject.SetActive(enable);
        }
    }

    #endregion // PRIVATE_METHODS
}
