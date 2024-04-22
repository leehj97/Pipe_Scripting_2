using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ToggleModel
{
    public static ToggleModel Instance;

    public GameObject obstName;

    private void Awake()
    {
        Instance = this;
    }

    private void ToggleObst(bool isOn)
    {
        if (obstName != null && isOn)
            obstName.SetActive(true);
        else if (obstName != null && !isOn)
            obstName.SetActive(false);
    }
}
