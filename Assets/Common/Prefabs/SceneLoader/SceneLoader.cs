﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* <summary>
 * Helper functions to load and unload scene and navigate between them.
 * </summary>
 * */
public class SceneLoader : AssetBundleManager
{
    #region PUBLIC_MEMBER_VARIABLES

    public bool EditorDebugMode = false;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PROTECTED_MEMBER_VARIABLES

    protected static List<Project> projectsList;

    #endregion // PROTECTED_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    [SerializeField]
    private LoadingScreen loadingScreen = null;

    private Project mOpenProject = null;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected void Start()
    {
        loadingScreen.Reset();
        loadingScreen.Show();
        LoadAssetBundleFromStreamingAssets("3dmxr_streamingasset");

        var jsonTextFile = Resources.Load<TextAsset>("Database/projects_DB");
        projectsList = JsonUtility.FromJson<JsonArrayWrapper<Project>>(jsonTextFile.text).array;
        Debug.Log("INFO: The projects database contains " + projectsList.Count + " projects.");
    }

    new protected void Update()
    {
        base.Update();

#if UNITY_ANDROID
        // On Android, the Back button is mapped to the Esc key
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(mOpenProject == null) Application.Quit();
            LoadMainMenu();
        }
#endif
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    public static SceneLoader Instance { get; set; }

    /* <summary>
     * Load the main menu scene.
     * </summary>
     * */
    public void LoadMainMenu()
    {
        Debug.Log("Unloading current scene's assets and loading MainMenu...");
        UnloadProjects();
        SceneManager.LoadScene("3DMXR-Menu");
    }

    public void OpenProject(string projectName)
    {
        foreach (Project p in projectsList)
        {
            if (p.name == projectName)
            {
                Debug.Log("Open project " + projectName + "...");
                LoadSceneByName(p.firstScene, p.requieredBundles);
                mOpenProject = p;
                return;
            }
        }
        Debug.Log("ERROR: Can't find the project " + projectName + "!");
    }

    /* <summary>
     * Load the scene with the name passed as argument.
     * </summary>
     * <param name=scene>name of the scene to load</param>
     * */
    public void LoadSceneByName(string sceneName, List<string> requieredBundles = null)
    {
        loadingScreen.Reset();
        loadingScreen.Show();

#if UNITY_EDITOR
        if (EditorDebugMode)
        {
            SceneManager.LoadSceneAsync(sceneName);
            loadingScreen.Hide();
            return;
        }
#endif

        StartCoroutine(LoadSceneFromBundle(sceneName, requieredBundles));
    }

    #endregion //PUBLIC_METHODS


    #region PRIVATE_METHODS

    private void UnloadProjects()
    {
        if (mOpenProject != null) // a project is open
        {
            UnloadAllAssets();
            mOpenProject = null;
        }
    }

    #endregion //PRIVATE_METHODS
}

[Serializable]
public class Project
{
    public string name;
    public string firstScene;
    public List<string> requieredBundles;

    public Project(string name, string firstScene, List<string> requieredBundles)
    {
        this.name = name;
        this.firstScene = firstScene;
        this.requieredBundles = requieredBundles;
    }
}

[Serializable]
public class JsonArrayWrapper<T>
{
    public List<T> array;
}