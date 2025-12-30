namespace Work.TRPG.UI
{
    public class DiceRollPresenter
    {
        private readonly DiceRollModel _model;
        private readonly DiceRollView _view;

        public bool IsCompleted { get; private set; }
        public CheckInfo LastResult => _model.CheckInfo.Value;

        public DiceRollPresenter(DiceRollModel model, DiceRollView view)
        {
            _model = model;
            _view = view;

            _view.OnClickConfirm = OnClickConfirm;
        }

        public void StartRoll()
        {
            IsCompleted = false;
            _view.SetActive(true);
            _model.Roll();
            _view.SetStatUI(_model.Stat.Value);
            _view.PlayRollAnimation(() =>
            {
                _view.SetResultUI(_model.CheckInfo.Value);
            });
        }

        private void OnClickConfirm()
        {
            IsCompleted = true;
            _view.SetActive(false);
        }
    }
}