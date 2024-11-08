using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MathProblem
{
    public string question;
    public int answer;
}

public class Ene_MathProblemManager : MonoBehaviour
{
    public List<MathProblem> mathProblems;

    void Awake()
    {
        mathProblems = new List<MathProblem>()
    {
        new MathProblem { question = "11 + 7", answer = 18 },
        new MathProblem { question = "14 * 5", answer = 70 },
        new MathProblem { question = "21 - 13", answer = 8 },
        new MathProblem { question = "24 / 6", answer = 4 },
        new MathProblem { question = "13 + 9", answer = 22 },
        new MathProblem { question = "16 * 8", answer = 128 },
        new MathProblem { question = "26 - 17", answer = 9 },
        new MathProblem { question = "30 / 10", answer = 3 },
        new MathProblem { question = "17 + 15", answer = 32 },
        new MathProblem { question = "18 * 9", answer = 162 },
        new MathProblem { question = "35 - 19", answer = 16 },
        new MathProblem { question = "40 / 8", answer = 5 },
        new MathProblem { question = "19 + 21", answer = 40 },
        new MathProblem { question = "38 - 23", answer = 15 },
        new MathProblem { question = "45 / 9", answer = 5 },
        new MathProblem { question = "25 + 27", answer = 52 },
        new MathProblem { question = "47 - 31", answer = 16 },
        new MathProblem { question = "54 / 9", answer = 6 },
        new MathProblem { question = "29 + 33", answer = 62 },
        new MathProblem { question = "51 - 37", answer = 14 },
        new MathProblem { question = "60 / 12", answer = 5 },
        new MathProblem { question = "31 + 39", answer = 70 },
        new MathProblem { question = "55 - 41", answer = 14 },
        new MathProblem { question = "66 / 11", answer = 6 },
        new MathProblem { question = "37 + 41", answer = 78 },
        new MathProblem { question = "63 - 47", answer = 16 },
        new MathProblem { question = "70 / 14", answer = 5 },
        new MathProblem { question = "43 + 47", answer = 90 },
        new MathProblem { question = "72 - 55", answer = 17 },
        new MathProblem { question = "80 / 16", answer = 5 },
        new MathProblem { question = "49 + 51", answer = 100 },
        new MathProblem { question = "75 - 59", answer = 16 },
        new MathProblem { question = "90 / 18", answer = 5 }
    };

        Debug.Log("Math problems initialized with count: " + mathProblems.Count);
    }

    public MathProblem GetRandomMathProblem()
    {

        int randomIndex = Random.Range(0, mathProblems.Count);
        Debug.Log("Randomly selected problem index: " + randomIndex);

        return mathProblems[randomIndex];
    }

}
