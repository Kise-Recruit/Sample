using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class MoveState : IPlayerState
    {
        private PlayerCharacter main;
        private InputAction moveInput;
        private Action changeIdleState;

        public PlayerState State => PlayerState.Move;

        public MoveState(PlayerCharacter player, PlayerInput playerInput, Action changeIdleState)
        {
            main = player;
            moveInput = playerInput.actions["Move"];
            this.changeIdleState = changeIdleState;
        }

        public void Init() 
        {
            main.StartAnimation();
        }

        public void Update()
        {
            var inputValue = moveInput.ReadValue<Vector2>();
    
            if (inputValue == Vector2.zero)
            {
                changeIdleState();
            }

            Vector3 moveDirection = new Vector3(inputValue.x, 0.0f, inputValue.y);
            moveDirection.Normalize();

            main.transform.position += moveDirection * Time.deltaTime;
        }

        public void Exit() {}
    }
}