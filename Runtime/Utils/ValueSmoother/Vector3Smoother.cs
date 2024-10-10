using UnityEngine;

namespace SensenToolkit
{
    public class Vector3Smoother : AValueSmoother<Vector3>
    {
        public Vector3Smoother(int bufferSize) : base(bufferSize, Vector3.zero) { }

        protected override Vector3 Divide(Vector3 a, int b) => a / b;
        protected override Vector3 Minus(Vector3 a, Vector3 b) => a - b;
        protected override Vector3 Plus(Vector3 a, Vector3 b) => a + b;
    }
}
