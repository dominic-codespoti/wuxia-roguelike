using Project._Scripts.Entities.Combat;

namespace Project._Scripts.Common.Interfaces
{
    public interface IAttackController
    {
        void Shoot(bool isSecondary, Skill skill);
    }
}