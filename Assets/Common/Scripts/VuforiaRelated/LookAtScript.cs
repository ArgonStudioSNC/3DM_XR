using UnityEngine;

public class LookAtScript : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    public Transform lookAtTransform = null;
    public bool lookAway = false;
    public bool applyOnChildren = true;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    Quaternion m_lookRotation;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Update()
    {
        if (lookAtTransform)
        {
            Vector3 direction = (lookAway ? transform.position - lookAtTransform.position : lookAtTransform.position - transform.position).normalized;
            m_lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = m_lookRotation;

            if (applyOnChildren)
            {
                foreach (Transform child in GetComponentInChildren<Transform>())
                {
                    direction = (lookAway ? child.position - lookAtTransform.position : lookAtTransform.position - child.position).normalized;
                    m_lookRotation = Quaternion.LookRotation(direction);
                    child.rotation = m_lookRotation;
                }
            }
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS
}