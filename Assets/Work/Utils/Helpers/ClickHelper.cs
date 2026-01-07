using UnityEngine;
using Work.Inputs;

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

        public ClickData(RaycastHit hit, LayerMask targetLayer, MouseClickType clickType)
        {
            Point = hit.point;
            Hit = hit;
            Targetlayer = targetLayer;
            ClickType = clickType;
        }
    }

    public class ClickHelper
    {
        //클릭했을때 이벤트 받아서... 이거 어케 받지
        public ClickHelper()
        {

        }
    }
}
