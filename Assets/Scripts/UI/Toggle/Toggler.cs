using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Toggler : MonoBehaviour
{
    private Toggle toggle;

    private void Awake()
    {
        toggle = gameObject.GetComponent<Toggle>();
    }

    private void Start()
    {
        
    }
}
