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
        new MathProblem { question = "1 + 2", answer = 3 },
        new MathProblem { question = "3 * 6", answer = 18 },
        new MathProblem { question = "9 - 7", answer = 2 },
        new MathProblem { question = "8 / 2", answer = 4 }
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
