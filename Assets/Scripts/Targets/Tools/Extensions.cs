using System.Collections.Generic;
using UnityEngine;

namespace Targets.Tools
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
    }
}
