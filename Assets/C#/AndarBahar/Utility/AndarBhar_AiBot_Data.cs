using System.Collections.Generic;
using UnityEngine;
namespace AndarBahar.Utility
{
    [CreateAssetMenu]
   public class AndarBhar_AiBot_Data : ScriptableObject
    {
        public List<Bot> bots;
        public void AddData(Bot data)
        {
            bots.Add(data);
        }
        public List<Bot> GetData()
        {
            return bots;
        }
    }
}
