using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    public float scoreAchieved = 10f;
    public string alliacne; //TODO: Make Enum Type

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Note")
        {
            if (alliacne == "Blue")
                ScoreManager.blueScore += scoreAchieved;
            if (alliacne == "Red")
                ScoreManager.redScore += scoreAchieved;

            Destroy(other.gameObject);
        }
    }
}
