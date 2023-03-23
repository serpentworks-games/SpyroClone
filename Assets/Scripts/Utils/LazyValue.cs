namespace SpyroClone.Utils
{
    /// <summary>
    /// Makes sure init has been called just before first use
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyValue<T>
    {
        T _value;
        bool _initialized = false;
        InitializerDelegate _initializer;

        public delegate T InitializerDelegate();

        public LazyValue(InitializerDelegate initializer)
        {
            _initializer = initializer;
        }

        public T Value 
        {
            get
            {
                ForceInit();
                return _value;
            }
            set 
            {
                _initialized = true;
                _value = Value;
            }
        }

        public void ForceInit()
        {
            if(!_initialized)
            {
                _value = _initializer();
                _initialized = true;
            }
        }
    }
}