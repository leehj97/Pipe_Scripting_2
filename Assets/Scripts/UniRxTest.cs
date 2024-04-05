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
        // ToYieldInstruction()은 OnCompleted가 발행되어 코루틴 종료
        // 따라서 OnCompleted가 반드시 발행되는 스트림에서만 사용할 수 있다.
        Debug.Log("Press any key");
        yield return this.UpdateAsObservable()
            .FirstOrDefault(_ => Input.anyKeyDown)
            .ToYieldInstruction();
        // FirstOrDefault 조건을 충족하면 OnNext와 OnCompleted를 모두 발행한다.
        Debug.Log("Pressed");
    }
}