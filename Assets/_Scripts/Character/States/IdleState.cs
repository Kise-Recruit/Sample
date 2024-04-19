using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class IdleState : IPlayerState
    {
        private PlayerCharacter main;
        public IdleState(PlayerCharacter player) => main = player;

        public PlayerState State => PlayerState.Idle;
        public void Init() {}
        public void Update() {}
        public void Exit() {}

    }
}
