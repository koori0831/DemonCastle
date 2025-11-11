namespace Core
{
    public class ReactiveProperty<T>
    {
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    OnValueChanged?.Invoke(_value);
                }
            }
        }
        public event System.Action<T> OnValueChanged;

        public ReactiveProperty(T initialValue = default)
        {
            _value = initialValue;
        }
    }
}