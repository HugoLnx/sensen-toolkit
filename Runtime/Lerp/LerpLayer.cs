namespace SensenToolkit
{
    // TODO: Make it Garbageless
    // - Make tests for that class
    // - Use pool for LayeredLerp
    // - Use pool for LerpLayer
    public class LerpLayer<T>
    {
        public T Target { get; set; }
        public float RawInterpolationValue { get; set; }
        public float Weight { get; set; }
        public int Inx { get; internal set; }

        public float GetInterpolationValue(int layersCount)
        {
            if (Floatx.IsGreaterOrEqual(RawInterpolationValue, 0f))
            {
                return RawInterpolationValue;
            }
            float avg = 1f / (float) (layersCount - 1);
            return avg * (float) Inx;
        }
    }
}
