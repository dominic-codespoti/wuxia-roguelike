using System.Numerics;
using Entities.Combat;
using Entities.Player;

namespace Common.Interfaces
{
    public interface IAttackController
    {
        void Shoot(bool isSecondary, Skill skill);
    }
}