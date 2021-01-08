using Assets.Scripts.Persistence.DAO.Implementation;

using Mono.Data.Sqlite;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MyNewGame : MonoBehaviour
{
    public Text label;
    public Text label2;
    public InputField input;

    private void Start()
    {
       // label2.text = $"SAVED VERSION: {PlayerPrefs.GetInt(SQLiteDataSource.DB_NAME_PREF)} | CODE VERSION: {SQLiteDataSource.MY_DB_VERSION}";
    }

    public void MyMethod()
    {
        label2.text = "Segundo Método";
    }

    public void LoadDataAwake()
    {
        try
        {
            var weaponDAO = new WeaponDAO(GamesCodeDataSource.Instance);
            var w = weaponDAO.GetWeapon(2);
            label.text = w.Name + " : " + w.Attack.ToString();
        }
        catch (Exception e)
        {
            label.text = "ERRO AO LER O BANCO DE DADOS";
        }
    }


    public void LoadData()
    {
        var weaponDAO = new WeaponDAO(GamesCodeDataSource.Instance);
        var w = weaponDAO.GetWeapon(1);
        label.text = w.Name + " : " + w.Attack.ToString();
    }


    public void SaveNewWeapon()
    {
        var weaponDAO = new WeaponDAO(GamesCodeDataSource.Instance);
        var w = new Weapon(0, input.text, 150, 221.5f);

        if (weaponDAO.SetWeapon(w))
        {
            label.text = w.Name + " : " + w.Attack.ToString();
        }

    }
}
