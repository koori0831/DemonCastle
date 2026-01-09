using Work.Characters.Code;
using Work.Characters.FSM.Code;
using Work.Entities;

namespace Work.Characters.CharacterState
{
    public class CharacterAttackState : State
    {
        private Character _character;
        private StateCompo _stateCompo;
        private CharacterAttackCompo _attackCompo;
        private CharacterMovementCompo _movementCompo;

        public CharacterAttackState(Entity entity, int animHash) : base(entity, animHash)
        {
            _character = entity as Character;
            _stateCompo = _character.GetCompo<StateCompo>();
            _attackCompo = _character.GetCompo<CharacterAttackCompo>(true);
            _movementCompo = _character.GetCompo<CharacterMovementCompo>();
        }

        public override void Enter()
        {
            base.Enter();
            _attackCompo.isAttacking = true;
            _movementCompo.SetCanMove(true);
            _movementCompo.SetMultiplier(0.4f);
        }

        public override void Update()
        {
            base.Update();

            if (IsAnimationEndTriggered)
                _stateCompo.ChangeState("IDLE");

        }

        public override void Exit()
        {
            base.Exit();
            _attackCompo.isAttacking = false;
            _attackCompo.AddAttackCount();
            _movementCompo.SetCanMove(false);
            _movementCompo.SetMultiplier(1);
        }
    }
}
