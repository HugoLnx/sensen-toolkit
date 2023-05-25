using System.Collections.Generic;

namespace SensenToolkit.DataStructures.RangeBinaryTree
{
    public class RangeBuildingStep<T>
    {
        public RBNode<T> Node { get; set; }
        public IList<RBLeafData<T>> Leaves { get; set; }
        public Range Range { get; set; }
        public bool MinEdge { get; set; }
        public bool MaxEdge { get; set; }
        public float Min => Range.Min;
        public float Max => Range.Max;

        public (RangeBuildingStep<T>, RangeBuildingStep<T>) Execute()
        {
            RangeBuildingStep<T> left = this.BuildHalfRange(isLeft: true);
            RangeBuildingStep<T> right = this.BuildHalfRange(isLeft: false);
            return (left, right);
        }

        private RangeBuildingStep<T> BuildHalfRange(bool isLeft)
        {
            Range range = isLeft
                ? new Range(begin: Min, end: Range.Middle)
                : new Range(begin: Range.Middle, end: MaxEdge ? Max : Max - TreeBuilder.IntersectionOffsetFix);

            IList<RBLeafData<T>> leaves = new List<RBLeafData<T>>();
            foreach (RBLeafData<T> leaf in Leaves)
            {
                if (Range.Intersects(leaf.Range, precision: TreeBuilder.RangeComparisonPrecision))
                {
                    leaves.Add(leaf);
                }
            }

            bool rangeDoesNotContainLeaves = leaves.Count == 0;
            if (rangeDoesNotContainLeaves) return null;

            RBNode<T> childNode;
            if (leaves.Count == 1)
            {
                childNode = new RBNode<T>(range: leaves[0].Range, content: leaves[0].Content);
            }
            else
            {
                childNode = new RBNode<T>(range: range);
            }

            if (isLeft) Node.Left = childNode;
            else Node.Right = childNode;

            bool hasNothingToProcessNext = leaves.Count <= 1;
            if (hasNothingToProcessNext) return null;

            return new RangeBuildingStep<T>() {
                Leaves = leaves,
                Range = range,
                Node = childNode,
                MinEdge = isLeft && MinEdge,
                MaxEdge = !isLeft && MaxEdge
            };
        }
    }
}
