using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PipeNameDivider : MonoBehaviour, INameDivider
{
    private Dictionary<string, Transform> pipeParents = new();

    public void DivideWithName(GameObject pipe, string obstName)
    {
        if (pipeParents.ContainsKey(obstName))
            pipe.transform.SetParent(pipeParents[obstName]);
        else
        {
            GameObject obj = new GameObject(obstName);
            obj.transform.SetParent(transform);
            pipe.transform.SetParent(obj.transform);
            pipeParents.Add(obstName, obj.transform);
        }
    }
}
