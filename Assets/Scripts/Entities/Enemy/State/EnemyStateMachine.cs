namespace Entities.Enemy.State
{
    public class EnemyStateMachine
    {
        private Enemy _enemy;
        private IEnemyState _currentState;

        public EnemyStateMachine(Enemy enemy, IEnemyState initialState)
        {
            this._enemy = enemy;
            _currentState = initialState;
            _currentState.Enter(enemy);
        }

        public void Update()
        {
            IEnemyState newState = _currentState.CheckTransitions(_enemy);
            if (newState != _currentState)
            {
                TransitionToState(newState);
            }
            _currentState.Execute(_enemy);
        }

        private void TransitionToState(IEnemyState newState)
        {
            _currentState.Exit(_enemy);
            _currentState = newState;
            _currentState.Enter(_enemy);
        }
    }
}