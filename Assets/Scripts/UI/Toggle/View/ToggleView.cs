using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ToggleView : MonoBehaviour
{
    public GameObject obstName;

    private Toggle toggle;
    private Toggle bigToggle;

    private void Awake()
    {
        toggle = gameObject.GetComponent<Toggle>();
        bigToggle = gameObject.transform.parent.GetChild(0).GetComponent<Toggle>();
    }

    public IObservable<bool> OnToggleValueChangedAsObservable()
    {
        return toggle.OnValueChangedAsObservable();
    }

    public IObservable<bool> OnBigtoggleValueChangedAsObservable()
    {
        return bigToggle.OnValueChangedAsObservable();
    }

    private void Start()
    {
        toggle.OnValueChangedAsObservable().
            Subscribe(isOn => { ToggleObst(isOn); });

        toggle.OnValueChangedAsObservable().
            Where(isOn => isOn == false).
            Subscribe(isOn => { bigToggle.SetIsOnWithoutNotify(isOn); });
    }

    public void ToggleObst(bool isOn)
    {
        if (obstName != null && isOn)
            obstName.SetActive(true);
        else if (obstName != null && !isOn)
            obstName.SetActive(false);
    }
}
