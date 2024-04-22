using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Toggler : MonoBehaviour
{
    public GameObject obstName;

    private Toggle toggle;

    private void Awake()
    {
        toggle = gameObject.GetComponent<Toggle>();
    }

    private void Start()
    {
        toggle.OnValueChangedAsObservable().
            Subscribe(isOn => { ToggleObst(isOn); });
    }

    private void ToggleObst(bool isOn)
    {
        if (obstName != null && isOn)
            obstName.SetActive(true);
        else if (obstName != null && !isOn)
            obstName.SetActive(false);
    }
}
