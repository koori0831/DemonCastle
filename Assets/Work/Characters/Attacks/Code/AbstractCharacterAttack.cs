using System;
using System.Collections.Generic;
using UnityEngine;
using Work.Characters.Code;
using Work.Entities;

namespace Work.Characters.Attacks.Code
{
    public class AbstractCharacterAttack
    {//여기서 Attack에 대한 내용을 만들고 이걸 Abstract로 만들어서 상속받은 class에서 직접구현 -> 모두 다른 내용을 가진 하위 클래스 이러한 것들을 공격때 마다 돌려가며 실행
        public Character Owner;
    }
}
