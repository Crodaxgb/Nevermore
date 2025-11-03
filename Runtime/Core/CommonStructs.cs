namespace NevermoreStudios.Core
{
    [System.Serializable]
    public struct MinMaxData<T>
    {
        public T MinValue;
        public T MaxValue;

        public MinMaxData(T min, T max)
        {
            MinValue = min;
            MaxValue = max;
        }
    }
}
