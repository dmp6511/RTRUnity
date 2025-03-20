using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class QuestionManger : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // assign UI Elements
        // need: Array for questions (each question needs a class)
        // array for choices buttons
        // string for additional info
        // string for score label
        // string for question number


        // fetch the questions.json
    }


        [System.Serializable]
        // Question Class
        public class Question {
        public string Q;

        // array for possible answers
        //public Answers[] answers;
        public string correctAnswer;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
