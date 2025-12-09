using System;
using System.Collections.Generic;
using UnityEngine;

namespace Work.Combat
{
    public struct AttackClassParameters
    {
        public GameObject[] RecallObject { get; private set; }
        public List<IntAndStringPair> stringKeyIntValueList { get; private set; }
        public List<FloatAndStringPair> stringKeyFloatValueList { get; private set; }
        public List<VectorAndStringPair> stringKeyVectorValueList { get; private set; }
        public List<EffectAndStringPair> stringKeyEffectValueList { get; private set; }

        public AttackClassParameters(
            GameObject[] recallObject,
            List<IntAndStringPair> stringKeyIntValueList,
            List<FloatAndStringPair> stringKeyFloatValueList,
            List<VectorAndStringPair> stringKeyVectorValueList,
            List<EffectAndStringPair> stringKeyEffectValueList)
        {
            RecallObject = recallObject;
            this.stringKeyFloatValueList = stringKeyFloatValueList;
            this.stringKeyVectorValueList = stringKeyVectorValueList;
            this.stringKeyIntValueList = stringKeyIntValueList;
            this.stringKeyEffectValueList = stringKeyEffectValueList;
        }
    }

    [CreateAssetMenu(fileName = "AttackData", menuName = "SO/Combat/AbstractAttackDataSO", order = -10)]
    public class AbstractAttackDataSO : ScriptableObject
    {
        [field: SerializeField] public string AttackClassPath { get; private set; }
        [field: SerializeField] public string AttackName { get; private set; } = "Attack Name";
        [field: SerializeField] public int AttackCount { get; private set; } = 0;
        [field: SerializeField] public float AttackDamage { get; private set; } = 5;
        [field: SerializeField] public AttackParameters AttackParams { get; private set; }
    }

    [Serializable]
    public class GameObjectAndStringPair
    {
        public string name;
        public GameObject value;
    }

    [Serializable]
    public class IntAndStringPair
    {
        public string name;
        public int value;
    }

    [Serializable]
    public class FloatAndStringPair
    {
        public string name;
        public float value;
    }

    [Serializable]
    public class VectorAndStringPair
    {
        public string name;
        public Vector3 value;
    }

    [Serializable]
    public class EffectAndStringPair
    {
        public string name;
        public ParticleSystem value;
    }


    [Serializable]
    public class AttackParameters
    {
        [SerializeField] private List<GameObjectAndStringPair> RecallObjects;
        [SerializeField] private List<IntAndStringPair> Ints;
        [SerializeField] private List<FloatAndStringPair> Floats;
        [SerializeField] private List<VectorAndStringPair> Vectors;
        [SerializeField] private List<EffectAndStringPair> Effects;

        //public T GetValue<T>(string key)
        //{
        //    Type type = typeof(T);
        //    if (typeof(GameObject) == type)
        //    {
        //        return (T)GetObjectValue(key);
        //    }
        //}

        public bool GetValue(string key, out GameObject value)
        {
            value = GetObjectValue(key);
            return value != null;
        }

        public bool GetValue(string key, out float value)
        {
            value = GetFloatValue(key);
            return value != float.MinValue;
        }

        public bool GetValue(string key, out int value)
        {
            value = GetIntValue(key);
            return value != int.MinValue;
        }

        public bool GetValue(string key, out Vector3 value)
        {
            value = GetVectorValue(key);
            return value != Vector3.zero;
        }

        public bool GetValue(string key, out ParticleSystem value)
        {
            value = GetEffectValue(key);
            return value != null;
        }


        public float GetFloatValue(string key)
        {
            float value = float.MinValue;
            Floats.ForEach(s =>
            {
                if (s.name == key)
                    value = s.value;
            });

            return value;
        }

        public int GetIntValue(string key)
        {
            int value = int.MinValue;
            Ints.ForEach(s =>
            {
                if (s.name == key)
                    value = s.value;
            });

            return value;
        }

        public Vector3 GetVectorValue(string key)
        {
            Vector3 value = Vector3.zero;
            Vectors.ForEach(s =>
            {
                if (s.name == key)
                    value = s.value;
            });

            return value;
        }

        public ParticleSystem GetEffectValue(string key)
        {
            ParticleSystem value = null;
            Effects.ForEach(s =>
            {
                if (s.name == key)
                    value = s.value;
            });

            return value;
        }

        public GameObject GetObjectValue(string key)
        {
            GameObject value = null;
            RecallObjects.ForEach(s =>
            {
                if (s.name == key)
                    value = s.value;
            });

            return value;
        }
    }
}

//이펙트 , 생성할 오브젝트

//일반공격을 기본적으로 3가지를 가지는데 모두 
//클래스로 나누고 싶음 AttackData는 진짜 어떤 어텍인지 얼마만큼의 데미지가 들어가는지만 알고있으면 그만
//클래스로 나눈다 하면 일단 기본적으로 최상위 AbstractAttack이 있을꺼고 그 아래로 어떤 식으로 나눌지가 문제,
//캐릭별로 나누면 OOOAttack 이런식으로 하나씩 나와서 그 안에서 1, 2, 3콤보 공격을 모두 처리하고
//캐릭터 어택별로 하나하나 나누면 000OneComboAttack과 같이 캐릭당 3개씩 나눠지게 될거임 해당 클래스마다 필요한 정보 - 프리펩, 값들 과 같은 것들은
//AttackDataSO에 넣어두고 처음 시작할때 struct형태로 묶어서 보내주면 그곳에 들어있는 파라미터들을 활용해서 만들기