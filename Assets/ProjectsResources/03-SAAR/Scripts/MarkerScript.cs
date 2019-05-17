using UnityEngine;
using UnityEngine.EventSystems;

public class MarkerScript : MonoBehaviour, IPointerClickHandler
{
    #region PUBLIC_MEMBER_VARIABLES

    [Tooltip("Reference sur le panneau d'affichage de project (ne pas modifier)")]
    public GameObject projectPanel;
    public string titreProject;
    public string[] nomsEtudiants;
    [TextArea]
    public string descriptifProject;
    public string nomTarget;

    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES

    private string studentNames;

    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS

    protected void Start()
    {
        studentNames = nomsEtudiants[0];
        for (int i = 1; i < nomsEtudiants.Length; i++)
        {
            studentNames += ", " + nomsEtudiants[i];
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    public void OnPointerClick(PointerEventData eventData)
    {
        projectPanel.GetComponent<ProjectPanelScript>().Setup(titreProject, studentNames, descriptifProject, nomTarget);
    }
    #endregion // PUBLIC_METHODS
}
