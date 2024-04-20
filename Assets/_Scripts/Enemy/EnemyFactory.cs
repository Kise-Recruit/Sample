using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] Transform spownPointTransform;
        [SerializeField] Transform enemysParentTransform;
        [SerializeField] List<EnemysPreset> enemyPresetList = new List<EnemysPreset>();

        private const float GENERATE_COOL_TIME = 2.0f;

        private float generateTimer = 0.0f;
        private Transform playerTransform;
        private Transform[] spownPositionList;

        public void Init(Transform playerTransform)
        {
            this.playerTransform = playerTransform;

            var spownPoints = spownPointTransform.GetComponentsInChildren<Transform>();
            spownPositionList = new Transform[spownPoints.Length];
            spownPositionList = spownPoints;
        }

        void Update()
        {
            generateTimer -= Time.deltaTime;

            if (generateTimer <= 0.0f)
            {
                generateTimer = GENERATE_COOL_TIME;

                if (spownPositionList.Length == 0 || enemyPresetList.Count == 0)
                {
                    Debug.LogWarning("(EnemyFactory) 生成に必要な情報が足りません");
                }
                GenerateEnemyPreset();
            }
        }

        private void GenerateEnemyPreset()
        {
            // 敵集団のプリセットを生成
            int spownRndIndex = Random.Range(0, spownPositionList.Length);
            int enemypresetRndIndex = Random.Range(0, enemyPresetList.Count);
            EnemysPreset newPreset = Instantiate(enemyPresetList[enemypresetRndIndex], spownPositionList[spownRndIndex].position, Quaternion.identity);
            newPreset.SetUpEnemys(playerTransform, enemysParentTransform);
        }
    }
}