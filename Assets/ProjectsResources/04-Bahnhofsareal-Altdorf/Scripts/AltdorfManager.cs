using UnityEngine;

/* <summary>
 * Manager class attached to the AltdorfManager GameObject.
 * Act like a spate machine to hold the variables of the Altdorf project. Variables are project (id).
 * </summary>
 * */
public class AltdorfManager : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES

    public Transform alternativesAR;
    public Transform alternativesVR;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private int m_currentAlternativeID = 0;
    private bool m_vegetationEnabled = true;
    private GameObject[] m_vegetationTag;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        m_vegetationTag = GameObject.FindGameObjectsWithTag("Vegetation");
    }

    protected void Update()
    {
        UpdateAlternatives();
        UpdateVegetation();
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    /*<summary>
     * Switch to the next project in the list.
     * </summary>
     * */
    public void SwitchProject()
    {
        m_currentAlternativeID = (m_currentAlternativeID + 1) % 2;
    }

    public void EnableVegetation(bool value)
    {
        m_vegetationEnabled = value;
    }

    #endregion // PUBLIC_METHODS


    #region PRIVATE_METHODS

    private void UpdateAlternatives()
    {
        foreach (Transform child in alternativesAR) child.gameObject.SetActive(false);
        foreach (Transform child in alternativesVR) child.gameObject.SetActive(false);

        alternativesAR.GetChild(m_currentAlternativeID).gameObject.SetActive(true);
        alternativesVR.GetChild(m_currentAlternativeID).gameObject.SetActive(true);
    }

    private void UpdateVegetation()
    {
        foreach (GameObject go in m_vegetationTag) go.SetActive(m_vegetationEnabled);
    }

    #endregion // PRIVATE_METHODS
}
