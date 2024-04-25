using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleModel : MonoBehaviour
{
    public GameObject obstName;
    public void ToggleObst(bool isOn)
    {
        if (obstName != null && isOn)
            obstName.SetActive(true);
        else if (obstName != null && !isOn)
            obstName.SetActive(false);
    }
}
