using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ToggleModel
{
    public ReactiveProperty<bool> IsOn { get; private set; }

    public ToggleModel(bool isOn)
    {
        IsOn.Value = isOn;
    }
}
