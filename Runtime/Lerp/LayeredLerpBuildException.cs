using System;

namespace SensenToolkit.Lerp
{
    public class LayeredLerpBuildException : Exception
    {
        public LayeredLerpBuildException(string message) : base(message)
        {
        }

        public LayeredLerpBuildException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
