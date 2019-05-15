using UnityEngine;

/* <summary>
 * Script for the ARUserInterface GameObject.
 * Enable or disable the children depending on Mobile or Viewer mode.
 * */
public class ARUserInterfaceScript : MonoBehaviour
{
    #region MONOBEHAVIOUR_METHODS

    protected void Update()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(!TransitionManager.Globals.IsFullscreenMode);
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
