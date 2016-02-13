using UnityEngine;

using System.Collections.Generic;

namespace LostLand.Combat.Ship
{
    public class TurnbasedModule : MonoBehaviour
    {
        protected static List<TurnbasedModule> allModules = new List<TurnbasedModule>();

        public static void AddInstance(TurnbasedModule module)
        {
            allModules.Add(module);
        }

        public static void RemoveInstance(TurnbasedModule module)
        {
            allModules.Remove(module);
        }

        public static void StartTurn()
        {
            foreach(TurnbasedModule mod in allModules)
            {
                mod.ResetValues();
            }
        }

        protected virtual void ResetValues()
        {

        }
    }
}