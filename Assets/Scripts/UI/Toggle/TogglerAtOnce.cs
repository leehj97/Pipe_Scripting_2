using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using UniRx.Triggers;
using UnityEngine.Purchasing.MiniJSON;

public class TogglerAtOnce : MonoBehaviour
{
    [SerializeField] private List<Toggle> toggles = new();

    private Toggle bigToggle;

    private void Awake()
    {
        bigToggle = gameObject.GetComponent<Toggle>();
    }

    private void Start()
    {
        toggles = gameObject.transform.parent.GetComponentsInChildren<Toggle>().ToList();
        toggles.Remove(bigToggle);

        bigToggle.OnValueChangedAsObservable().
            Subscribe(isOn => { ToggleAll(isOn); });

        foreach (Toggle toggle in toggles)
        {
            toggle.OnValueChangedAsObservable().
            Subscribe(isOn => { CheckAllToggles(); });
        }
    }

    public void ToggleAll(bool isOn)
    {
        foreach (Toggle toggle in toggles)
            toggle.isOn = isOn;
    }

    private void CheckAllToggles()
    {
        if (toggles.All(toggle => toggle.isOn))
            bigToggle.SetIsOnWithoutNotify(true);
    }
}
