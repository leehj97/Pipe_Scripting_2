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
    public GameObject pipesDepth;

    [SerializeField] private GameObject pipesName;

    private Toggle toggle;
    private string toggleName;

    private void Awake()
    {
        toggle = gameObject.GetComponent<Toggle>();
        toggleName = toggle.transform.name;
    }
    private void Start()
    {
        try { pipesName = pipesDepth.transform.Find($"{toggleName}").gameObject; }
        catch { }

        toggle.OnValueChangedAsObservable().Subscribe(
            isOn => { TogglePipe(isOn); });
    }
    public void TogglePipe(bool isOn)
    {
        if (pipesName != null && isOn)
            pipesName.SetActive(true);
        else if (pipesName != null && !isOn)
            pipesName.SetActive(false);
    }
}
