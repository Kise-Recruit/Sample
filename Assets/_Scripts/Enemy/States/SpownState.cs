using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class SpownState : IEnemyState
    {
        private EnemyBase main;
        public SpownState(EnemyBase enemy) => main = enemy;

        public EnemyState State => EnemyState.Spown;
        public void Init() {}
        public void Update() {}
        public void Exit() {}
    }
}