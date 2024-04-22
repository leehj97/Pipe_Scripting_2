using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class ToggleView : MonoBehaviour
{
    [SerializeField] private List<Toggle> pipeToggles;
    [SerializeField] private List<Toggle> facilityToggles;

    [SerializeField] private Toggle pipeEntireToggle;
    [SerializeField] private Toggle facilityEntireToggle;

    private void SetEntireToggleWithoutNotifyAsObservable(List<Toggle> toggles, Toggle entireToggle)
    {
        foreach (Toggle toggle in toggles)
        {
            toggle.OnValueChangedAsObservable().Where(isOn => isOn == false).Subscribe(isOn =>
            {
                entireToggle.SetIsOnWithoutNotify(isOn);
            });

            toggle.OnValueChangedAsObservable().Subscribe(isOn => { CheckAllToggles(toggles, entireToggle); });
        }
    }

    private void ToggleAllAsObservable(List<Toggle> toggles, Toggle entireToggle)
    {
        entireToggle.OnValueChangedAsObservable().Subscribe(isOn => { ToggleAll(isOn, toggles); });
    }

    private void CheckAllToggles(List<Toggle> toggles, Toggle entireToggle)
    {
        if (toggles.All(toggle => toggle.isOn))
            entireToggle.SetIsOnWithoutNotify(true);
    }

    private void ToggleAll(bool isOn, List<Toggle> toggles)
    {
        foreach (Toggle toggle in toggles)
            toggle.isOn = isOn;
    }
}