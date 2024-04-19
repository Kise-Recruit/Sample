using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class AvoidState : IPlayerState
    {
        private PlayerCharacter main;
        public AvoidState(PlayerCharacter player) => main = player;

        public PlayerState State => PlayerState.Avoid;
        public void Init() {}
        public void Update() {}
        public void Exit() {}

    }
}