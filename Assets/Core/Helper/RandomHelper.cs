using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Core.Helper
{
    public class RandomHelper
    {
        public const float RandomSeedNumber = 3939f;

        /// <summary>
        /// 시작 후 경과 실제 시간으로 랜덤 시드 재설정.
        /// </summary>
        /// <remarks>Unity 랜덤 생성기를 다시 초기화합니다. 게임 재시작 등에서 최신 실행 상태 기반 랜덤을 얻을 때 사용하세요.</remarks>
        public static void ResetRandomSeed()
        {
            int currentTime = (int)(Time.realtimeSinceStartup * RandomSeedNumber);
            Random.InitState(currentTime);
        }

        /// <summary>
        /// 주사위 굴리기(정수).
        /// </summary>
        /// <remarks>최소값과 최대값이 동일하면 해당 값을 반환합니다.</remarks>
        public static int RollDice(int minInclusive, int maxInclusive)
        {
            return Random.Range(minInclusive, maxInclusive + 1);
        }

        /// <summary>
        /// 동전 던지기(앞/뒤) 결과를 반환.
        /// </summary>
        /// <remarks>앞/뒤가 동일한 확률로 나옵니다.</remarks>
        /// <returns>앞이면 <see langword="true"/>, 뒤면 <see langword="false"/>.</returns>
        public static bool HeadsOrTails()
        {
            return Random.Range(0, 2) == 0 ? false : true;
        }

        public static bool IsPassed(float current, float max = 1f)
        {
            float randomValue = Random.Range(0f, max);
            return current > randomValue;
        }

        /// <summary>
        /// 가중치 배열에 따라 인덱스를 선택합니다.
        /// </summary>
        /// <remarks>배열 합이 1.0이 아니면 에러/경고를 로그하고 -1을 반환합니다. 균일 난수로 가중 선택을 수행합니다.</remarks>
        /// <param name="percentArray">각 결과의 확률 가중치(0.0~1.0). 전체 합은 1.0이어야 합니다.</param>
        /// <returns>선택된 인덱스. 입력이 유효하지 않으면 -1.</returns>
        public static int DiceList(float[] percentArray)
        {
            float total = percentArray.Sum();
            if (total > 1.0f)
            {
                Debug.LogError("DiceList: Percent array total exceeds 1.0");
                return -1; // 오류 경우
            }
            else if (total < 1.0f)
            {
                Debug.LogWarning("DiceList: Percent array total is less than 1.0, normalizing.");
                return -1;
            }

            float minPercent = 0f;
            float randomValue = Random.Range(0f, 1f);

            for (int i = 0; i < percentArray.Length; i++)
            {
                float maxPercent = minPercent + percentArray[i];
                if (randomValue >= minPercent && randomValue <= maxPercent)
                {
                    return i;
                }

                minPercent = maxPercent;
            }

            return -1;
        }

        /// <summary>
        /// 값들의 상대 비율로 인덱스를 무작위 선택합니다.
        /// </summary>
        /// <remarks>모든 요소가 0이면 마지막 인덱스를 반환합니다. 입력 배열을 정규화하지 않습니다. Unity의 Random.value 사용.</remarks>
        /// <param name="percentArray">각 인덱스의 상대 확률(비음수). 최소 한 개 요소 필요.</param>
        /// <returns>선택된 인덱스.</returns>
        public static int SelectRandom(float[] percentArray)
        {
            float total = percentArray.Sum();
            float randomValue = Random.value * total;

            for (int i = 0; i < percentArray.Length; i++)
            {
                if (randomValue < percentArray[i])
                {
                    return i;
                }
                else
                {
                    randomValue -= percentArray[i];
                }
            }

            return percentArray.Length - 1; // 다른 인덱스가 선택되지 않으면 마지막 인덱스 반환
        }
    }
}
