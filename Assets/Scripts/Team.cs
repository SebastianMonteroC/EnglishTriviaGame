using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public string teamName;
    public int score;
    public List<string> powerUps;

    public Team(string name){
        this.teamName = name;
        score = 0;
        powerUps = new List<string>();
    }

    public Team(string name, int score, List<string> powerUps) {
        this.teamName = name;
        this.score = score;
        this.powerUps = powerUps;
    }
}
