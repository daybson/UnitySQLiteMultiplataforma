using Assets.Scripts.Persistence.DAO.Implementation;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text t;

    void Awake()
    {
        GamesCodeDataSource.RegisterInitialLoad += CarregarDados;
    }

    //chamado pelo onclick de um botao na UI
    public void CarregarDados()
    {
        var w = GamesCodeDataSource.Instance.WeaponDAO.GetWeapon(1);
        //var wd = new WeaponDAO(GamesCodeDataSource.Instance);
        //var w = wd.GetWeapon(1);
        print(w.Name);
        t.text = w.Name;
    }
}
