using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class JumpState : IPlayerState
    {
        private PlayerCharacter main;
        public JumpState(PlayerCharacter player) => main = player;

        public PlayerState State => PlayerState.Jump;
        public void Init() {}
        public void Update() {}
        public void Exit() {}

    }
}