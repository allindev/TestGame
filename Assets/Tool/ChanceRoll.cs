using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Tool
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ChanceRoll
    {
        private struct Possibility
        {
            public int index;
            public float rarity;
            public bool finished;

            public Possibility(int index, float rarity, bool finished)
            {
                this.index = index;
                this.rarity = rarity;
                this.finished = finished;
            }
        };

        private struct Thing
        {
            float from;
            float to;
            public Possibility pos;

            public Thing(float from, float to, Possibility pos)
            {
                this.from = from;
                this.to = to;
                this.pos = pos;
            }

            //Returns whether or not the given number is within from and to (inclusive)
            public bool isInside(float number)
            {
                return number >= from && number <= to;
            }
        };

        private List<Possibility> list;

        private bool takeAwayPossibilityAfterRoll;

        //if takeAway is true, then the posibility that occurs after a roll is taken out of the next posibilities.
        public ChanceRoll(bool takeAway = false)
        {
            list = new List<Possibility>();
            takeAwayPossibilityAfterRoll = takeAway;
        }


        public void Add(float rarity)
        {
            list.Add(new Possibility(list.Count, rarity, false));
        }


        public int Count()
        {
            return list.Count;
        }


        //Returns the index of the rolled possibility.
        public int Roll()
        {
            float from = 0;
            float to = 0;

            List<Thing> t = new List<Thing>();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].finished) continue;

                to += list[i].rarity;

                t.Add(new Thing(from, to, list[i]));

                from = to;
            }

            float randomNumber = Random.Range(0, to);
            int winningListIndex = -1;
            for (int i = 0; i < t.Count; i++)
            {
                if (t[i].isInside(randomNumber))
                {
                    winningListIndex = t[i].pos.index;
                    break;
                }
            }

            if (winningListIndex == -1) return -1;

            if (takeAwayPossibilityAfterRoll)
            {
                list[winningListIndex] = new Possibility(list[winningListIndex].index, list[winningListIndex].rarity,
                    true);
            }

            return winningListIndex;
        }
    }
}
