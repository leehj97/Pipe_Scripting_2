using System.Collections.Generic;
using UnityEngine;

public class NameClassifier : MonoBehaviour
{
    private Dictionary<string, Transform> entireParents = new();
    public void ClassifyWithName(GameObject obj, string obstName)
    {
        if (entireParents.ContainsKey(obstName))
            obj.transform.SetParent(entireParents[obstName]);
        else
        {
            GameObject objparent = new GameObject(obstName);
            objparent.transform.SetParent(transform);
            obj.transform.SetParent(objparent.transform);
            entireParents.Add(obstName, objparent.transform);
        }
    }
}
