namespace Work.TRPG.Dialogue
{
    /// <summary>
    /// 대화 노드 종류.
    /// </summary>
    public enum DialogueNodeType
    {
        None,
        Start,
        Dialogue,
        Choice,
        Check,
        End
    }

    /// <summary>
    /// TRPG 판정에 사용되는 스탯 타입.
    /// </summary>
    public enum StatType
    {
        None,
        STR,
        DEF,
        DEX,
        CON,
        INT,
        LUK,
        SAN
    }
}
