using System;
using System.Collections.Generic;
using System.Text;

namespace evolib
{
    public static class BattleEntry
    {
        public enum Type
        {
            Matching = 0,
            Cancel,
            Leave,
            SessionError,
        }
    }
}
