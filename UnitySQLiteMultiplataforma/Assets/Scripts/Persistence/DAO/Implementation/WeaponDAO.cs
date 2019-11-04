using Assets.Scripts.Persistence.DAO.Specification;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Persistence.DAO.Implementation
{
    public class WeaponDAO : IWeaponDAO
    {
        public ISQLiteConnectionProvider ConnectionProvider { get; protected set; }

        public WeaponDAO(ISQLiteConnectionProvider connectionProvider) => ConnectionProvider = connectionProvider;

        public bool DeleteWeapon(int id)
        {
            var commandText = "DELETE FROM Weapon WHERE Id = @id;";

            using (var connection = ConnectionProvider.Connection)
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.Parameters.AddWithValue("@id", id);

                    return command.ExecuteNonQueryWithFK() > 0;
                }
            }
        }

        public Weapon GetWeapon(int id)
        {
            var commandText = "SELECT * FROM Weapon WHERE Id = @id;";
            Weapon weapon = null;

            using (var connection = ConnectionProvider.Connection)
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.Parameters.AddWithValue("@id", id);

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        weapon = new Weapon();

                        weapon.Id = reader.GetInt32(0);
                        weapon.Name = reader.GetString(1);
                        weapon.Attack = reader.GetInt32(2);
                        weapon.Price = reader.GetDouble(3);                        
                    }
                }
            }
            return weapon;
        }

        public bool SetWeapon(Weapon weapon)
        {
            var commandText = "INSERT INTO Weapon (Name, Attack, Price) VALUES (@name, @attack, @price);";

            using (var connection = ConnectionProvider.Connection)
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;

                    command.Parameters.AddWithValue("@name", weapon.Name);
                    command.Parameters.AddWithValue("@attack", weapon.Attack);
                    command.Parameters.AddWithValue("@price", weapon.Price);

                    return command.ExecuteNonQueryWithFK() > 0;
                }
            }
        }

        public bool UpdateWeapon(Weapon weapon)
        {
            var commandText =
            "UPDATE Weapon SET " +
            "Name = @name, " +
            "Attack = @attack, " +
            "Price = @price " +
            "WHERE Id = @id;";

            using (var connection = ConnectionProvider.Connection)
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;

                    command.Parameters.AddWithValue("@id", weapon.Id);
                    command.Parameters.AddWithValue("@name", weapon.Name);
                    command.Parameters.AddWithValue("@attack", weapon.Attack);
                    command.Parameters.AddWithValue("@price", weapon.Price);

                    return command.ExecuteNonQueryWithFK() > 0;
                }
            }
        }
    }
}