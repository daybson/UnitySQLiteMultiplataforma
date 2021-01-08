using System.Collections.Generic;

namespace Assets.Scripts.Persistence
{
    public class SQLMigration
    {
        public readonly static Dictionary<int, List<string>> versionScripts = new Dictionary<int, List<string>>()
        {
            { 
                2, 
                new List<string>()
                {
                    "UPDATE Weapon SET Attack = 35 WHERE Id = 1;",
                    "DELETE FROM Character WHERE Id = 1;",
                }
            },
        };
    }
}
