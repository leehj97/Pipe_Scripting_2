using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class InterfaceInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPipeVector3Value>().To<StringToVector3Converter>().AsSingle();

        Container.Bind<IPipeVector3ValueList>().To<CornerPipesVector3ListGetter>().AsSingle();
        // Container.BindInstance(new CornerPipesVector3ListGetter());

        // Container.Bind<INameClassifier>().To<PipeNameClassifier>().FromComponentInHierarchy().AsSingle();
        // 작동은 하지만 비용이 크다. INameClassifier는 Pipe, Facility 2종류가 있기 때문에 To를 사용하면 안됨.
    }
}