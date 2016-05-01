using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks2DOnline.Core.Collections.Base
{
    public abstract class FastSearchCollection<TObject, TLink> 
    {
        protected class Node
        {
            public Node Left { get; set; }
            public Node Right { get; set; }

            public Node() { }

            public Node(Node left, Node right)
            {
                Left = left;
                Right = right;
            }
        }
        protected class LinkedObject : Node
        {
            public TLink Link { get; set; }
            public Node Data { get; set; }

            public override bool Equals(object obj)
            {
                var item = (TLink)obj;
                return item != null && Link.Equals(item);
            }
        }
        protected class DataObject : Node
        {
            public Node Link { get; set; }
            public TObject Data { get; set; }

            public override bool Equals(object obj)
            {
                var item = (TObject)obj;
                return item != null && Data.Equals(item);
            }
        }

        protected readonly Node Root = new Node();

        protected abstract int Cmp(TObject lhs, TObject rhs);
        protected abstract int Cmp(TLink lhs, TLink rhs);

        public void Add(TObject item, TLink linkItem)
        {
            var dataObject = new DataObject();
            var linkedObject = new LinkedObject();

            dataObject.Data = item;
            dataObject.Link = linkedObject;

            linkedObject.Data = dataObject;
            linkedObject.Link = linkItem;

            if (Root.Right == null)
                Root.Right = dataObject;
            else
                Add((DataObject)Root.Right, dataObject);

            if (Root.Left == null)
                Root.Left = linkedObject;
            else
                Add((LinkedObject)Root.Left, linkedObject);
        }

        private void Add(LinkedObject root, LinkedObject newNode)
        {
            if (Cmp(root.Link, newNode.Link) < 0)
            {
                if (root.Right != null)
                    Add((LinkedObject)root.Right, newNode);
                else root.Right = newNode;
            }
            else if (Cmp(root.Link, newNode.Link) > 0)
            {
                if (root.Left != null)
                    Add((LinkedObject)root.Left, newNode);
                else root.Left = newNode;
            }
        }

        private void Add(DataObject root, DataObject newNode)
        {
            if (Cmp(root.Data, newNode.Data) <= 0)
            {
                if (root.Right != null)
                    Add((DataObject)root.Right, newNode);
                else root.Right = newNode;
            }
            else
            {
                if (root.Left != null)
                    Add((DataObject)root.Left, newNode);
                else root.Left = newNode;
            }
        }

        private Node FindNode(Node root, object item)
        {
            if (root == null) return null;

            if (root.Equals(item))
                return root;

            var node = FindNode(root.Left, item);
            return node ?? FindNode(root.Right, item);
        }

        public TLink Get(TObject item)
        {
            var tmp = (DataObject)FindNode(Root.Right, item);
            return ((LinkedObject) tmp.Link).Link;
        }

        public TObject Get(TLink item)
        {
            var tmp = (LinkedObject) FindNode(Root.Left, item);
            return ((DataObject) tmp.Data).Data;
        }
    }
}
