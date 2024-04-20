using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class MoveState : IPlayerState
    {
        private const float MOVE_SPEED = 10.0f;
        private const float ROTATE_TIME = 0.2f;

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

            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
 
            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 targetMoveVec = cameraForward * inputValue.y + Camera.main.transform.right * inputValue.x;

            main.transform.position += targetMoveVec.normalized * Time.deltaTime;
            main.transform.forward = Vector3.Slerp(main.transform.forward, targetMoveVec, ROTATE_TIME);
        }

        public void Exit() {}
    }
}