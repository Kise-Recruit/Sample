using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class ReceiveDamageState : IPlayerState
    {
        private PlayerCharacter main;
        public ReceiveDamageState(PlayerCharacter player) => main = player;

        public PlayerState State => PlayerState.ReceiveDamage;
        public void Init() {}
        public void Update() {}
        public void Exit() {}

    }
}