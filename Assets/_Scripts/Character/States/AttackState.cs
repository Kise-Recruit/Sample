using System;
using UnityEngine;

namespace Player
{
    public class AttackState : IPlayerState
    {
        private const float ATTACK_START_TIME = 0.15f;
        private const float ATTACK_END_TIME = 0.5f;

        private PlayerCharacter main;
        public PlayerState State => PlayerState.Attack;

        private Action onStartAttack;
        private Action onEndAttack;
        private bool isLastFrameAttaking = false;

        public AttackState(PlayerCharacter player, Action onStartAttack, Action onEndAttack)
        {
            main = player;
            this.onStartAttack = onStartAttack;
            this.onEndAttack = onEndAttack;
        }

        public void Init() 
        {
            main.StartAnimation(() => main.ChangeState(PlayerState.Idle));
            isLastFrameAttaking = false;
        }

        public void Update() 
        {
            bool isAttaking = main.GetAnimationPlayTime > ATTACK_START_TIME && main.GetAnimationPlayTime <= ATTACK_END_TIME;

            if (!isLastFrameAttaking && isAttaking)
            {
                onStartAttack();
            }

            if (isLastFrameAttaking && !isAttaking)
            {
                onEndAttack();
            }

            if (!isAttaking)
            {
                if (main.MoveInput.phase == UnityEngine.InputSystem.InputActionPhase.Started)
                {
                    main.ChangeState(PlayerState.Move);
                }
            }

            isLastFrameAttaking = isAttaking;
        }

        public void Exit() 
        {
            onEndAttack();
        }
    }
}