using System;

namespace Player
{
    public class UltimateState : IPlayerState
    {
        private PlayerCharacter main;
        private Action onStartAttack;
        private Action onEndAttack;

        public PlayerState State => PlayerState.Ultimate;

        public UltimateState(PlayerCharacter player, Action onStartAttack, Action onEndAttack)
        {
            main = player;
            this.onStartAttack = onStartAttack;
            this.onEndAttack = onEndAttack;
        }

        public void Init() {}
        public void Update() {}
        public void Exit() 
        {
            onEndAttack();
        }

    }
}