using UnityEngine;
using UnityEngine.UI;

public class PipeToggleSetter : MonoBehaviour
{
    public GameObject pipes;
    public GameObject togglePrefab;
    public GameObject toggleParent;

    private void Start()
    {
        int pipesType = pipes.transform.childCount;

        for (int i = 0; i < pipesType; i++)
        {
            InstantiateToggle(i);
        }
    }

    private void InstantiateToggle(int index)
    {
        GameObject toggle = Instantiate(togglePrefab, toggleParent.transform);
        toggle.transform.name = pipes.transform.GetChild(index).name;

        toggle.GetComponentInChildren<Text>().text = pipes.transform.GetChild(index).name;
        toggle.GetComponent<ToggleModel>().obstName = pipes.transform.GetChild(index).gameObject;
    }
}
