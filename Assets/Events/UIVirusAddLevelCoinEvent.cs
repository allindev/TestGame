
using UnityEngine;

namespace Events
{
    public struct UIVirusAddLevelCoinEvent
    {
        public Vector2 WorldPos;

        public UIVirusAddLevelCoinEvent(Vector2 worldPos)
        {
            WorldPos = worldPos;
        }
    }
}
