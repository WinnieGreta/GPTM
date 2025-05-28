namespace Interfaces.Weapons
{
    public interface IDamageDealer : IDespawnable
    {
        void DoDamage(IDamagable target);
    }
}