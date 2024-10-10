namespace SensenToolkit
{
    public class FloatSmoother : AValueSmoother<float>
    {
        public FloatSmoother(int bufferSize) : base(bufferSize, 0f) { }

        protected override float Divide(float a, int b) => a / b;
        protected override float Minus(float a, float b) => a - b;
        protected override float Plus(float a, float b) => a + b;
    }
}
