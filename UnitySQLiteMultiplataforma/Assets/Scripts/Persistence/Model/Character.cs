using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Agility { get; set; }
    public int Health { get; set; }
    public int WeaponId { get; set; }
    
    public Character(int id, string name, int attack, int defense, int agility, int health, int weaponId)
    {
        Id = id;
        Name = name;
        Attack = attack;
        Defense = defense;
        Agility = agility;
        Health = health;
        WeaponId = weaponId;
    }
}
