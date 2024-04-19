using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class DieingState : IPlayerState
    {
        private PlayerCharacter main;
        public DieingState(PlayerCharacter player) => main = player;

        public PlayerState State => PlayerState.Die;
        public void Init() {}
        public void Update() {}
        public void Exit() {}

    }
}