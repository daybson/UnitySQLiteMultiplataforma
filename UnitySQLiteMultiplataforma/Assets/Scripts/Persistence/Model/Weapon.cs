using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Attack { get; set; }
    public double Price { get; set; }
 
    public Weapon(int id, string name, int attack, double price)
    {
        Id = id;
        Name = name;
        Attack = attack;
        Price = price;
    }

    public Weapon()
    {
    }
}
