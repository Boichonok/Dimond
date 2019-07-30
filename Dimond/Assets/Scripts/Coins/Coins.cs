using UnityEngine;
using System.Collections;

public class Coins : MonoBehaviour
{
    private static int currentCoin = 0;
    public  int CurrentCoin { get { return currentCoin; }}
    private  int maxCoin = 1000;
   
    public void AddCoin(int amount)
    {

        currentCoin += amount;
        if (currentCoin > maxCoin || amount > maxCoin)
            currentCoin = maxCoin;
    }

    public void RemoveCoin(int amount)
    {

        currentCoin -= amount;
        if (currentCoin < 0)
            currentCoin = 0;
    }
}
