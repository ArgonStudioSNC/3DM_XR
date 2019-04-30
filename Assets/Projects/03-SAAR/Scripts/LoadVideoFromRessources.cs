using UnityEngine;
using UnityEngine.Video;

public class LoadVideoFromRessources : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    public string videoPath;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    void Start()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
        if (!videoPlayer) throw new MissingComponentException("No VideoPlayer component found");

        var video = Resources.Load<VideoClip>(videoPath);
        if (!video) throw new MissingReferenceException("Cannot find the video in Resources at path " + videoPath);

        videoPlayer.clip = video;
    }

    #endregion // MONOBEHAVIOUR_METHODS
}
