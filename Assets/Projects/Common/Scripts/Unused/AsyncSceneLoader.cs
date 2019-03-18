using UnityEngine;
using System.Collections;

public class AsyncSceneLoader : MonoBehaviour
{
    #region PUBLIC_MEMBERS_VARIABLES

    public float loadingDelay = 2.5f;

    #endregion //PUBLIC_MEMBERS_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    void Start()
    {
        StartCoroutine(LoadNextSceneAfter(loadingDelay));
    }

    #endregion //MONOBEHAVIOUR_METHODS


    #region PRIVATE_METHODS

    private IEnumerator LoadNextSceneAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    #endregion //PRIVATE_METHODS
}
