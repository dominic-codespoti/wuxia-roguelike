namespace Entities.Enemy.State
{
    public interface IEnemyState
    {
        void Enter(Enemy enemy);
        void Execute(Enemy enemy);
        void Exit(Enemy enemy);
        IEnemyState CheckTransitions(Enemy enemy);
    }
}