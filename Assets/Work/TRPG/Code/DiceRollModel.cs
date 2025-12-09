using Core;
using System;

namespace Work.TRPG.Code
{
    public class DiceRollModel
    {
        private readonly TRPGDiceSystem _diceSystem;

        public ReactiveProperty<int> Stat { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<CheckInfo> CheckInfo { get; } = new ReactiveProperty<CheckInfo>();
        public ReactiveProperty<bool> IsRolling { get; } = new ReactiveProperty<bool>(false);

        public DiceRollModel(int initialStat)
        {
            _diceSystem = new TRPGDiceSystem();
            Stat.Value = initialStat;
        }

        public void Roll()
        {
            IsRolling.Value = true;
            var info = _diceSystem.RollDice(Stat.Value);
            CheckInfo.Value = info;
            IsRolling.Value = false;
        }
    }
}
