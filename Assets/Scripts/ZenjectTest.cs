using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ZenjectTest : MonoBehaviour
{
    public class NameClassifier
    {
        INameClassifier _nameClassifier;

        [Inject]
        public void Init(INameClassifier nameClassifier)
        {
            _nameClassifier = nameClassifier;
        }
    }
}