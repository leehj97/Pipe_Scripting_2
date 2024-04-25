using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ToggleManager : MonoBehaviour
{
    private ToggleModel toggleModel;

    [SerializeField] private GameObject pipeTogglesParent;
    [SerializeField] private GameObject facilityTogglesParent;

    [SerializeField] private List<Toggle> pipeToggles;
    [SerializeField] private List<Toggle> facilityToggles;

    [SerializeField] private Toggle pipeEntireToggle;
    [SerializeField] private Toggle facilityEntireToggle;

    private void Start()
    {
        pipeToggles = pipeTogglesParent.transform.GetComponentsInChildren<Toggle>().ToList();
        pipeToggles.Remove(pipeEntireToggle);

        facilityToggles = facilityTogglesParent.transform.GetComponentsInChildren<Toggle>().ToList();
        facilityToggles.Remove(facilityEntireToggle);

        toggleModel = pipeToggles[0].GetComponent<ToggleModel>();

        ToggleObstAsObservable(pipeToggles);
        ToggleObstAsObservable(facilityToggles);

        SetEntireToggleWithoutNotifyAsObservable(pipeToggles, pipeEntireToggle);
        SetEntireToggleWithoutNotifyAsObservable(facilityToggles, facilityEntireToggle);

        ToggleAllAsObservable(pipeToggles, pipeEntireToggle);
        ToggleAllAsObservable(facilityToggles, facilityEntireToggle);
    }

    private void ToggleObstAsObservable(List<Toggle> toggles)
    {
        foreach (Toggle toggle in toggles)
        {
            toggle.OnValueChangedAsObservable().
                Subscribe(isOn => { toggleModel.ToggleObst(isOn); });
        }
    }

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