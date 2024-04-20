using System;
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
        private static readonly int AttackAnimHash = Animator.StringToHash("WAIT04");
        private static readonly int ReceiveDmageAnimHash = Animator.StringToHash("DAMAGED00");
        private static readonly int DieAnimHash = Animator.StringToHash("DAMAGED01");
        private static readonly int RECEIVE_DAMAGE_COOL_TIME = 500;

        private bool IsJumpable => currentState.State == PlayerState.Idle || currentState.State == PlayerState.Move;
        private bool IsMovable => currentState.State == PlayerState.Idle;

        private IPlayerState currentState;
        private IPlayerState prevState;
        private Dictionary<PlayerState, IPlayerState> stateDictionary;
        private Animator animator;
        private int hp = 100;
        private bool isReceivingDamage = true;
        private PlayerInput playerInput;

        public void Init()
        {
            playerInput = GetComponent<PlayerInput>();
            animator = GetComponent<Animator>();

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
                { PlayerState.Move, new MoveState(this, playerInput, () => ChangeState(PlayerState.Idle)) },
                // 回避
                { PlayerState.Avoid, new AvoidState(this) },
                // ジャンプ
                { PlayerState.Jump, new JumpState(this) },
                // 攻撃
                { PlayerState.Attack, new AttackState(this) },
                // 必殺
                { PlayerState.Ultimate, new UltimateState(this) },
                // 攻撃を受けた時
                { PlayerState.ReceiveDamage, new ReceiveDamageState(this) },
                // 死亡時
                { PlayerState.Die, new DieingState(this) },
            };
            stateDictionary = dic;
        }

        private void ChangeState(PlayerState nextState)
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

        public void OnMove(InputAction.CallbackContext context)
        {
            if (IsMovable)
            {
                ChangeState(PlayerState.Move);
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (IsJumpable && context.phase == InputActionPhase.Started)
            {
                ChangeState(PlayerState.Jump);
            }
        }

        public void StartAnimation()
        {
            switch(currentState.State)
            {
                case PlayerState.Idle:
                    animator.Play(IdleAnimHash);
                    break;

                case PlayerState.Move:
                    animator.Play(MoveAnimHash);
                    break;

                case PlayerState.Jump:
                    animator.Play(JumpAnimHash);
                    break;

                case PlayerState.Attack:
                    animator.Play(AttackAnimHash);
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
                var ctn = this.GetCancellationTokenOnDestroy();
                ReceivableDamageCoolTimeAsync(ctn).Forget();
            }
        }

        private async UniTask ReceivableDamageCoolTimeAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(RECEIVE_DAMAGE_COOL_TIME), false, PlayerLoopTiming.Update, token);
            isReceivingDamage = false;
        }
    }
}