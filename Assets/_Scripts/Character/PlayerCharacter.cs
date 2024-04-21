using System;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCharacter : MonoBehaviour
    {
        private static readonly int IdleAnimHash = Animator.StringToHash("Idle");
        private static readonly int MoveAnimHash = Animator.StringToHash("Move");
        private static readonly int JumpAnimHash = Animator.StringToHash("Jump");
        private static readonly int AttackAnimHash = Animator.StringToHash("Attack");
        private static readonly int ReceiveDmageAnimHash = Animator.StringToHash("DAMAGED00");
        private static readonly int DieAnimHash = Animator.StringToHash("DAMAGED01");
        private static readonly int RECEIVE_DAMAGE_COOL_TIME = 500;
        private static readonly int NORMAL_ATTACK_POW = 100;
        private static readonly int ULTIMATE_ATTACK_POW = 100;

        private InputAction moveInput;
        public InputAction MoveInput => moveInput;
        private InputAction attackInput;
        public InputAction AttackInput => attackInput;

        [SerializeField] AttackHitBox normalAttackHitBox;
        [SerializeField] AttackHitBox ultimateAttackHitBox;
        private IPlayerState currentState;
        private IPlayerState prevState;
        private Dictionary<PlayerState, IPlayerState> stateDictionary;
        private Animator animator;
        private int hp = 100;
        private bool isReceivingDamage = true;
        private PlayerInput playerInput;

        public float GetAnimationPlayTime => animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        public void Init()
        {
            animator = GetComponent<Animator>();

            playerInput = GetComponent<PlayerInput>();
            moveInput = playerInput.actions["Move"];
            attackInput = playerInput.actions["Attack"];

            normalAttackHitBox.Init(NORMAL_ATTACK_POW);
            // ultimateAttackHitBox.Init(ULTIMATE_ATTACK_POW);

            CreateStateDictionary();
            ChangeState(PlayerState.Idle);
        }

        private void CreateStateDictionary()
        {
            Dictionary<PlayerState, IPlayerState> dic = new()
            {
                // 何もしない
                { PlayerState.Idle, new IdleState(this) },
                // 移動
                { PlayerState.Move, new MoveState(this) },
                // 回避
                { PlayerState.Avoid, new AvoidState(this) },
                // ジャンプ
                { PlayerState.Jump, new JumpState(this) },
                // 攻撃
                { PlayerState.Attack, new AttackState(this, () => normalAttackHitBox.OnAttackStart(), () => normalAttackHitBox.OnAttackEnd()) },
                // 必殺
                { PlayerState.Ultimate, new UltimateState(this) },
                // 攻撃を受けた時
                { PlayerState.ReceiveDamage, new ReceiveDamageState(this) },
                // 死亡時
                { PlayerState.Die, new DieingState(this) },
            };
            stateDictionary = dic;
        }

        public void ChangeState(PlayerState nextState)
        {
            IPlayerState newState = stateDictionary[nextState];
            prevState = currentState;
            currentState?.Exit();
            currentState = newState;
            currentState.Init();
        }

        void Update()
        {
            currentState.Update();
        }

        public void StartAnimation(Action onEndAnimation = null)
        {
            int playAnimHash = 0;
            switch(currentState.State)
            {
                case PlayerState.Idle:
                    playAnimHash = IdleAnimHash;
                    break;

                case PlayerState.Move:
                    playAnimHash = MoveAnimHash;
                    break;

                case PlayerState.Jump:
                    playAnimHash = JumpAnimHash;
                    break;

                case PlayerState.Attack:
                    playAnimHash = AttackAnimHash;
                    break;

                case PlayerState.ReceiveDamage:
                    animator.Play(ReceiveDmageAnimHash);
                    break;

                case PlayerState.Die:
                    animator.Play(DieAnimHash);
                    break;

                default:
                    break;
            }

            animator.Play(playAnimHash);
            animator.Update(0);

            if (onEndAnimation != null)
            {
                AnimationPlayFlow(onEndAnimation).Forget();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Enemy"))
            {
                ReceiveDmage(5);
            }
        }

        public void ReceiveDmage(int attackPower)
        {
            if (currentState.State == PlayerState.Die)
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
                ChangeState(PlayerState.Die);
            }
            else
            {
                ReceivableDamageCoolTimeAsync().Forget();
            }
        }

        private async UniTask ReceivableDamageCoolTimeAsync()
        {
            var token = this.GetCancellationTokenOnDestroy();
            await UniTask.Delay(TimeSpan.FromMilliseconds(RECEIVE_DAMAGE_COOL_TIME), false, PlayerLoopTiming.Update, token);
            isReceivingDamage = false;
        }

        private async UniTask AnimationPlayFlow(Action onEndAnimation = null)
        {
            var token = this.GetCancellationTokenOnDestroy();

            await UniTask.Yield();
            if (token.IsCancellationRequested)
            {
                return;
            }

            var currentAnimInfo = animator.GetCurrentAnimatorStateInfo(0);
            await UniTask.Delay(TimeSpan.FromMilliseconds(currentAnimInfo.length * 1000.0f), false, PlayerLoopTiming.Update, token);

            if (currentAnimInfo.tagHash == animator.GetCurrentAnimatorStateInfo(0).tagHash)
            {
                onEndAnimation?.Invoke();
            }
        }
    }
}