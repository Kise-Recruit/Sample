using System;
using UnityEngine;

namespace Player
{
    public class AttackState : IPlayerState
    {
        private PlayerCharacter main;
        public PlayerState State => PlayerState.Attack;

        private Action onStartAttack;
        private Action onEndAttack;

        public AttackState(PlayerCharacter player, Action onStartAttack, Action onEndAttack)
        {
            main = player;
            this.onStartAttack = onStartAttack;
            this.onEndAttack = onEndAttack;
        }

        public void Init() 
        {
            main.StartAnimation(() => main.ChangeState(PlayerState.Idle));
            onStartAttack();
        }

        public void Update() 
        {
        }

        public void Exit() 
        {
            onEndAttack();
        }
    }
}