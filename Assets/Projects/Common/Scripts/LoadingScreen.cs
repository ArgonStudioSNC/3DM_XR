using UnityEngine;
using UnityEngine.UI;
using static AssetBundleManager;

public class LoadingScreen : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    [SerializeField]
    private RawImage loadingLogo = null;
    [SerializeField]
    private Transform loadingBox = null;
    [SerializeField]
    private Text loadingStateText = null;
    [SerializeField]
    private RectTransform progressBar = null;
    [SerializeField]
    private Text progressText = null;

    // Initialize as the initial local scale of the bar fill game object. Used to cache the Y-value
    private Vector3 mBarFillLocalScale;
    private float mTimeElapsed;
    private float mMinTimeToShow;
    private AssetBundleManager mAssetBundleManager;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Awake()
    {
        // Save the bar fill's initial local scale
        mBarFillLocalScale = progressBar.localScale;
    }

    protected void Start()
    {
        mAssetBundleManager = FindObjectOfType<AssetBundleManager>();
        if (!mAssetBundleManager)
        {
            throw new MissingComponentException("No AssetBundleManager found for LoadingScreen");
        }
    }

    protected void Update()
    {
        mTimeElapsed += Time.deltaTime;

        switch (mAssetBundleManager.ActiveLoadingState)
        {
            case LoadingState.Inactive:
                break;

            case LoadingState.Downloading:
                loadingBox.gameObject.SetActive(true);
                loadingStateText.text = "Téléchargement des paquets du projet";
                SetProgress(mAssetBundleManager.WWW.downloadProgress);
                loadingLogo.rectTransform.Rotate(Vector3.forward, 90.0f * Time.deltaTime);
                break;

            case LoadingState.LoadingFromCache:
            case LoadingState.LoadingFromStreamingAssets:
                if (mAssetBundleManager.CurrentLoadingOperation != null)
                {
                    SetProgress(mAssetBundleManager.CurrentLoadingOperation.progress);
                }
                else if (mAssetBundleManager.WWW != null)
                {
                    SetProgress(mAssetBundleManager.WWW.downloadProgress);
                }

                loadingLogo.rectTransform.Rotate(Vector3.forward, 90.0f * Time.deltaTime);
                break;

            case LoadingState.UnitySplashScreen:
                break;

            case LoadingState.Error:
                loadingBox.gameObject.SetActive(true);
                loadingStateText.text = mAssetBundleManager.ErrorMessage;
                loadingStateText.color = Color.red;
                progressText.text = "Annuler";

                if (Input.GetButtonDown("Fire1"))
                {
                    mAssetBundleManager.SetInactive();
                    Reset();
                    Hide();
                }
                break;

            case LoadingState.Success:
                loadingLogo.rectTransform.Rotate(Vector3.forward, 90.0f * Time.deltaTime);
                if (mTimeElapsed >= mMinTimeToShow)
                {
                    Hide();
                    mAssetBundleManager.SetInactive();
                }
                break;

            default:
                break;
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    public void Show()
    {
        GetComponent<Canvas>().enabled = true;
    }

    public void Hide()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void Reset(int minTimeToShow = 0)
    {
        loadingStateText.color = Color.white;
        loadingBox.gameObject.SetActive(false);
        mMinTimeToShow = minTimeToShow;
        SetProgress(0f); // reset progress bar
        mTimeElapsed = 0f; // reset time elapsed
    }

    #endregion // PUBLIC_METHODS


    #region PRIVATE_METHODS

    // Updates the UI based on the progress
    private void SetProgress(float progress)
    {
        mBarFillLocalScale.x = progress;
        progressBar.localScale = mBarFillLocalScale;
        progressText.text = Mathf.CeilToInt(progress * 100).ToString() + "%";
    }

    #endregion // PRIVATE_METHODS
}
