using System.Collections;
using UnityEngine;
using Work.TRPG.UI;
using Yarn.Unity;

namespace Work.Dialogue.CommandBinder
{
    public class DiceCommandBinder : MonoBehaviour
    {
        [SerializeField] private DialogueRunner dialogueRunner;
        [SerializeField] private VariableStorageBehaviour variableStorage;
        [SerializeField] private DiceRollView diceRollView;

        private DiceRollPresenter presenter;

        private void Awake()
        {
            // Yarn 커맨드 등록 (int 파라미터 1개 받는 RollCheckUI)
            dialogueRunner.AddCommandHandler<int>("RollCheckUI", RollCheckUICoroutine);
        }

        private IEnumerator RollCheckUICoroutine(int stat)
        {
            // 1. 모델 생성
            var model = new DiceRollModel(stat);

            // 2. 프레젠터 생성
            presenter = new DiceRollPresenter(model, diceRollView);

            // 3. UI 시작
            presenter.StartRoll();

            // 4. 플레이어가 '확정' 누를 때까지 기다리기
            while (!presenter.IsCompleted)
                yield return null;

            // 5. 결과를 Yarn 변수로 전달
            var info = presenter.LastResult;

            variableStorage.SetValue("$dice_original", info.originalDice);
            variableStorage.SetValue("$dice_final", info.finalDice);
            variableStorage.SetValue("$dice_isDouble", info.isDouble);
            variableStorage.SetValue("$dice_result", (int)info.result);

            // 여기서 코루틴 종료 → Yarn이 자동으로 다음 줄 진행
        }
    }
}
