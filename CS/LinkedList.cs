using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    public delegate bool Condition<T>(T n);

    class Node<T>
    {
        public T value;
        public Node<T> next;

        public Node(T n)
        {
            value = n;
            next = null;
        }
    }

    class LinkedList<T>
    {
        public T this[int i]
        {
            get => Get(i);
        }

        Node<T> head = null;
        public int cnt = 0;

        public void AppendNode(T n)
        {
            cnt++;
            Node<T> node = new Node<T>(n);
            Node<T> tail = head;

            while(tail != null)
            {
                if (tail.next == null) break;
                tail = tail.next;
            }

            if (tail == null) head = node;
            else tail.next = node;
        }

        public T Get(int idx)
        {
            if (idx >= cnt) return default;

            Node<T> curNode = head;
            while(curNode != null)
            {
                if (idx == 0) break; 
                curNode = curNode.next;
                idx--;
            }
            return curNode.value;
        }

        public void RemoveAt(int idx)
        {            
            if (idx >= cnt) return;
            cnt--;

            if (head == null || idx == 0)
            {
                head = head.next;
            }
            else
            {
                Node<T> curNode = head;
                Node<T> prevNode = null;

                while (curNode != null)
                {
                    if (idx == 0) break;
                    prevNode = curNode;
                    curNode = curNode.next;
                    idx--;
                }
                prevNode.next = curNode.next;
            }
        }

        public void Remove(T n)
        {
            Node<T> prevNode = null;
            Node<T> curNode = head;

            if (head == null) return;
            if(curNode.value.Equals(n))
            {
                cnt--;
                head = head.next;
            }
            else
            {
                while (curNode != null)
                {
                    if (curNode.value.Equals(n))
                    {
                        cnt--;
                        prevNode.next = curNode.next;
                        break;
                    }
                    prevNode = curNode;
                    curNode = curNode.next;
                }
            }
        }

        public void RemoveAll(Condition<T> func)
        {
            Node<T> prevNode = null;
            Node<T> curNode = head;

            while (curNode != null)
            {
                if (func(curNode.value))
                {
                    if(prevNode == null)
                    {
                        curNode = head = curNode.next;
                    } 
                    else 
                    {
                        curNode = prevNode.next = curNode.next;
                    }
                    cnt--;
                    continue;
                }
                prevNode = curNode;
                curNode = curNode.next;
            }
        }
    }

    class Program 
    {
        static bool Compare(int n)
        {
            return n >= 2 && n <= 4;
        }

        /*static bool Equal (int n)
        {
            return n == 8;
        }*/

        static void Main(string[] args)
        {
            LinkedList<int> sll = new LinkedList<int>();
            sll.AppendNode(10);
            sll.AppendNode(9);
            sll.AppendNode(8);
            sll.AppendNode(7);
            sll.AppendNode(4);
            sll.AppendNode(2);
            sll.AppendNode(7);
            sll.AppendNode(8);
            sll.AppendNode(3);

            for (int i = 0; i < sll.cnt; i++) Console.WriteLine($"[{sll.Get(i)}]");
            Console.WriteLine();

            sll.RemoveAt(3);
            for (int i = 0; i < sll.cnt; i++) Console.WriteLine($"[{sll.Get(i)}]");

            Console.WriteLine();
            sll.Remove(10);
            for (int i = 0; i < sll.cnt; i++) Console.WriteLine($"[{sll.Get(i)}]");
            Console.WriteLine();

            sll.RemoveAll(Compare);
            for (int i = 0; i < sll.cnt; i++) Console.WriteLine($"[{sll.Get(i)}]");
            Console.WriteLine();

            //sll.RemoveAll(Equal);
            sll.RemoveAll(n => n == 8);
            for (int i = 0; i < sll.cnt; i++) Console.WriteLine($"[{sll.Get(i)}]");
            Console.WriteLine();


            LinkedList<string> ssll = new LinkedList<string>();
            ssll.AppendNode("Atents");
            ssll.AppendNode("Game");
            ssll.AppendNode("Aacademy");


            for (int i = 0; i < ssll.cnt; i++) Console.WriteLine($"[{ssll.Get(i)}]");
            Console.WriteLine();
        }
    }
}
