using System;
using UnityEngine;
using Work.Characters.Events;
using Work.Inputs;
using Work.Utils.EventBus;

namespace Work.Utils.Helpers
{
    public struct ClickData
    {
        //마지막으로 어디를 클릭했는지 
        //클릭한 부분의 레이어는 무엇인지 
        //좌클릭인지 우클릭인지
        public UnityEngine.Vector3 Point { get; private set; }
        public RaycastHit Hit { get; private set; }
        public LayerMask Targetlayer { get; private set; }
        public MouseClickType ClickType { get; private set; }

        public ClickData(RaycastHit hit, MouseClickType clickType)
        {
            
            Debug.Assert(hit.collider != null, "Non-existent reference");
            Point = hit.point;
            Hit = hit;
            Targetlayer = hit.collider.gameObject.layer;
            ClickType = clickType;
        }
    }

    
    public class ClickHelper : IHelper
    {
        //클릭했을때 이벤트 받아서... 이거 어케 받지
        public static ClickData LastClickData { get; private set; }
        public static Action OnAttackTriggerEvent;


        private void HandleMouseClickEvent(MouseClickEvent evt)
        {
            Debug.Assert(evt.ClickData.Hit.collider != null , "ClickData is Non-existent");
            LastClickData = evt.ClickData;

            if(evt.ClickData.ClickType == MouseClickType.Left)
            {
                OnAttackTriggerEvent?.Invoke();
            }
            else if(evt.ClickData.ClickType == MouseClickType.Right)
            {

            }
        }

        public void Initialize()
        {
            Bus<MouseClickEvent>.Events += HandleMouseClickEvent;
        }

        public void Dispose()
        {
            Bus<MouseClickEvent>.Events -= HandleMouseClickEvent;
            OnAttackTriggerEvent = null;
        }
    }
}
