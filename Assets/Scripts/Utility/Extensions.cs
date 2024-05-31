using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class Extensions
    {
        public static void Shuffle<T>(this List<T> items)
        {      
            for(int i = 0; i < items.Count - 1; i++)
            {
                int pos = Random.Range(i, items.Count); 
                (items[i], items[pos]) = (items[pos], items[i]);
            }
        }
        
        public static void ShuffleWithoutLastRepeat<T>(this List<T> items, T previousLast)
        {      
            items.Shuffle();
            
            if (EqualityComparer<T>.Default.Equals(items[0], previousLast))
            {
                int pos = Random.Range(1, items.Count); 
                (items[0], items[pos]) = (items[pos], items[0]);
            }
        }
    }
}
