using System;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    public int points;

    public void Start()
    {
        points = 300;
        UIManager.instance.UpdatePoints(points);
    }

    public void AddPoints(int amount)
    {
        points += amount;
    }

    public bool SpendPoints(int amount)
    {
        if (points >= amount)
        {
            points -= amount;
            return true;
        }
        return false;
    }
}
