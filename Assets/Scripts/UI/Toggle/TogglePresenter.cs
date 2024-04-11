using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class TogglePresenter : MonoBehaviour
{
    [SerializeField] private ToggleView toggleView;

    private ToggleModel toggleModel;

    private void Start()
    {
        toggleModel = new ToggleModel(toggleView);

        toggleView.OnValueChanged
            .Subscribe(isOn => toggleModel.IsOn.Value = isOn)
            .AddTo(this);

        toggleModel.IsOn
            .Subscribe(isOn => toggleView.SetToggle(isOn))
            .AddTo(this);
    }
}
