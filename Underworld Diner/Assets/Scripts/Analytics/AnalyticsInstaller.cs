using Analytics.Signals;
using Zenject;

namespace Analytics
{
    public class AnalyticsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<AnalyticsLevelStartEvent>();
            
            Container.BindInterfacesAndSelfTo<AppAnalytiscService>().AsSingle();

            Container.BindSignal<AnalyticsLevelStartEvent>()
                .ToMethod<AppAnalytiscService>(x => x.OnLevelStart)
                .FromResolve();
        }
    }
}