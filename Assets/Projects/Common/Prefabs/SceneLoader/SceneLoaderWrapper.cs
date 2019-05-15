using UnityEngine;

public sealed class SceneLoaderWrapper : MonoBehaviour
{
    #region PUBLIC_METHODS

    public void LoadMainMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneLoader.Instance.LoadSceneByName(sceneName);
    }

    public void OpenProject(string projectName)
    {
        SceneLoader.Instance.OpenProject(projectName);
    }

    #endregion // PUBLIC_METHODS
}
