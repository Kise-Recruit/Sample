namespace Enemy
{
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        ReceiveDamage,
        Die,
        Spown
    }

    public interface IEnemyState
    {
        EnemyState State { get; }
        public void Init() {}
        public void Update() {}
        public void Exit() {}
    }
}