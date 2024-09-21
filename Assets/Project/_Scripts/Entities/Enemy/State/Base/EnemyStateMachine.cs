namespace Project._Scripts.Entities.Enemy.State.Base
{
    public class EnemyStateMachine<T> where T : Enemy
    {
        private T _enemy;
        private IEnemyState<T> _currentState;

        public EnemyStateMachine(T enemy, IEnemyState<T> initialState)
        {
            this._enemy = enemy;
            _currentState = initialState;
            _currentState.Enter(enemy);
        }

        public void Update()
        {
            IEnemyState<T> newState = _currentState.CheckTransitions(_enemy);
            if (newState != _currentState)
            {
                TransitionToState(newState);
            }
            _currentState.Execute(_enemy);
        }

        private void TransitionToState(IEnemyState<T> newState)
        {
            _currentState.Exit(_enemy);
            _currentState = newState;
            _currentState.Enter(_enemy);
        }
    }
}