using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UniRxTest : MonoBehaviour
{
    private void Start() => StartCoroutine(WaitCoroutine());

    private IEnumerator WaitCoroutine()
    {
        Debug.Log("Wait for 1 second.");
        yield return Observable.Timer(TimeSpan.FromSeconds(1)).ToYieldInstruction();
        // ToYieldInstruction()�� OnCompleted�� ����Ǿ� �ڷ�ƾ ����
        // ���� OnCompleted�� �ݵ�� ����Ǵ� ��Ʈ�������� ����� �� �ִ�.
        Debug.Log("Press any key");
        yield return this.UpdateAsObservable()
            .FirstOrDefault(_ => Input.anyKeyDown)
            .ToYieldInstruction();
        // FirstOrDefault ������ �����ϸ� OnNext�� OnCompleted�� ��� �����Ѵ�.
        Debug.Log("Pressed");
    }
}