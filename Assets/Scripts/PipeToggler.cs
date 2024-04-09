using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PipeToggler : MonoBehaviour
{
    public GameObject pipesName;

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
            Subscribe(isOn => { TogglePipe(isOn); });

        toggle.OnValueChangedAsObservable().
            Where(isOn => isOn == false).
            Subscribe(isOn => { bigToggle.SetIsOnWithoutNotify(isOn); });

        // 여기다가 다 켜지면 공지없이 켜지게만 하면 되겠다!!
    }
    public void TogglePipe(bool isOn)
    {
        if (pipesName != null && isOn)
            pipesName.SetActive(true);
        else if (pipesName != null && !isOn)
            pipesName.SetActive(false);
    }
}
