using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Blade.UI.TabMenu
{
    public class MenuCanvasUI : MonoBehaviour
    {
        public enum WindowState
        {
            Close, Open, Closing, Opening
        }
        
        [SerializeField] private UIPlayerInputSO uiPlayerInput;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private Dictionary<TabDataSO, TabButtonUI> _tabButtons;
        private Dictionary<TabDataSO, AbstractTabPanelUI> _tabPanels;

        private AbstractTabPanelUI _currentTab;
        private WindowState _windowState = WindowState.Close;
        
        private void Awake()
        {
            _tabButtons = GetComponentsInChildren<TabButtonUI>()
                .ToDictionary(btn => btn.TabData);

            foreach (TabButtonUI btn in _tabButtons.Values)
            {
                btn.OnTabButtonClicked += HandleMenuButtonClick;
                btn.SetActive(false);
            }
            
            _tabPanels = GetComponentsInChildren<AbstractTabPanelUI>()
                .ToDictionary(panel => panel.TabData);

            uiPlayerInput.OnMenuButtonPress += HandleMenuKeyPress;
        }

        private void Start()
        {
            foreach (AbstractTabPanelUI panel in _tabPanels.Values)
            {
                panel.ClosePanel(false); //애니메이션 없이 다 닫아라.
            }
        }

        private void OnDestroy()
        {
            foreach (TabButtonUI btn in _tabButtons.Values)
            {
                btn.OnTabButtonClicked -= HandleMenuButtonClick;
            }
            uiPlayerInput.OnMenuButtonPress -= HandleMenuKeyPress;
        }

        private void HandleMenuKeyPress()
        {
            if (_windowState == WindowState.Closing || _windowState == WindowState.Opening)
                return;

            if (_windowState == WindowState.Close)
            {
                _windowState = WindowState.Opening;
                Time.timeScale = 0;
                SetOpenWindow(true, () => _windowState = WindowState.Open);
            }else if (_windowState == WindowState.Open)
            {
                _windowState = WindowState.Closing;
                SetOpenWindow(false, () =>
                {
                    _windowState = WindowState.Close;
                    Time.timeScale = 1;
                });
            }
        }

        public void SetOpenWindow(bool isOpen, Action callback, bool isTween = true)
        {
            float targetAlpha = isOpen ? 1f : 0;
            if (isTween)
            {
                canvasGroup.DOFade(targetAlpha, 0.25f)
                    .SetUpdate(true)
                    .OnComplete(() => callback?.Invoke());
            }
            else
            {
                canvasGroup.alpha = targetAlpha;
                callback?.Invoke();
            }
            canvasGroup.interactable = isOpen;
            canvasGroup.blocksRaycasts = isOpen;
            
            if(isOpen)
                HandleMenuButtonClick(_tabPanels.Keys.First());
            else
            {
                _currentTab?.ClosePanel(true);
                _currentTab = null;
            }
            
        }

        private void HandleMenuButtonClick(TabDataSO targetTab)
        {
            if (_currentTab != null && _currentTab.TabData == targetTab) return;
            
            foreach(TabButtonUI btn in _tabButtons.Values)
                btn.SetActive(btn.TabData == targetTab);
            
            _currentTab?.ClosePanel(true);
            _currentTab = _tabPanels.GetValueOrDefault(targetTab);
            Debug.Assert(_currentTab != null, $"TabPanelUI for '{targetTab.TabName}' not found.");
            _currentTab.OpenPanel(true);
        }
        
        #if UNITY_EDITOR
        [ContextMenu("Close window")]
        private void CloseWindow()
        {
            SetOpenWindow(false, null, false);
        }
        
        [ContextMenu("Open window")]
        private void OpenWindow()
        {
            SetOpenWindow(true, null, false);
        }
        #endif
    }
}