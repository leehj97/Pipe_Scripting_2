using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Toggler : MonoBehaviour
{
    public GameObject obstName;

    private Toggle toggle;
    private Toggle bigToggle;

    private void Awake()
    {
        toggle = gameObject.GetComponent<Toggle>();
        bigToggle = gameObject.transform.parent.GetChild(0).GetComponent<Toggle>();
    }

    private void Start()
    {
        toggle.OnValueChangedAsObservable().
            Subscribe(isOn => { ToggleObst(isOn); });

        toggle.OnValueChangedAsObservable().
            Where(isOn => isOn == false).
            Subscribe(isOn => { bigToggle.SetIsOnWithoutNotify(isOn); });
    }

    private void ToggleObst(bool isOn)
    {
        if (obstName != null && isOn)
            obstName.SetActive(true);
        else if (obstName != null && !isOn)
            obstName.SetActive(false);
    }
}
