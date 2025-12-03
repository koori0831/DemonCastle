using UnityEngine;
using System.Collections;

namespace Work.TRPG.Code
{
    // 주사위 판정 결과
    public enum CheckResult
    {
        CriticalSuccess, // 대성공
        Success,         // 성공
        Failure,         // 실패
        Fumble           // 대실패
    }
}