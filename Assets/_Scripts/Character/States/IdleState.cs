using System;
using System.Diagnostics;
using UnityEngine;
namespace Player
{
    public class IdleState : IPlayerState
    {
        private PlayerCharacter main;

        public IdleState(PlayerCharacter player)
        {
            main = player;
        }

        public PlayerState State => PlayerState.Idle;
        public void Init() 
        {
            main.StartAnimation();
        }

        public void Update() 
        {
            if (main.MoveInput.phase == UnityEngine.InputSystem.InputActionPhase.Started)
            {
                main.ChangeState(PlayerState.Move);
            }

            if (main.AttackInput.phase == UnityEngine.InputSystem.InputActionPhase.Performed)
            {
                main.ChangeState(PlayerState.Attack);
            }
        }

        public void Exit() {}

    }
}
