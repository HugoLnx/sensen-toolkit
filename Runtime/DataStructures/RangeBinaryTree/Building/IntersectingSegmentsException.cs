using System;
using System.Runtime.Serialization;

namespace SensenToolkit.DataStructures.RangeBinaryTree
{
    [Serializable]
    public class IntersectingSegmentsException : Exception
    {
        public IntersectingSegmentsException()
        {
        }

        public IntersectingSegmentsException(string message) : base(message)
        {
        }

        public IntersectingSegmentsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IntersectingSegmentsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
