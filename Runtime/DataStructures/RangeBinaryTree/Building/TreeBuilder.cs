using System.Collections.Generic;
using System.Linq;
using SensenToolkit.Mathx;

namespace SensenToolkit.DataStructures.RangeBinaryTree
{
    public static class TreeBuilder
    {
        public const float RangeComparisonPrecision = 1e-7f;
        public const float IntersectionOffsetFix = RangeComparisonPrecision * 1e2f;
        private const int LookupMaxDecimals = 5;
        private const float LookupMaxPrecision = 1e-5f;
        public static RBNode<T> BuildAllNodesUntilLeaves<T>(IList<RBLeafData<T>> leaves)
        {
            leaves = leaves.OrderBy(l => l.Range.Begin).ToList();
            ValidateLeaves(leaves);
            AddMinimalDistanceBetweenLeaves(leaves);
            Range treeRange = new(leaves[0].Range.Begin, leaves[^1].Range.End);
            RBNode<T> root = new(range: treeRange);
            Queue<RangeBuildingStep<T>> queue = new();
            queue.Enqueue(new RangeBuildingStep<T>() {
                Range = treeRange,
                Leaves = leaves,
                Node = root,
                MinEdge = true,
                MaxEdge = true
            });

            while (queue.Count > 0)
            {
                RangeBuildingStep<T> step = queue.Dequeue();
                (RangeBuildingStep<T> leftStep, RangeBuildingStep<T> rightStep) = step.Execute();
                if (leftStep != null) queue.Enqueue(leftStep);
                if (rightStep != null) queue.Enqueue(rightStep);
            }
            return root;
        }

        public static float EnsureLookupValidValue(float val)
        {
            val = (float) System.Math.Round(val, LookupMaxDecimals);
            return val + LookupMaxPrecision;
        }

        private static void AddMinimalDistanceBetweenLeaves<T>(IList<RBLeafData<T>> leaves)
        {
            foreach (RBLeafData<T> leaf in leaves)
            {
                if (leaf != leaves[^1])
                {
                    leaf.Range = leaf.Range.Shrink(endBy: IntersectionOffsetFix);
                }
            }
        }

        private static void ValidateLeaves<T>(IList<RBLeafData<T>> leaves)
        {
            float maxLeafDistance = leaves.Max(leaf => leaf.Range.Length);
            float minLeafDistance = leaves.Min(leaf => leaf.Range.Length);
            float lastMax = float.MinValue;
            foreach (RBLeafData<T> leaf in leaves)
            {
                if (leaf.Range.Begin < lastMax)
                {
                    throw new IntersectingSegmentsException();
                }
                lastMax = leaf.Range.End;
            }
        }
    }
}
