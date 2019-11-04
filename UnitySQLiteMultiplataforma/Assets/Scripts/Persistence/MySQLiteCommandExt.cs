using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Persistence
{
    public static class MySQLiteCommandExt
    {
        public static int ExecuteNonQueryWithFK(this SqliteCommand command)
        {
            var tmp = command.CommandText;
            command.CommandText = $"PRAGMA foreign_keys = true; {tmp}";
            return command.ExecuteNonQuery();
        }
    }
}
