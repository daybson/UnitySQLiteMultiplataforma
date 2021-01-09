using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SQLMigration
{
    public readonly static Dictionary<int, List<string>> versionScripts =
        new Dictionary<int, List<string>>()
        {
            {
                2,
                new List<string>() 
                {
                    "UPDATE Weapon SET Attack = 35 WHERE Id = 1;",
                    "DELETE FROM Character WHERE Id = 1;",
                }
            },
            {
                3,
                new List<string>()
                {
                    "ALTER TABLE Character ADD COLUMN Mana INTEGER;",
                    "ALTER TABLE Weapon RENAME COLUMN Attack TO Damage;"
                }
            },
        };
}

