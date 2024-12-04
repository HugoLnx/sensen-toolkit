namespace SensenToolkit.Internal
{
    public class CutGraph
    {
        public CutGraphNode[] AllNodes { get; set; }
        public CutGraphNode[] CutNodes { get; set; }

        public CutGraph(CutGraphNode[] allNodes, CutGraphNode[] cutNodes)
        {
            AllNodes = allNodes;
            CutNodes = cutNodes;
        }
    }
}
