using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PowerUp
{
    private static string[] POWER_UPS = {"Double Points", "Change Question", "Re-Spin", "Sabotage"};
    private static string[] HANDICAP_POWER_UPS = {"Triple Points", "Steal Point", "Sabotage", "Turn Skip"};

    public string GetRegularPowerUp() {
        Random random = new Random();
        int randomIndex = random.Next(4);
        return POWER_UPS[randomIndex];
    }

    public string GetHandicapPowerUp() {
        Random random = new Random();
        int randomIndex = random.Next(4);
        return HANDICAP_POWER_UPS[randomIndex];
    }
}
