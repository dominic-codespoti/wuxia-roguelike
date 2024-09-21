namespace Project._Scripts.Entities.Enemy.State.Base
{
    public interface IEnemyState<T> where T : Enemy
    {
        void Enter(T enemy);
        void Execute(T enemy);
        void Exit(T enemy);
        IEnemyState<T> CheckTransitions(T enemy);
    }
}