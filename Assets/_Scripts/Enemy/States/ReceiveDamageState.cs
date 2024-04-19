using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class ReceiveDamageState : IEnemyState
    {
        private EnemyBase main;
        public ReceiveDamageState(EnemyBase enemy) => main = enemy;

        public EnemyState State => EnemyState.ReceiveDamage;
        public void Init() 
        {
            main.StartAnimation();
        }

        public void Update() {}
        public void Exit() {}
    }
}