using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Work.Utils.Helpers
{
    public static class GameBootstrap
    {
        public static HelperManager HelperManager { get; private set; }

        [RuntimeInitializeOnLoadMethod] // 유니티 생명주기가 아니라 그 이전 씬로드 이전단계 : 유니티와 완전히 별개인 C#고유 기능의 스크립트들만 여기서
        private static void Init()
        {
            HelperManager = new HelperManager();
            HelperManager.Initialize();
        }

        
    }
}
