using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class AssetBundleManager : MonoBehaviour
{
    public enum LoadingState
    {
        Inactive,
        Downloading,
        LoadingFromCache,
        LoadingFromStreamingAssets,
        UnitySplashScreen,
        Success,
        Error
    }

    [Serializable]
    internal class Bundle
    {
        public string name;
        public uint version;

        public Bundle(string name, uint version)
        {
            this.name = name;
            this.version = version;
        }
    }

    #region PUBLIC_MEMBER_VARIABLES

    public LoadingState ActiveLoadingState { get; set; }

    public UnityWebRequest WWW { get; set; }
    public AsyncOperation CurrentLoadingOperation { get; set; }
    public string ErrorMessage { get; set; }

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private List<AssetBundle> mLoadedAssetBundles = new List<AssetBundle>();
    private List<Bundle> mBundleVersions;
    private string fileText;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Update()
    {
        switch (ActiveLoadingState)
        {
            case LoadingState.Inactive:
            case LoadingState.Downloading:
                break;

            case LoadingState.LoadingFromCache:
                if (CurrentLoadingOperation.progress == 0.9f)
                {
                    CurrentLoadingOperation.allowSceneActivation = true;
                    ActiveLoadingState = LoadingState.Success;
                }
                break;

            case LoadingState.LoadingFromStreamingAssets:

                if (CurrentLoadingOperation != null)
                {
                    if (CurrentLoadingOperation.isDone) ActiveLoadingState = LoadingState.Success;
                }
                else if (WWW != null)
                {
                    if (WWW.isDone) ActiveLoadingState = LoadingState.Success;
                }

                break;

            case LoadingState.UnitySplashScreen:
                if (!SplashScreen.isFinished) break;

                if (CurrentLoadingOperation != null)
                {
                    if (CurrentLoadingOperation.isDone) ActiveLoadingState = LoadingState.Success;
                }
                else ActiveLoadingState = LoadingState.Success;

                break;

            case LoadingState.Success:
            case LoadingState.Error:
            default:
                break;
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    public void SetInactive()
    {
        WWW = null;
        CurrentLoadingOperation = null;
        ErrorMessage = null;
        ActiveLoadingState = LoadingState.Inactive;
    }

    public IEnumerator LoadSceneByName(string sceneName, List<string> requieredBundles = null)
    {
        yield return StartCoroutine(UpdateVersionsCoroutine());

        // DOWLOADING BUNDLES
        if (requieredBundles != null) // else jump to loading
        {
            ActiveLoadingState = LoadingState.Downloading;

            foreach (string requieredBundle in requieredBundles)
            {
                uint version = 0;
                if (mBundleVersions != null)
                {
                    foreach (Bundle availableBundle in mBundleVersions)
                    {
                        if (availableBundle.name == requieredBundle)
                        {
                            version = availableBundle.version;
                            break;
                        }
                    }
                }
                if (version == 0)
                {
                    ErrorMessage = "No version file available";
                    ActiveLoadingState = LoadingState.Error;
                    yield break;
                }

                yield return StartCoroutine(DownloadAssetBundle(requieredBundle, version));
                if (ActiveLoadingState == LoadingState.Error) yield break;
            }
        }

        // LOADING SCENE
        Debug.Log("Loading scene " + sceneName + "...");
        CurrentLoadingOperation = SceneManager.LoadSceneAsync(sceneName);
        CurrentLoadingOperation.allowSceneActivation = false;
        ActiveLoadingState = LoadingState.LoadingFromCache;
    }

    public void LoadAssetBundleFromStreamingAssets(string assetBundleName)
    {
        string assetBundlePath = ResolveStreamingAssetsPath(assetBundleName);
        CurrentLoadingOperation = AssetBundle.LoadFromFileAsync(assetBundlePath);
        /*
#if UNITY_IOS || UNITY_EDITOR
        Debug.Log("Loading " + assetBundleName + " from StreamingAssets with LoadFromFileAsync\nPATH= " + assetBundlePath);
        CurrentLoadingOperation = AssetBundle.LoadFromFileAsync(assetBundlePath);
#elif UNITY_ANDROID
        assetBundlePath = new Uri(assetBundlePath).AbsolutePath;
        Debug.Log("Loading " + assetBundleName + " from StreamingAssets with UnityWebRequest\nPATH= " + assetBundlePath);
        StartCoroutine(LoadAssetBundleFromStreamingAssetsAndroid(assetBundlePath));
#endif
*/
        ActiveLoadingState = LoadingState.LoadingFromStreamingAssets;
    }

    public void UnloadAllAssets()
    {
        foreach (AssetBundle assetBundle in mLoadedAssetBundles)
        {
            Debug.Log("Unloading asset bundle " + assetBundle.name);
            assetBundle.Unload(false);
        }
        mLoadedAssetBundles = new List<AssetBundle>();
    }

    #endregion // PUBLIC_METHODS


    #region PRIVATE_METHODS    

    private IEnumerator UpdateVersionsCoroutine()
    {
        mBundleVersions = null;
        string versioningFileName = Path.Combine("AssetBundles", "versioning_" + Application.version + ".json");
        Uri uri = ResolveFileUri(versioningFileName);

        using (WWW = UnityWebRequest.Get(uri))
        {
            yield return WWW.SendWebRequest();
            if (WWW.isNetworkError | WWW.isHttpError)
            {
                Debug.Log("Failed to download a new version file");

                string jsonFile;
                try
                {
                    jsonFile = DeserializeData(ResolvePersistentDataPath(versioningFileName));
                }
                catch (FileNotFoundException e)
                {
                    Debug.Log("Can't find a versioning file: " + e);
                    yield break;
                }
                yield return mBundleVersions = JsonUtility.FromJson<JsonArrayWrapper<Bundle>>(jsonFile).array;
            }
            else
            {
                string jsonFile = WWW.downloadHandler.text;
                SerializeData(ResolvePersistentDataPath(versioningFileName), jsonFile);
                //SerializeData(Application.persistentDataPath + "/" + versioningFileName, jsonFile);
                yield return mBundleVersions = JsonUtility.FromJson<JsonArrayWrapper<Bundle>>(jsonFile).array;
            }
        }
    }

    private string DeserializeData(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException("No file at location " + path);
        return File.ReadAllText(path);
    }

    private void SerializeData(string path, string file)
    {
        try
        {
            if (!File.Exists(path))
            {
                FileStream fs = File.Create(path);
                fs.Close();
            }
            File.WriteAllText(path, file);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private IEnumerator DownloadAssetBundle(string assetBundleName, uint assetBundleVersion)
    {
        Uri uri = ResolveFileUri(Path.Combine("AssetBundles", assetBundleVersion.ToString(), assetBundleName));
        Debug.Log("Downloading bundle " + assetBundleName + " version " + assetBundleVersion.ToString() + "\n URI: " + uri);

        WWW = UnityWebRequestAssetBundle.GetAssetBundle(uri, assetBundleVersion, 0);

        using (WWW)
        {
            yield return WWW.SendWebRequest();

            if (WWW.isNetworkError)
            {
                ErrorMessage = "No connexion available";
                ActiveLoadingState = LoadingState.Error;
            }
            else if (WWW.isHttpError)
            {
                ErrorMessage = "Error: HTTP response code " + WWW.responseCode;
                ActiveLoadingState = LoadingState.Error;
            }
            else
            {
                mLoadedAssetBundles.Add(DownloadHandlerAssetBundle.GetContent(WWW));
            }
        }
    }

    private IEnumerator LoadAssetBundleFromStreamingAssetsAndroid(string assetBundlePath)
    {
        using (WWW = UnityWebRequestAssetBundle.GetAssetBundle(assetBundlePath, 0))
        {
            yield return WWW.SendWebRequest();

            //if (WWW.isNetworkError || WWW.isHttpError) Debug.Log("ERROR: Fail to load from streaming assets!");

            DownloadHandlerAssetBundle.GetContent(WWW);
        }
    }

    private string ResolveStreamingAssetsPath(string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "3DMXR");
#if UNITY_EDITOR
        path = Path.Combine(path, "Editor", fileName);
#elif UNITY_IOS
        path = Path.Combine(path, "iOS", fileName);
#elif UNITY_ANDROID
        path = Path.Combine(path, "Android", fileName);
#endif
        return path;
    }

    private Uri ResolveFileUri(string fileName)
    {
        Uri uri = new Uri("https://argonstudio.ch/");
        string path = "3DMXR/";
#if UNITY_EDITOR
        path = path + "Editor/";
#elif UNITY_IOS
        path = path + "iOS/";
#elif UNITY_ANDROID
        path = path + "Android/";
#endif
        Debug.Log(new Uri(uri, path + fileName));

        return new Uri(uri, path + fileName);
    }

    private string ResolvePersistentDataPath(string fileName)
    {
        string path = Application.persistentDataPath;
#if UNITY_EDITOR
        path = Path.Combine(path, "3DMXR", "Editor");
#elif UNITY_IOS
        path = Path.Combine(path, "3DMXR", "iOS");
#elif UNITY_ANDROID
        path = Path.Combine(path, "3DMXR", "Android");
#endif
        path = Path.Combine(path, fileName);
        string directories = Path.GetDirectoryName(path);
        if (!Directory.Exists(directories)) Directory.CreateDirectory(directories);

        return path;
    }

    #endregion // PRIVATE_METHODS

}
