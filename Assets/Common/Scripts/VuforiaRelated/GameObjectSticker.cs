using UnityEngine;

public class GameObjectSticker : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    public Transform targetGameObject;
    public bool rotateWithTarget
    {
        get { return m_rotateWithTarget; }
        set
        {
            m_rotateWithTarget = !value;
        }
    }

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    Vector3 m_fixedUIPos;
    Quaternion m_quat = Quaternion.identity;
    private bool m_rotateWithTarget = true;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        m_fixedUIPos = transform.position - targetGameObject.position;
    }

    protected void Update()
    {
        // POSITION
        if (targetGameObject)
        {
            if (m_rotateWithTarget)
            {
                m_quat = Quaternion.AngleAxis(targetGameObject.rotation.eulerAngles.y, Vector3.up);
            }

            transform.position = targetGameObject.position + m_quat * m_fixedUIPos;
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS

}