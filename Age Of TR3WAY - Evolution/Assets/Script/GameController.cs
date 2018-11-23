using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    static int money = 1000;

    public static void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();
    }

    public static void TakeMoney(int amount)
    {
        if(!TransactionIsValid(amount))
        {
            throw new Exception("Not enough money to perform this transaction !");
        }
        money -= amount;
        UpdateMoneyText();
    }

    private static void UpdateMoneyText()
    {
        var moneyText = GameObject.FindGameObjectWithTag("Money");
        var moneyTextGUI = moneyText.GetComponent<TextMeshProUGUI>();
        moneyTextGUI.text = "TR3COINS : " + money.ToString();
    }

    private static bool TransactionIsValid(int amount)
    {
        return money - amount >= 0;
    }
}
