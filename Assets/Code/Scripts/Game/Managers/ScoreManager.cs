using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    [Header("Text Settings")]
    public TextMeshProUGUI blueText;
    public TextMeshProUGUI redText;

    [Header("Score Settings")]
    public static float blueScore = 0;
    public static float redScore = 0;

    //TODO: Team Settings

    void Update()
    {
        blueText.text = "Blue Alliacne: " + blueScore;
        redText.text = "Red Alliance: " + redScore;
    }
}
