using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using UniRx.Triggers;

public class PipeTogglerAtOnce : MonoBehaviour
{
    private Toggle bigToggle;
    [SerializeField] private List<Toggle> toggles = new();
    // [SerializeField] private ReactiveCollection<Toggle> toggles = new();
    // [SerializeField] private ReactiveCollection<bool> togglesIsOn = new();

    private void Awake()
    {
        bigToggle = gameObject.GetComponent<Toggle>();
    }

    private void Start()
    {
        toggles = gameObject.transform.parent.GetComponentsInChildren<Toggle>().ToList();
        // toggles = gameObject.transform.parent.GetComponentsInChildren<Toggle>().ToReactiveCollection();
        toggles.Remove(bigToggle);

        bigToggle.OnValueChangedAsObservable().
            Subscribe(isOn => { ToggleAllPipes(isOn); });
    }

    public void ToggleAllPipes(bool isOn)
    {
        foreach (Toggle toggle in toggles)
            toggle.isOn = isOn;
    }
}
