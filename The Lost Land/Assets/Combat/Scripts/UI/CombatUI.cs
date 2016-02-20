using UnityEngine;
using UnityEngine.UI;

using System.Collections;

using LostLand.Combat.Ship;

namespace LostLand.Combat.UI
{
    public class CombatUI : MonoBehaviour
    {
        [SerializeField]
        private Button applyButton;
        [SerializeField]
        private Button resetButton;

        void Awake()
        {
            UIUtility.RemoveAll(applyButton);
            UIUtility.AddAction(applyButton, Ship.Ship.ActiveShip.ApplyMovement);

            UIUtility.RemoveAll(resetButton);
            UIUtility.AddAction(resetButton, TurnbasedModule.StartTurn);
        }
    }
}