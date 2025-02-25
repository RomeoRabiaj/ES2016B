﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Class that abstracts Unit Shopping
 */

public class UnitShop : MonoBehaviour
{
    // bool to  exchange the two respawn points
    private bool respawnExchange;
    public Transform[] respawns;
    public List<GameObject> units;

    // Must be queried since it could change
    public List<Unit> GetAvailable()
    {
        var availableUnits = new List<Unit>();
        foreach (var o in units)
            availableUnits.Add(o.GetComponent<Unit>());
        return availableUnits;
    }

    public bool IsPurchasable(Unit toPurchase, int numCoins)
    {
        if (GetAvailable().Contains(toPurchase))
            return toPurchase.purchaseCost <= numCoins;
        return false;
    }

    public List<Unit> GetPurchasable(int numCoins)
    {
        return GetAvailable().Where(x => IsPurchasable(x, numCoins)).ToList();
    }

    public void Purchase(Unit toPurchase)
    {
        // TODO do with tags
        var unitToPut = units.Where(x => x.GetComponent<Unit>().Equals(toPurchase)).ToList()[0];

        //we take the respawn points at the same order than gamecontroller
        var respawns = GameObject.FindGameObjectsWithTag("SpawnP");

        //bool respawnExchange helps to distribute enemies in the two spawn points
        if (respawnExchange)
        {
            Instantiate(unitToPut, respawns[GameController.instance.spawnPoint2].transform.position,
                respawns[GameController.instance.spawnPoint2].transform.rotation);
            respawnExchange = false;
        }
        else
        {
            Instantiate(unitToPut, respawns[GameController.instance.spawnPoint1].transform.position,
                respawns[GameController.instance.spawnPoint1].transform.rotation);
            respawnExchange = true;
        }
    }
}