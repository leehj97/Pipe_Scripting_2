using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnitInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Container.Bind<Foo>().FromFactory<FooFactory>();
    }
}

public class Foo
{
    public class Pool : MemoryPool<Foo>
    {
    }
}
public class Bar
{
    readonly Foo.Pool fooPool;
    readonly List<Foo> foos = new List<Foo>();

    public Bar(Foo.Pool _fooPool)
    {
        fooPool = _fooPool;
    }

    public void AddFoo()
    {
        foos.Add(fooPool.Spawn());
    }

    public void RemoveFoo()
    {
        var foo = foos[0];
        fooPool.Despawn(foo);
        foos.Remove(foo);
    }
}
public class TestInstaller : MonoInstaller<TestInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<Bar>().AsSingle();
        Container.BindMemoryPool<Foo, Foo.Pool>();
    }
}