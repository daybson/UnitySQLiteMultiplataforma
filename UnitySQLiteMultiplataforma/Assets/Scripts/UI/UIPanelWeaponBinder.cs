using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIPanelWeaponBinder : MonoBehaviour
{
    public Button ButtonAdd;
    public Button ButtonEdit;
    public Button ButtonDelete;
    public Button ButtonSave;
    public Button ButtonCancel;
    public Button ButtonSearch;

    public InputField InputFieldID;
    public InputField InputFieldName;
    public InputField InputFieldAttack;
    public InputField InputFieldPrice;
    public InputField InputFieldSearch;

    public Text TextMessage;

    public Transform ScrollContent;

    public GameObject ResultSearchPrefab;

    public void ClearAllInputs()
    {
        InputFieldID.text = string.Empty;
        InputFieldName.text = string.Empty;
        InputFieldAttack.text = string.Empty;
        InputFieldPrice.text = string.Empty;
        InputFieldSearch.text = string.Empty;

        ScrollContent.GetComponentsInChildren<Button>().ToList().ForEach(b => Destroy(b.gameObject));
        weaponsResultSearch.Clear();
    }

    public void SetModeAddOrEdition()
    {
        ButtonSave.interactable = true;
        ButtonCancel.interactable = true;
        InputFieldName.interactable = true;
        InputFieldAttack.interactable = true;
        InputFieldPrice.interactable = true;

        InputFieldID.interactable = false;
        InputFieldSearch.interactable = false;
        ButtonAdd.interactable = false;
        ButtonEdit.interactable = false;
        ButtonDelete.interactable = false;
        ButtonSearch.interactable = false;

        ScrollContent.GetComponentsInChildren<Button>().ToList().ForEach(b => b.interactable = false);
    }

    protected List<Weapon> weaponsResultSearch = new List<Weapon>();

    protected Weapon WeaponSelected;

    public void SetCancelMode()
    {
        ButtonAdd.interactable = true;
        ButtonSearch.interactable = true;
        InputFieldSearch.interactable = true;

        ButtonEdit.interactable = false;
        ButtonDelete.interactable = false;
        ButtonSave.interactable = false;
        ButtonCancel.interactable = false;
        InputFieldName.interactable = false;
        InputFieldAttack.interactable = false;
        InputFieldPrice.interactable = false;
        InputFieldID.interactable = false;

        ClearAllInputs();
    }

    private void Awake()
    {
        ClearAllInputs();
        SetCancelMode();

        ButtonAdd.onClick.AddListener(
            delegate
            {
                this.WeaponSelected = null;
                SetModeAddOrEdition();
                ClearAllInputs();
            });

        ButtonEdit.onClick.AddListener(
            delegate
            {
                SetModeAddOrEdition();
            });

        ButtonDelete.onClick.AddListener(
            delegate
            {
                DeleteWeapon();
                SetCancelMode();
                ClearAllInputs();
            });

        ButtonSave.onClick.AddListener(
            delegate
            {
                if (this.WeaponSelected == null)
                    SetWeapon();
                else
                    UpdateWeapon();
            });

        ButtonCancel.onClick.AddListener(
            delegate
            {
                SetCancelMode();
            });

        ButtonSearch.onClick.AddListener(
            delegate
            {
                ExecuteSearch();
            });
    }

    private void ExecuteSearch()
    {
        int inputId;
        if (int.TryParse(InputFieldSearch.text, out inputId))
        {
            if (weaponsResultSearch.Any(w => w.Id == inputId))
                return;

            var weapon = GamesCodeDataSource.Instance.WeaponDAO.GetWeapon(inputId);
            if (weapon != null)
            {
                weaponsResultSearch.Add(weapon);
                var buttonWeapon = Instantiate(ResultSearchPrefab);
                buttonWeapon.GetComponentInChildren<Text>().text = weapon.ToString();

                var button = buttonWeapon.GetComponent<Button>();
                button.interactable = true;
                button.onClick.AddListener(delegate { SetWeaponToUI(weapon); });

                buttonWeapon.transform.SetParent(ScrollContent);
                TextMessage.text = string.Empty;
            }
            else
            {
                TextMessage.text = "Not found!";
            }
        }
        else
        {
            TextMessage.text = "Format error!";
        }

    }


    protected void SetWeapon()
    {
        try
        {
            if (InputFieldName.text.Length <= 0)
            {
                TextMessage.text = "Digite o nome";
                return;
            }
            if (InputFieldAttack.text.Length <= 0)
            {
                TextMessage.text = "Digite o ataque";
                return;
            }
            if (InputFieldPrice.text.Length <= 0)
            {
                TextMessage.text = "Digite o preço";
                return;
            }

            var weapon = new Weapon();
            weapon.Name = InputFieldName.text;
            weapon.Attack = int.Parse(InputFieldAttack.text);
            weapon.Price = double.Parse(InputFieldPrice.text);

            if (GamesCodeDataSource.Instance.WeaponDAO.SetWeapon(weapon))
            {
                TextMessage.text = "Weapon inserida com sucesso!";
                SetCancelMode();
            }
            else
                TextMessage.text = "Weapon não foi inserida!";
        }
        catch (Exception ex)
        {
            TextMessage.text = $"Error: {ex.Message}";
        }
    }


    protected void SetWeaponToUI(Weapon weapon)
    {
        this.WeaponSelected = weapon;
        InputFieldID.text = this.WeaponSelected.Id.ToString();
        InputFieldName.text = this.WeaponSelected.Name;
        InputFieldAttack.text = this.WeaponSelected.Attack.ToString();
        InputFieldPrice.text = this.WeaponSelected.Price.ToString();
        ButtonEdit.interactable = true;
        ButtonDelete.interactable = true;
    }


    protected void SetUIToWeapon()
    {
        try
        {
            this.WeaponSelected.Name = InputFieldName.text;
            this.WeaponSelected.Attack = int.Parse(InputFieldAttack.text);
            this.WeaponSelected.Price = double.Parse(InputFieldPrice.text);
        }
        catch (Exception e)
        {
            TextMessage.text = $"Error: {e.Message}";
        }
    }

    protected void UpdateWeapon()
    {
        try
        {
            SetUIToWeapon();
            if (GamesCodeDataSource.Instance.WeaponDAO.UpdateWeapon(this.WeaponSelected))
            {
                TextMessage.text = "Weapon updated!";
                SetCancelMode();
            }
            else
                TextMessage.text = "Weapon not updated!";
        }
        catch (Exception e)
        {
            TextMessage.text = $"Error: {e.Message}";
        }
    }

    protected void DeleteWeapon()
    {
        if (this.WeaponSelected == null)
            return;

        try
        {
            if (GamesCodeDataSource.Instance.WeaponDAO.DeleteWeapon(this.WeaponSelected.Id))
            {
                this.weaponsResultSearch.Remove(this.WeaponSelected);
                var x = ScrollContent.GetComponentsInChildren<Text>().ToList().Find(t => t.text == this.WeaponSelected.ToString());
                if (x != null)
                    Destroy(x.transform.parent.gameObject);
                TextMessage.text = "Weapon deleted!";
                
            }
            else
                TextMessage.text = "Weapon not removed!";
        }
        catch (Exception e)
        {
            TextMessage.text = $"Error: {e.Message}";
        }

    }
}
