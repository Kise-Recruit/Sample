using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class DieingState : IEnemyState
    {
        private EnemyBase main;
        public DieingState(EnemyBase enemy) => main = enemy;

        public EnemyState State => EnemyState.Die;
        public void Init() 
        {
            main.StartAnimation();
        }

        public void Update() {}
        public void Exit() {}
    }
}