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
    private void Start() =>
        Observable.WhenAll(
            Observable.FromCoroutine<string>(o => CoroutineA(o))
            , Observable.FromCoroutine<string>(o => CoroutineB(o))
        ).Subscribe(xs =>
        {
            foreach (var x in xs)
            {
                Debug.Log("result: " + x);
            }
        });
    private IEnumerator CoroutineA(IObserver<string> observer)
    {
        Debug.Log("CoroutineA 시작");
        yield return new WaitForSeconds(3);
        observer.OnNext("CoroutineA 종료");
        observer.OnCompleted();
    }
    private IEnumerator CoroutineB(IObserver<string> observer)
    {
        Debug.Log("CoroutineB 시작");
        yield return new WaitForSeconds(1);
        observer.OnNext("CoroutineB 종료");
        observer.OnCompleted();
    }
}