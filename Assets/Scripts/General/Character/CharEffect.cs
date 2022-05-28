


namespace ms
{
    public class CharEffect
    {
        // Character effects from Effect.wz
        public enum Id
        {
            LEVELUP,
            JOBCHANGE,
            SCROLL_SUCCESS,
            SCROLL_FAILURE,
            MONSTER_CARD,
        }

        public static EnumMap<Id, string> PATHS_One = new EnumMap<Id, string>
        {
            [Id.LEVELUP] = "LevelUp",
            [Id.JOBCHANGE] = "JobChanged",
        };
        
        public static EnumMap<Id, string> PATHS_Two = new EnumMap<Id, string>
        {
            [Id.SCROLL_SUCCESS] = "Enchant/Success",
            [Id.SCROLL_FAILURE] = "Enchant/Failure",
            [Id.MONSTER_CARD] = "MonsterBook/cardGet",
        };
    }
}
