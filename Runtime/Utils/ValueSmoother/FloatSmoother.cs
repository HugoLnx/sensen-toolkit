namespace SensenToolkit
{
    public class FloatSmoother : AValueSmoother<float>
    {
        public FloatSmoother(
            float increaseDurationSecs,
            float? decreaseDurationSecs = null,
            float? decreaseThreshold = null
        ) : base(increaseDurationSecs, decreaseDurationSecs, decreaseThreshold) { }

        protected override float Minus(float a, float b) => a - b;
        protected override float Plus(float a, float b) => a + b;
        protected override float Multiply(float a, float b) => a * b;
        protected override float ToFloat(float a) => a;
    }
}
