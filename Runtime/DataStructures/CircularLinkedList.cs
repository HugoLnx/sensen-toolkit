using System;
using System.Collections.Generic;

namespace SensenToolkit.DataStructures
{
    public class CircularLinkedList<T>
    {
        private HashSet<CircularLinkedListNode<T>> _nodes = new();
        public IReadOnlyCollection<CircularLinkedListNode<T>> Nodes => _nodes;
        public CircularLinkedListNode<T> Root { get; private set; }
        public int Count { get; private set; } = 0;

        public void SetRoot(CircularLinkedListNode<T> node)
        {
            if (Root == null)
            {
                AddFirstNode(node);
                return;
            }

            if (Contains(node))
            {
                Root = node;
                return;
            }

            throw new InvalidOperationException("Cannot set node as root if it is not in the list");
        }

        public CircularLinkedListNode<T> AddNext(T value)
        {
            CircularLinkedListNode<T> node = new(this, value);
            return AddNext(node);
        }

        public CircularLinkedListNode<T> AddPrevious(T value)
        {
            CircularLinkedListNode<T> node = new(this, value);
            return AddPrevious(node);
        }

        public CircularLinkedListNode<T> AddAfter(CircularLinkedListNode<T> node, T value)
        {
            CircularLinkedListNode<T> newNode = new(this, value);
            return AddAfter(node, newNode);
        }

        public CircularLinkedListNode<T> AddBefore(CircularLinkedListNode<T> node, T value)
        {
            CircularLinkedListNode<T> newNode = new(this, value);
            return AddBefore(node, newNode);
        }

        public CircularLinkedListNode<T> AddFirstNode(T value)
        {
            CircularLinkedListNode<T> node = new(this, value);
            return AddFirstNode(node);
        }

        public CircularLinkedListNode<T> AddNext(CircularLinkedListNode<T> node)
        {
            return Root == null
                ? AddFirstNode(node)
                : AddAfter(Root, node);
        }

        public CircularLinkedListNode<T> AddPrevious(CircularLinkedListNode<T> node)
        {
            return Root == null
                ? AddFirstNode(node)
                : AddBefore(Root, node);
        }

        public CircularLinkedListNode<T> AddAfter(CircularLinkedListNode<T> node, CircularLinkedListNode<T> newNode)
        {
            if (node == null) return AddNext(newNode);
            CheckNodeIsInList(node);
            EnsureNodeIsNotIncludedInList(newNode);
            RegisterNode(newNode);
            newNode.Next = node.Next;
            newNode.Previous = node;
            node.Next = newNode;
            return newNode;
        }

        public CircularLinkedListNode<T> AddBefore(CircularLinkedListNode<T> node, CircularLinkedListNode<T> newNode)
        {
            if (node == null) return AddPrevious(newNode);
            CheckNodeIsInList(node);
            EnsureNodeIsNotIncludedInList(newNode);
            RegisterNode(newNode);
            newNode.Next = node;
            newNode.Previous = node.Previous;
            node.Previous = newNode;
            return newNode;
        }

        public CircularLinkedListNode<T> AddFirstNode(CircularLinkedListNode<T> node)
        {
            if (Root != null)
            {
                throw new InvalidOperationException("Cannot add first node when current node is set");
            }
            EnsureNodeIsNotIncludedInList(node);
            RegisterNode(node);
            node.Next = node;
            node.Previous = node;
            return Root;
        }

        public CircularLinkedListNode<T> Remove(CircularLinkedListNode<T> node)
        {
            CheckNodeIsInList(node);
            UnregisterNode(node);
            CircularLinkedListNode<T> nodeNext = node.Next;
            CircularLinkedListNode<T> nodePrevious = node.Previous;
            nodePrevious.Next = nodeNext;
            nodeNext.Previous = nodePrevious;
            node.Next = null;
            node.Previous = null;
            return node;
        }

        public CircularLinkedListNode<T> RemoveNext(T value)
        {
            CircularLinkedListNode<T> node = FindNext(value);
            if (node == null) return null;
            return Remove(node);
        }

        public CircularLinkedListNode<T> FindNext(T value)
        {
            CircularLinkedListNode<T> node = Root;
            for (int i = 0; i < Count; i++)
            {
                if (node.Value.Equals(value)) return node;
                node = node.Next;
            }
            return null;
        }

        public CircularLinkedListNode<T> FindPrevious(T value)
        {
            CircularLinkedListNode<T> node = Root;
            for (int i = 0; i < Count; i++)
            {
                if (node.Value.Equals(value)) return node;
                node = node.Previous;
            }
            return null;
        }

        public bool Contains(CircularLinkedListNode<T> node)
        {
            return _nodes.Contains(node);
        }

        private void RegisterNode(CircularLinkedListNode<T> node)
        {
            Root ??= node;
            Count++;
            _nodes.Add(node);
            node.List = this;
        }

        private void UnregisterNode(CircularLinkedListNode<T> node)
        {
            if (node == Root)
            {
                Root = node.Next;
            }
            Count--;
            _nodes.Remove(node);
            node.List = null;
        }

        private void CheckNodeIsInList(CircularLinkedListNode<T> node)
        {
            if (node.List != this || !Contains(node))
            {
                throw new InvalidOperationException("Node is not in this list");
            }
        }

        private void EnsureNodeIsNotIncludedInList(CircularLinkedListNode<T> node)
        {
            if (node.List != this) return;
            if (Contains(node)) Remove(node);
        }
    }
}
