using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SensenToolkit.Internal
{
    public class CutGraphNode
    {
        public Vector2 Position;
        public bool IsCutIntersection;
        public bool IsSideA;
        public bool IsSideB;
        // public bool IsSideANext;
        public CutGraphNode NextNode;
        public CutGraphNode PreviousNode;
        public CutGraphNode NextCrossingCutNode;
        public CutGraphNode PreviousCrossingCutNode { get; internal set; }
        public bool IsAVertexThatWasCut { get; internal set; }
        public bool IsCornerCut { get; internal set; }
        public bool IsVertex => !IsCutIntersection;
        public bool IsSideAExclusive => IsSideA && !IsSideB;
        public bool IsSideBExclusive => IsSideB && !IsSideA;

        public List<CutGraphNode> SideABranchNodes { get; } = new();
        public List<CutGraphNode> SideBBranchNodes { get; } = new();
        public int SideABranchCount => SideABranchNodes.Count;
        public int SideBBranchCount => SideBBranchNodes.Count;
        public bool HasBranchToSideA => SideABranchCount > 0;
        public bool HasBranchToSideB => SideBBranchCount > 0;
        public bool HasOneBranchToSideA => SideABranchCount == 1;
        public bool HasOneBranchToSideB => SideBBranchCount == 1;

        // public Polygon2DNode SideANode => IsSideANext ? NextNode : PreviousNode;
        // public Polygon2DNode SideBNode => IsSideANext ? PreviousNode : NextNode;
    }
}
