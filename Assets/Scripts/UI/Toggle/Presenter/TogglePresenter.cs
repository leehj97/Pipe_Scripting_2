using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class TogglePresenter : MonoBehaviour
{
    public GameObject obstName;

    public ToggleView toggleview;

    private void Start()
    {
        toggleview = GetComponent<ToggleView>();

        List<Toggle> toggles = new();

        toggleview.OnToggleValueChangedAsObservable().
            Subscribe(isOn => { ToggleObst(isOn); });

        /*
        toggleview.OnBigtoggleValueChangedAsObservable().
            Subscribe(isOn => { ToggleAll(isOn); });
        */

        /*
        toggle.OnValueChangedAsObservable().
            Where(isOn => isOn == false).
            Subscribe(isOn => { bigToggle.SetIsOnWithoutNotify(isOn); });
        */
    }

    public void ToggleObst(bool isOn)
    {
        if (obstName != null && isOn)
            obstName.SetActive(true);
        else if (obstName != null && !isOn)
            obstName.SetActive(false);
    }
}
