namespace SensenToolkit
{
    public class BooleanMonitor
    {
        public event System.Action OnActiveEnter;
        public event System.Action OnActiveStay;
        public event System.Action OnActiveExit;

        public System.Func<bool> Getter { get; }
        private bool _wasActive;

        public BooleanMonitor(System.Func<bool> valueGetter)
        {
            Getter = valueGetter;
        }

        public void Tick()
        {
            bool isActive = Getter();
            if (isActive)
            {
                if (!_wasActive) OnActiveEnter?.Invoke();
                OnActiveStay?.Invoke();
            }
            else if (_wasActive)
            {
                OnActiveExit?.Invoke();
            }
            _wasActive = isActive;
        }
    }
}
