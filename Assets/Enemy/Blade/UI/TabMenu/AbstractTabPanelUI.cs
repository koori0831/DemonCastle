using DG.Tweening;
using UnityEngine;

namespace Blade.UI.TabMenu
{
    public abstract class AbstractTabPanelUI : MonoBehaviour
    {
        [field: SerializeField] public TabDataSO TabData { get; private set; }
        [field: SerializeField] public RectTransform RectTrm { get; private set; }
        [SerializeField] protected float hideHeight;
        [SerializeField] protected float initHeight;

        public virtual void OpenPanel(bool isTween)
        {
            if (isTween)
            {
                RectTrm.anchoredPosition = new Vector2(RectTrm.anchoredPosition.x, hideHeight);
                RectTrm.DOKill();
                RectTrm.DOAnchorPosY(initHeight, 0.25f).SetUpdate(true);
            }
            else
            {
                RectTrm.anchoredPosition = new Vector2(RectTrm.anchoredPosition.x, initHeight);
            }
        }
 
        public virtual void ClosePanel(bool isTween)
        {
            if (isTween)
            {
                RectTrm.DOKill();
                RectTrm.DOAnchorPosY(hideHeight, 0.25f).SetUpdate(true);
            }
            else
            {
                RectTrm.anchoredPosition = new Vector2(RectTrm.anchoredPosition.x, hideHeight);
            }
        }
        
        #if UNITY_EDITOR
        [ContextMenu("Close Panel")]
        private void ClosePanel() => ClosePanel(false);
        
        [ContextMenu("Open Panel")]
        private void OpenPanel() => OpenPanel(false);
        #endif
    }
}