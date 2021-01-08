using Assets.Scripts.Persistence.DAO.Specification;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Persistence.DAO.Implementation
{
    public class CharacterDAO : ICharacterDAO
    {
        public ISQLiteConnectionProvider ConnectionProvider { get; protected set; }

        public CharacterDAO(ISQLiteConnectionProvider connectionProvider) => ConnectionProvider = connectionProvider;

        public bool DeleteCharacter(int id)
        {
            var commandText = "DELETE FROM Character WHERE Id = @id;";

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

        public Character GetCharacter(int id)
        {
            var commandText = "SELECT * FROM Character WHERE Id = @id;";
            Character character = null;

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
                        character = new Character();

                        character.Id = reader.GetInt32(0);
                        character.Name = reader.GetString(1);
                        character.Attack = reader.GetInt32(2);
                        character.Defense = reader.GetInt32(3);
                        character.Agility = reader.GetInt32(4);
                        character.Health = reader.GetInt32(5);
                        character.WeaponId = reader.GetInt32(6);
                    }
                }
            }
            return character;
        }

        public bool SetCharacter(Character character)
        {
            var commandText = "INSERT INTO Character(Name, Attack, Defense, Agility, Health, WeaponId) " +
            " VALUES (@name, @attack, @defense, @agility, @health, @weaponId);";

            using (var connection = ConnectionProvider.Connection)
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;

                    command.Parameters.AddWithValue("@name", character.Name);
                    command.Parameters.AddWithValue("@attack", character.Attack);
                    command.Parameters.AddWithValue("@defense", character.Defense);
                    command.Parameters.AddWithValue("@agility", character.Agility);
                    command.Parameters.AddWithValue("@health", character.Health);
                    command.Parameters.AddWithValue("@weaponId", character.WeaponId);

                    return command.ExecuteNonQueryWithFK() > 0;
                }
            }
        }

        public bool UpdateCharacter(Character character)
        {
            var commandText =
            "UPDATE Character SET " +
            "Name = @name, " +
            "Attack = @attack, " +
            "Defense = @defense, " +
            "Agility = @agility, " +
            "Health = @health, " +
            "WeaponId = @weaponId " +
            "WHERE Id = @id;";

            using (var connection = ConnectionProvider.Connection)
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;

                    command.Parameters.AddWithValue("@id", character.Id);
                    command.Parameters.AddWithValue("@name", character.Name);
                    command.Parameters.AddWithValue("@attack", character.Attack);
                    command.Parameters.AddWithValue("@defense", character.Defense);
                    command.Parameters.AddWithValue("@agility", character.Agility);
                    command.Parameters.AddWithValue("@health", character.Health);
                    command.Parameters.AddWithValue("@weaponId", character.WeaponId);

                    return command.ExecuteNonQueryWithFK() > 0;
                }
            }
        }
    }
}