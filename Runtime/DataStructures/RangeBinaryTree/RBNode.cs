namespace SensenToolkit.DataStructures.RangeBinaryTree
{
    public class RBNode<T>
    {
        private readonly Range _range;
        public RBNode<T> Left {private get; set;}
        public RBNode<T> Right {private get; set;}
        private readonly T _content;

        public RBNode(Range range, RBNode<T> left = null, RBNode<T> right = null, T content = default)
        {
            _range = range;
            Left = left;
            Right = right;
            _content = content;
        }

        private int _DEBUG_NextForCallsCount;
        public T Lookup(float val)
        {
            val = TreeBuilder.EnsureLookupValidValue(val);
            _DEBUG_NextForCallsCount = 0;
            RBNode<T> previousNode = this;
            RBNode<T> node = this;
            while (node != null)
            {
                previousNode = node;
                node = node.NextFor(val);
            }
            if (previousNode == null)
            {
                return default;
            }
            else
            {
                return previousNode._content;
            }
        }

        private RBNode<T> NextFor(float val)
        {
            // UnityEngine.Debug.Log($"NextFor: {val} {_range} contains={_range.Contains(val)} side={(val >= _range.Middle ? "RIGHT" : "LEFT")}");
            // if (_DEBUG_NextForCallsCount++ > 1000) throw new System.Exception("NextFor is stuck in a loop");
            if (!RangeContains(val))
            {
                return null;
            }
            if (val >= _range.Middle)
            {
                return Right?.RangeContains(val) == true ? Right : null;
            }
            else
            {
                return Left?.RangeContains(val) == true ? Left : null;
            }
        }

        private bool RangeContains(float val)
        {
            return _range.Contains(val);
        }
    }
}
