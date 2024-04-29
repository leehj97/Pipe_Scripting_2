using UnityEngine;
using Zenject;

public class UnitInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPipeVector3Value>().To<StringToVector3Converter>().AsSingle();

        Container.Bind<IPipeVector3ValueList>().To<CornerPipesVector3ListGetter>().AsSingle();

        Container.Bind<PipesPositionGetter>().AsSingle();
    }
}