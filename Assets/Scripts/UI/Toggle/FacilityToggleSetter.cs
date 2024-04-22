using UnityEngine;
using UnityEngine.UI;

public class FacilityToggleSetter : MonoBehaviour
{
    public GameObject facilities;
    public GameObject togglePrefab;
    public GameObject toggleParent;

    private void Start()
    {
        int facilitiesType = facilities.transform.childCount;

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
    }
}
