
using UnityEngine;

namespace Events
{
    public struct UIVirusAddTotalCoinEvent
    {
        public Vector2 UiPos;
        public bool IsPositive;

        public UIVirusAddTotalCoinEvent(Vector2 uiPos, bool isPositive)
        {
            UiPos = uiPos;
            IsPositive = isPositive;
        }
    }
   
}
