using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class RunState : IPlayerState
    {
        private PlayerCharacter main;
        public RunState(PlayerCharacter player) => main = player;

        public PlayerState State => PlayerState.Run;
        public void Init() {}
        public void Update() {}
        public void Exit() {}

    }
}