using System.Collections.Generic;
using UnityEngine;

public class PipeNameClassifier : MonoBehaviour, INameClassifier
{
    private Dictionary<string, Transform> pipeParents = new();

    public void ClassifyWithName(GameObject pipe, string obstName)
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
