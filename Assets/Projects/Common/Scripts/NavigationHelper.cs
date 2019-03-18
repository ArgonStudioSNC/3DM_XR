using UnityEngine;

public sealed class NavigationHelper : MonoBehaviour
{
    #region PUBLIC_METHODS

    public void LoadMainMenu()
    {
        SceneController.Instance.LoadMainMenu();
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneController.Instance.LoadSceneByName(sceneName);
    }

    public void OpenProject(string projectName)
    {
        SceneController.Instance.OpenProject(projectName);
    }

    #endregion // PUBLIC_METHODS
}
