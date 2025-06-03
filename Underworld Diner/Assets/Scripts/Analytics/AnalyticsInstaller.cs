using Analytics.Signals;
using Zenject;

namespace Analytics
{
    public class AnalyticsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<AnalyticsLevelStartEvent>();
            Container.DeclareSignal<AnalyticsLevelEndEvent>();
            
            Container.BindInterfacesAndSelfTo<AppAnalytiscService>().AsSingle();

            Container.BindSignal<AnalyticsLevelStartEvent>()
                .ToMethod<AppAnalytiscService>(x => x.OnLevelStart)
                .FromResolve();
            Container.BindSignal<AnalyticsLevelEndEvent>()
                .ToMethod<AppAnalytiscService>(x => x.OnLevelEnd)
                .FromResolve();
        }
    }
}