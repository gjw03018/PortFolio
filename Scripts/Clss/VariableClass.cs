using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Variable
{
    public class levelpoint
    {
        private const float level01 = 1.2f;
        private const float level02 = 1.2f;
        private const float level03 = 1.2f;
        private const float level04 = 1.3f;
        private const float level05 = 1.3f;

        public float GetPoint(int i)
        {
            switch (i)
            {
                case 1:
                    return level01;
                case 2:
                    return level02;
                case 3:
                    return level03;
                case 4:
                    return level04;
                case 5:
                    return level05;
            }
            return 1;
        }
    }

    public enum GoodsType { Gold, Jewelry };
    public enum InvenTpye { All, Smith, Upgarde, Equip}
    public enum Equipment { ETC, Head, Chest, Legs, Shoes }
    public enum CharacterGrade {C, B, A, S}

    public enum ItemGrade
    {
        C, B, A, S
    }
    public enum ItemType
    {
        Use, Equip, Smith, Upgrade, ETC
    }

    public enum CharacterType { Player, Monster}

}
