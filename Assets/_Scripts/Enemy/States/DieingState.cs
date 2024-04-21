using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        public void Update() 
        {
            if (main.GetAnimationPlayTime >= 0.9f)
            {
                main.DieFinish();                
            }
        }

        public void Exit() {}
    }
}