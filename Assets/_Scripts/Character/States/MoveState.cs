using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class MoveState : IPlayerState
    {
        private PlayerCharacter main;
        public MoveState(PlayerCharacter player) => main = player;

        public PlayerState State => PlayerState.Move;
        public void Init() {}
        public void Update() {}
        public void Exit() {}

    }
}