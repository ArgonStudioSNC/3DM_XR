using UnityEngine;
using System;

/* <summary>
 * Allow to move the player when a click on nothing is detected in VR.
 * </summary>
 * */
public class Autowalk : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    [Tooltip("With this speed the player will move.")]
    public float speed;
    [Tooltip("Activate this check-box if the player shall move when the TouchEvent is triggered.")]
    public bool walkWhenTriggered;
    [Tooltip("Activate this check-box if you want to freeze the y-coordinate for the player. " +
             "For example in the case of you have no collider attached to your CardboardMain-GameObject" +
             "and you want to stay in a fixed level.")]
    public bool freezeYPosition;
    [Tooltip("This is the fixed y-coordinate.")]
    public float yOffset;
    [Tooltip("From where the order to walk or stop walk comes.")]
    public TouchEventManager touchEventManager;

    public Vector3 resetPosition;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private bool m_isWalking = false; // is the player currently walking
    private Transform m_cameraTransform = null;
    private Rigidbody m_rigidbody;
    private bool m_gravityOn;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        m_cameraTransform = Camera.main.transform;
        if (!m_cameraTransform)
        {
            throw new NullReferenceException("Please add a main Camera to the scene.");
        }

        m_rigidbody = GetComponent<Rigidbody>();
        if (!m_rigidbody) Debug.Log("No Rigidbody component attached to this object.");


        EnableGravity(false);
    }

    protected void Update()
    {
        if (m_gravityOn != !TransitionManager.InAR) EnableGravity(!TransitionManager.InAR);

        if (!TransitionManager.InAR)
        {
            if (transform.position.y <= (yOffset - 0.5)) transform.position = resetPosition;
            // walk when the Trigger is used 
            if (touchEventManager.IsHit && !touchEventManager.UIHit)
            {
                if (touchEventManager.objectHit == null || touchEventManager.objectHit.name != "VR_EMS/Logement") // click on no other trigger in the scene
                {
                    m_isWalking = !m_isWalking;
                    Debug.Log("isWalking=" + m_isWalking);
                }
            }

            if (m_isWalking)
            {
                // move in the direction the camera is looking at
                Vector3 direction = new Vector3(m_cameraTransform.forward.x, 0, m_cameraTransform.forward.z).normalized * speed * Time.deltaTime;
                Quaternion rotation = Quaternion.Euler(new Vector3(0, -transform.rotation.eulerAngles.y, 0));
                transform.Translate(rotation * direction);
            }

            if (m_rigidbody) m_rigidbody.velocity = new Vector3(0, 0, 0); // disable collision accelerations

            if (freezeYPosition)
            {
                transform.position = new Vector3(transform.position.x, yOffset, transform.position.z); // fix the y coordinate
            }
        }

    }

    #endregion // MONOBEHAVIOUR_METHODS

    private void EnableGravity(bool value)
    {
        if (m_rigidbody)
        {
            m_rigidbody.useGravity = value;
            m_rigidbody.velocity = new Vector3(0, 0, 0); // disable collision accelerations
        }

        if (value)
        {
            transform.position = resetPosition;
        }
        else
        {
            m_isWalking = false; // you can only walk in VR
        }

        m_gravityOn = value;
    }
}
