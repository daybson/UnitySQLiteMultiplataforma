using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Persistence.DAO.Specification
{
    public interface ICharacterDAO
    {
        ISQLiteConnectionProvider ConnectionProvider { get; }
        bool SetCharacter(Character character);
        bool UpdateCharacter(Character character);
        bool DeleteCharacter(int id);
        Character GetCharacter(int id);
    }
}