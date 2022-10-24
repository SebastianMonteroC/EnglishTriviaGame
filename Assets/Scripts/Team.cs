using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    public string teamName;
    public int score;

    public Team(string name){
        this.teamName = name;
        score = 0;
    }
}
