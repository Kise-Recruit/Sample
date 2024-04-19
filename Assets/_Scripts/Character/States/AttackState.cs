using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class AttackState : IPlayerState
    {
        private PlayerCharacter main;
        public AttackState(PlayerCharacter player) => main = player;

        public PlayerState State => PlayerState.Attack;
        public void Init() {}
        public void Update() {}
        public void Exit() {}

    }
}