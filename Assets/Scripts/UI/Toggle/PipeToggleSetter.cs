using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PipeToggleSetter : MonoBehaviour
{
    public GameObject pipes;

    [SerializeField] private GameObject togglePrefab;
    [SerializeField] private GameObject toggleParent;

    private RectTransform togglePos;
    private Vector3 toggleOffset = new Vector3(0, -50, 0);

    private void Start()
    {
        int pipesType = pipes.transform.childCount;
        toggleParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, pipesType * 31f);

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
        toggle.GetComponent<Toggler>().obstName = pipes.transform.GetChild(index).gameObject;

        togglePos = toggle.GetComponent<RectTransform>();
        togglePos.position += toggleOffset + new Vector3 (0, -30 * index, 0);
    }
}
