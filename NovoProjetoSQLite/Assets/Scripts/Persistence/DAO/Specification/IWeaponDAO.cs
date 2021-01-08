using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Persistence.DAO.Specification
{
    public interface IWeaponDAO
    {
        ISQLiteConnectionProvider ConnectionProvider { get; }
        bool SetWeapon(Weapon weapon);
        bool UpdateWeapon(Weapon weapon);
        bool DeleteWeapon(int id);
        Weapon GetWeapon(int id);
    }
}