using Core.Helper;

namespace Work.TRPG
{
    [System.Serializable]
    public struct CheckInfo
    {
        public int originalDice;   // 원래 주사위 값 (0~99)
        public int finalDice;      // 보정된 주사위 값
        public CheckResult result; // 판정 결과
        public bool isDouble;      // 더블 발생 여부
        public int stat;   // 목표 수치
    }

    public class TRPGDiceSystem
    {
        public TRPGDiceSystem()
        {
            RandomHelper.ResetRandomSeed();
        }

        public CheckInfo RollDice(int stat)
        {
            CheckInfo checkInfo = new CheckInfo();

            checkInfo.originalDice = RandomHelper.RollDice(0, 99);
            checkInfo.finalDice = checkInfo.originalDice;

            checkInfo.isDouble = (checkInfo.originalDice % 11 == 0);

            checkInfo.stat = stat;

            bool isSuccess = (checkInfo.finalDice <= stat);

            if (checkInfo.isDouble)
            {
                if (isSuccess)
                {
                    checkInfo.finalDice -= 10;
                    if (checkInfo.finalDice < 0)
                        checkInfo.finalDice = 0;
                }
                else
                {
                    checkInfo.result = CheckResult.Fumble;
                    return checkInfo;
                }
            }

            if (checkInfo.finalDice <= (stat / 10))
                checkInfo.result = CheckResult.CriticalSuccess;
            else if (checkInfo.finalDice <= stat)
                checkInfo.result = CheckResult.Success;
            else
                checkInfo.result = CheckResult.Failure;

            return checkInfo;
        }
    }
}