using Work.Characters.Code;

namespace Work.Characters.Stats.Code
{
    public struct StatBlock
    {
        int STR, INT, LUK, DEF, SAN, CON, DEX;

        public int GetStatValue(CharacterStatEnum stat)
        {
            return stat switch
            {
                CharacterStatEnum.STR => STR,
                CharacterStatEnum.INT => INT,
                CharacterStatEnum.LUK => LUK,
                CharacterStatEnum.DEF => DEF,
                CharacterStatEnum.SAN => SAN,
                CharacterStatEnum.CON => CON,
                CharacterStatEnum.DEX => DEX,
                _ => 0,
            };
        }

        public static StatBlock operator +(StatBlock a, StatBlock b) => new()
        {
            STR = a.STR + b.STR,
            INT = a.INT + b.INT,
            LUK = a.LUK + b.LUK,
            DEF = a.DEF + b.DEF,
            SAN = a.SAN + b.SAN,
            CON = a.CON + b.CON,
            DEX = a.DEX + b.DEX,
        };
    }
}