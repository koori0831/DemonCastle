using System;

namespace Work.TRPG.Dialogue
{
    public static class DialogueGuidUtility
    {
        public static string CreateGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
