using System.Collections.Generic;

namespace SensenToolkit.DataStructures.RangeBinaryTree
{
    public static class RBTree
    {
        public static RBTree<T> FromLeaves<T>(IList<RBLeafData<T>> leaves)
        {
            RBNode<T> root = TreeBuilder.BuildAllNodesUntilLeaves<T>(leaves);
            return new RBTree<T>(root);
        }
    }

    public class RBTree<T>
    {
        private readonly RBNode<T> _root;

        public RBTree(RBNode<T> root)
        {
            _root = root;
        }

        public T Lookup(float val)
        {
            return _root.Lookup(val);
        }
    }
}
