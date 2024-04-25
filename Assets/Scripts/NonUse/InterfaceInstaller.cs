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
    }
}