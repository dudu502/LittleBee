using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Synchronize.Game.Lockstep.Misc
{
    public static class Extends
    {
        public static T SearchFromLastToFirst<T>(this LinkedList<T> linkedList, Predicate<T> predicate)
        {
            var node = linkedList.Last;
            while(node!=null)
            {
                if(node.Value != null)
                {
                    if (predicate(node.Value))
                        return node.Value;
                }
                node = node.Previous;
            }
            return default;
        }
        public static T SearchFromFirstToLast<T>(this LinkedList<T> linkedList, Predicate<T> predicate)
        {
            var node = linkedList.First;
            while (node != null)
            {
                if (node.Value != null)
                {
                    if (predicate(node.Value))
                        return node.Value;
                }
                node = node.Next;
            }
            return default;
        }

        public static void ForEachFromLastToFirst<T>(this LinkedList<T> linkedList,Action<T> action)
        {
            var node = linkedList.Last;
            while(node!=null)
            {
                if (node.Value != null)
                    action?.Invoke(node.Value);
                node = node.Previous;
            }
        }

        public static void ForEachFromFirstToLast<T>(this LinkedList<T> linkedList,Action<T> action)
        {
            var node = linkedList.First;
            while(node != null)
            {
                if (node.Value != null)
                    action?.Invoke(node.Value);
                node = node.Next;
            }
        }

        public static bool RemoveElementSwapTail<T>(this List<T> list,T element)
        {
            int index = list.IndexOf(element);
            if(index>-1)
            {
                int listCount = list.Count;
                T elementTail = list[listCount - 1];
                list[listCount - 1] = element;
                list[index] = elementTail;
                list.RemoveAt(listCount - 1);
                return true;
            }
            return false;
        }
        public static void SetButtonText(this UnityEngine.UI.Button button,string text)
        {
            if(button!=null)
            {
                var textComponent = button.GetComponentInChildren<UnityEngine.UI.Text>();
                if (textComponent != null)
                    textComponent.text = text;
            }
        }
        public static void SetToggleText(this UnityEngine.UI.Toggle toggle,string value)
        {
            var text = toggle.GetComponentInChildren<UnityEngine.UI.Text>();
            if (text != null)
            {
                text.text = value;
            }
        }
        public static void SetToggleTextColor(this UnityEngine.UI.Toggle toggle,UnityEngine.Color color)
        {
            var text = toggle.GetComponentInChildren<UnityEngine.UI.Text>();
            if(text != null)
            {
                text.color = color;
            }
        }
    }
}
