namespace SensenToolkit
{
    public class CircularLinkedListNode<T>
    {
        public CircularLinkedListNode<T> Next { get; internal set; }
        public CircularLinkedListNode<T> Previous { get; internal set; }
        public T Value { get; set; }
        public CircularLinkedList<T> List { get; internal set; }

        public CircularLinkedListNode(CircularLinkedList<T> list, T value)
        {
            List = list;
            Value = value;
        }
    }
}
