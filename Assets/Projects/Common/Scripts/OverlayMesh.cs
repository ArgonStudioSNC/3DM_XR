using UnityEngine;
using Vuforia;
using System;

/* <summary>
 * Clip an mesh to the clip plane to allow 2D object rendering in stereoscopic mode.
 * </summary>
 * */
public class OverlayMesh : MonoBehaviour
{
    #region PUBLIC_MEMBERS_VARIABLES

    public float meshScale = 0.012f; // relative to viewport

    #endregion // PUBLIC_MEMBERS_VARIABLES


    #region PRIVATE_MEMBERS_VARIABLES

    private Camera mCamera;

    #endregion // PRIVATE_MEMBERS_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        mCamera = Camera.main;
        if (!mCamera)
        {
            throw new NullReferenceException("Please add a main camera to the scene.");
        }
        VuforiaARController.Instance.RegisterVuforiaInitializedCallback(OnVuforiaStarted);
    }

    protected void Update()
    {
        if (mCamera.projectionMatrix.m00 > 0 || mCamera.projectionMatrix.m00 < 0)
        {
            // We adjust the reticle depth
            if (VideoBackgroundManager.Instance.VideoBackgroundEnabled)
            {
                // When the frustum skewing is applied (e.g. in AR view),
                // we shift the Reticle at the background depth,
                // so that the reticle appears in focus in stereo view
                BackgroundPlaneBehaviour bgPlane = mCamera.GetComponentInChildren<BackgroundPlaneBehaviour>();

                float bgDepth = (bgPlane != null) ? bgPlane.transform.localPosition.z : (mCamera.nearClipPlane);
                if (bgDepth > mCamera.nearClipPlane)
                {
                    transform.localPosition = Vector3.forward * bgDepth;
                }
                else
                {
                    transform.localPosition = Vector3.forward * (mCamera.nearClipPlane + 0.5f);
                }
            }
            else
            {
                // if the frustum is not skewed, then we apply a default depth (which works nicely in VR view)
                transform.localPosition = Vector3.forward * (mCamera.nearClipPlane + 0.5f);
            }

            // We scale the mesh to be a small % of viewport
            float localDepth = this.transform.localPosition.z;
            float tanHalfFovX = 1.0f / mCamera.projectionMatrix[0, 0];
            if (!TransitionManager.isFullscreenMode) tanHalfFovX = tanHalfFovX / 2.0f; // correction for viewer mode
            float tanHalfFovY = 1.0f / mCamera.projectionMatrix[1, 1];
            float minTanFov = Mathf.Min(tanHalfFovX, tanHalfFovY);
            float minViewSize = 2 * minTanFov * localDepth;
            transform.localScale = new Vector3(meshScale * minViewSize, meshScale * minViewSize, 1);
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region VUFORIA_CALLBACKS

    void OnVuforiaStarted()
    {
        mCamera = DigitalEyewearARController.Instance.PrimaryCamera ?? Camera.main;
    }

    #endregion // VUFORIA_CALLBACKS
}
