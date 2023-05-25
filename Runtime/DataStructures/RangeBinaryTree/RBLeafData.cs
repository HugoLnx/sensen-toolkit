namespace SensenToolkit.DataStructures.RangeBinaryTree
{
    public class RBLeafData<T>
    {
        public Range Range { get; set; }
        public T Content { get; set; }

        public RBLeafData(Range range, T content)
        {
            Range = range;
            Content = content;
        }
    }
}
