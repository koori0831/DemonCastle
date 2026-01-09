using System;
using UnityEngine;
using Work.Utils.Helpers;

namespace Work.Inputs.Test
{
    public class ClickTest : MonoBehaviour
    {
        public void Awake()
        {
            ClickHelper.OnAttackTriggerEvent += HandleClickEvent;
        }

        public void OnDestroy()
        {
            ClickHelper.OnAttackTriggerEvent -= HandleClickEvent;
        }

        private void HandleClickEvent()
        {
            transform.position = ClickHelper.LastClickData.Point;
        }
    }
}