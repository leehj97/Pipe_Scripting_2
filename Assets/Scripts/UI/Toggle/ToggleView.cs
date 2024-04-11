using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ToggleView : MonoBehaviour
{
    public IObservable<bool> OnValueChanged => toggle.OnValueChangedAsObservable();

    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    public void SetToggle(bool isOn)
    {
        toggle.isOn = isOn;
    }
}
