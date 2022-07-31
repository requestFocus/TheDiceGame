using UnityEngine;

public class NewBankBalanceGenerator
{
    private const string BALANCE = "Balance";
    
    public void SetNewBankBalance(int newBankBalanceAmount)
    {
        PlayerPrefs.SetInt(BALANCE, newBankBalanceAmount);
    }

    public int GenerateNewBankBalance()
    {
        return Random.Range(200, 800);
    }
}