using System.Collections.Generic;
using UnityEngine;

public class FacilityNameClassifier : MonoBehaviour, INameClassifier
{
    private Dictionary<string, Transform> facilityParents = new();

    public void ClassifyWithName(GameObject facility, string obstName)
    {
        if (facilityParents.ContainsKey(obstName))
            facility.transform.SetParent(facilityParents[obstName]);
        else
        {
            GameObject obj = new GameObject(obstName);
            obj.transform.SetParent(transform);
            facility.transform.SetParent(obj.transform);
            facilityParents.Add(obstName, obj.transform);
        }
    }
}
