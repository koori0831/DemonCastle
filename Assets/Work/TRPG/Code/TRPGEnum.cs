using UnityEngine;
using System.Collections;

namespace Work.TRPG.Code
{
    public enum CheckResult
    {
        CriticalSuccess, // 대성공
        Success,         // 성공
        Failure,         // 실패
        Fumble           // 대실패
    }
}