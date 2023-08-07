using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PowerUp
{
    private static string[] POWER_UPS = {"Double Points", "Change Question", "Re-Spin", "Sabotage"};
    private static string[] HANDICAP_POWER_UPS = {"Triple Points", "Steal Point", "Sabotage"};
    private static Random random = new Random();

    public string GetRegularPowerUp() {
        int randomIndex = random.Next(4);
        return POWER_UPS[randomIndex];
    }

    public string GetHandicapPowerUp() {
        int randomIndex = random.Next(4);
        return HANDICAP_POWER_UPS[randomIndex];
    }
}
