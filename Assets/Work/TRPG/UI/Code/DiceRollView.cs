using Core.Helper;
using LitMotion;
using LitMotion.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Work.TRPG.UI
{
    public class DiceRollView : MonoBehaviour
    {
        [Header("UGUI References")]
        [SerializeField] private CanvasGroup root;
        [SerializeField] private TMP_Text firstDiceText;//10의 자리
        [SerializeField] private TMP_Text secondDiceText;//1의 자리
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private Button confirmButton;

        [Header("Animation Settings")]
        [SerializeField] private float rollDuration = 1.0f;

        private MotionHandle _rollMotionHandle;

        public Action OnClickConfirm;

        private void OnEnable()
        {
            confirmButton.onClick.AddListener(() => OnClickConfirm?.Invoke());
            ResultTextRest();
            SetActive(false);
            confirmButton.interactable = false;
        }

        private void OnDestroy()
        {
            confirmButton.onClick.RemoveAllListeners();
        }

        public void SetActive(bool active)
        {
            root.alpha = active ? 1 : 0;
            root.blocksRaycasts = active;
            if (active == false) ResultTextRest();
        }

        private void ResultTextRest()
        {
            resultText.gameObject.SetActive(false);
            resultText.transform.localScale = Vector3.zero;
        }

        public void SetResultUI(CheckInfo checkInfo)
        {
            firstDiceText.text = (checkInfo.finalDice / 10).ToString();
            secondDiceText.text = (checkInfo.finalDice % 10).ToString();
            string resultStr = checkInfo.result switch
            {
                CheckResult.CriticalSuccess => "대성공",
                CheckResult.Success => "성공",
                CheckResult.Failure => "실패",
                CheckResult.Fumble => "대실패",
                _ => "오류"
            };
            resultText.text = resultStr;

            resultText.gameObject.SetActive(true);
            LMotion.Create(Vector3.zero, Vector3.one, rollDuration / 2)
                .WithEase(Ease.OutBounce)
                .BindToLocalScale(resultText.transform);
        }

        public void PlayRollAnimation(System.Action onComplete)
        {
            // 기존 애니 정지
            if (_rollMotionHandle.IsActive())
                _rollMotionHandle.Cancel();

            confirmButton.interactable = false;

            float start = 0;
            float end = rollDuration;

            _rollMotionHandle = LMotion
                .Create(start, end, rollDuration)
                .WithOnComplete(() =>
                {
                    onComplete?.Invoke();
                    confirmButton.interactable = true;
                })
                .WithEase(Ease.Linear)
                .Bind(value =>
                {
                    int randomValue = RandomHelper.RollDice(1, 99);
                    firstDiceText.text = (randomValue / 10).ToString();
                    secondDiceText.text = (randomValue % 10).ToString();
                });
        }
    }
}