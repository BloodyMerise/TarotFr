﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TarotFr.Domain
{
    //from https://stackoverflow.com/questions/716256/creating-a-circularly-linked-list-in-c
    static class CircularLinkedList
    {
        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
        {
            return current.Next ?? current.List.First;
        }

        public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current)
        {
            return current.Previous ?? current.List.Last;
        }        
    }
}
