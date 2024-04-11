using UnityEngine;
using UnityEngine.UI;

public class FacilityToggleSetter : MonoBehaviour
{
    public GameObject facilities;

    [SerializeField] private GameObject togglePrefab;
    [SerializeField] private GameObject toggleParent;

    private RectTransform togglePos;
    private Vector3 toggleOffset = new Vector3(0, -50, 0);

    private void Start()
    {
        int facilitiesType = facilities.transform.childCount;
        toggleParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, facilitiesType * 31f);

        for (int i = 0; i < facilitiesType; i++)
        {
            InstantiateToggle(i);
        }
    }

    private void InstantiateToggle(int index)
    {
        GameObject toggle = Instantiate(togglePrefab, toggleParent.transform);
        toggle.transform.name = facilities.transform.GetChild(index).name;

        toggle.GetComponentInChildren<Text>().text = facilities.transform.GetChild(index).name;
        toggle.GetComponent<Toggler>().obstName = facilities.transform.GetChild(index).gameObject;

        togglePos = toggle.GetComponent<RectTransform>();
        togglePos.position += toggleOffset + new Vector3(0, -30 * index, 0);
    }
}
