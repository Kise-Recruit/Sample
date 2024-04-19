using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class UltimateState : IPlayerState
    {
        private PlayerCharacter main;
        public UltimateState(PlayerCharacter player) => main = player;

        public PlayerState State => PlayerState.Ultimate;
        public void Init() {}
        public void Update() {}
        public void Exit() {}

    }
}