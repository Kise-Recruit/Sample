using System;
using System.Threading;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] Transform spownPointTransform;
        [SerializeField] Transform enemysParentTransform;
        [SerializeField] List<EnemysPreset> enemyPresetList = new List<EnemysPreset>();

        private const float GENERATE_COOL_TIME = 2000.0f;

        private Transform playerTransform;
        private Transform[] spownPositionList;

        public void Init(Transform playerTransform)
        {
            this.playerTransform = playerTransform;

            var spownPoints = spownPointTransform.GetComponentsInChildren<Transform>();
            spownPositionList = new Transform[spownPoints.Length];
            spownPositionList = spownPoints;

            if (spownPositionList.Length == 0 || enemyPresetList.Count == 0)
            {
                Debug.LogWarning("(EnemyFactory) 生成に必要な情報が足りません");
                return;
            }

            var ctn = this.GetCancellationTokenOnDestroy();
            ReceivableDamageCoolTimeAsync(ctn).Forget();
        }

        private async UniTask ReceivableDamageCoolTimeAsync(CancellationToken token)
        {
            while(!token.IsCancellationRequested)
            {
                GenerateEnemyPreset();
                await UniTask.Delay(TimeSpan.FromMilliseconds(GENERATE_COOL_TIME), false, PlayerLoopTiming.Update, token);
            }
        }

        private void GenerateEnemyPreset()
        {
            // 敵集団のプリセットを生成
            int spownRndIndex = UnityEngine.Random.Range(0, spownPositionList.Length);
            int enemypresetRndIndex = UnityEngine.Random.Range(0, enemyPresetList.Count);
            EnemysPreset newPreset = Instantiate(enemyPresetList[enemypresetRndIndex], spownPositionList[spownRndIndex].position, Quaternion.identity);
            newPreset.SetUpEnemys(playerTransform, enemysParentTransform);
        }
    }
}