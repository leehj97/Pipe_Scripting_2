using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PipeNameCollection : MonoBehaviour
{
    public void DividePipesWithName(GameObject pipe, string obstName)
    {
        Transform pipeParent = transform.Find($"{obstName}");
        if (pipeParent != null)
            pipe.transform.SetParent(pipeParent);
        else
        {
            GameObject obj = new GameObject($"{obstName}");
            obj.transform.SetParent(transform);
            pipe.transform.SetParent(obj.transform);
        }
    }
}
