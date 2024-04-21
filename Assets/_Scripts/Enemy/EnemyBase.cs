using System;
using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Player;

namespace Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        private static readonly int IdleAnimHash = Animator.StringToHash("Idle_A");
        private static readonly int MoveAnimHash = Animator.StringToHash("Walk");
        private static readonly int AttackAnimHash = Animator.StringToHash("Attack");
        private static readonly int ReceiveDmageAnimHash = Animator.StringToHash("Hit");
        private static readonly int DieAnimHash = Animator.StringToHash("Death");
        private static readonly int RECEIVE_DAMAGE_COOL_TIME = 500;

        private IEnemyState currentState;
        private IEnemyState prevState;
        private Dictionary<EnemyState, IEnemyState> stateDictionary;
        private Animator animator;
        private int hp = 100;
        private bool isReceivingDamage = true;
        private BoxCollider hitBoxCollider;

        private Transform playerTransform = null;
        public Vector3 PlayerPosition => playerTransform.position;

        public void Init(Transform playerTransform)
        {
            this.playerTransform = playerTransform;

            hitBoxCollider = GetComponent<BoxCollider>();
            animator = GetComponent<Animator>();

            CreateStateDictionary();
            ChangeState(EnemyState.Move);
        }

        private void CreateStateDictionary()
        {
            Dictionary<EnemyState, IEnemyState> dic = new()
            {
                // 何もしない
                { EnemyState.Idle, new IdleState(this) },
                // 移動
                { EnemyState.Move, new MoveState(this) },
                // 攻撃
                { EnemyState.Attack, new AttackState(this) },
                // 攻撃を受けた時
                { EnemyState.ReceiveDamage, new ReceiveDamageState(this) },
                // 死亡時
                { EnemyState.Die, new DieingState(this) },
                // 誕生時
                { EnemyState.Spown, new SpownState(this) },
            };
            stateDictionary = dic;
        }

        public void ChangeState(EnemyState nextState)
        {
            IEnemyState newState = stateDictionary[nextState];
            prevState = currentState;
            currentState?.Exit();
            currentState = newState;
            currentState.Init();
        }

        void Update()
        {
            currentState.Update();
        }

        public void StartAnimation()
        {
            switch(currentState.State)
            {
                case EnemyState.Idle:
                    animator.Play(IdleAnimHash);
                    break;

                case EnemyState.Move:
                    animator.Play(MoveAnimHash);
                    break;

                case EnemyState.Attack:
                    animator.Play(AttackAnimHash);
                    break;

                case EnemyState.ReceiveDamage:
                    animator.Play(ReceiveDmageAnimHash);
                    break;

                case EnemyState.Die:
                    animator.Play(DieAnimHash);
                    break;

                default:
                    break;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Attack")
            {
                AttackHitBox hitBox = other.GetComponent<AttackHitBox>();
                ReceiveDmage(hitBox.AttackPow);
            }
        }

        public void ReceiveDmage(int attackPower)
        {
            if (currentState.State == EnemyState.Die)
            {
                return;
            }

            if (!isReceivingDamage)
            {
                return;
            }

            hp -= attackPower;

            if (hp <= 0)
            {
                ChangeState(EnemyState.Die);
            }
            else
            {
                ChangeState(EnemyState.ReceiveDamage);
                ReceivableDamageCoolTimeAsync().Forget();
            }
        }

        private async UniTask ReceivableDamageCoolTimeAsync()
        {
            var token = this.GetCancellationTokenOnDestroy();
            await UniTask.Delay(TimeSpan.FromMilliseconds(RECEIVE_DAMAGE_COOL_TIME), false, PlayerLoopTiming.Update, token);
            isReceivingDamage = false;
        }
    }
}