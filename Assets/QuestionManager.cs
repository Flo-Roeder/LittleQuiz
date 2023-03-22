using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{

    [SerializeField] QuestionScriptables[] questions;
    private List<QuestionScriptables> questionList = new();
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI[] answerTexts= new TextMeshProUGUI[4];

    [SerializeField] GameObject stopRaycastObject; //prevent the player for hitting multiple answers
    [SerializeField] GameObject wrongAnswerResponse,victoryResponse;
    string tempCorrect; //stored for randomizing and for checking if answer was correct

    private void Awake()
    {
        InitiateQuestionList();
    }

    void Start()
    {
        stopRaycastObject.SetActive(false);
        wrongAnswerResponse.SetActive(false);
        victoryResponse.SetActive(false);

        GetRandomQuestion();
    }


    //copy array to list for editing while runtime
    private void InitiateQuestionList()
    {
        for (int i = 0; i < questions.Length; i++)
        {
            questionList.Add(questions[i]);
        }
    }

    //get random question from list
    //prevent getting questions double by removing from list
    private void GetRandomQuestion()
    {
        if (questionList.Count<=0)
        {
            stopRaycastObject.SetActive(true);
            victoryResponse.SetActive(true);
            return;
        }
        int _roll=Random.Range(0,questionList.Count);
        SetQuestion(questionList[_roll]);
        questionList.Remove(questionList[_roll]);
    }

    //get and show the question plus randomized answers
    public void SetQuestion (QuestionScriptables questionScriptables)
    {
        questionText.text = questionScriptables.Question;
        //temporary list to store random answers in 
        //loading in a random correct answer
        List<string> _tempShowings = new();
        tempCorrect = questionScriptables.CorrectAnswer[Random.Range(0, questionScriptables.CorrectAnswer.Length)];
        _tempShowings.Add(tempCorrect);
        //temporary list for getting random wrong answers
        List<string> _tempFalse = questionScriptables.FalseAnswer.ToList<string>();
        //loading in three wrong answers to _tempShowings
        //prevent duplicates
        for (int i = 0; i < 3; i++)
        {
            int _rollQestion = Random.Range(0, _tempFalse.Count);
            _tempShowings.Add(_tempFalse[_rollQestion]);
            _tempFalse.Remove(_tempFalse[_rollQestion]);
        }
        //randomize _tempShowing
        //get the text shown in scene
        //prevent duplicates
        for (int i = 0; i < 4; i++)
        {
            int _rollShowing = Random.Range(0, _tempShowings.Count);
            answerTexts[i].text = _tempShowings[_rollShowing];
            _tempShowings.Remove(_tempShowings[_rollShowing]);

            //answerTexts[i].text = _tempShowings[i];
        }
    }

    //check the answer for correct aor not
    //starting coroutine
    public void CheckAnswer(GameObject buttonObject)
    {
        string _text = buttonObject.GetComponentInChildren<TextMeshProUGUI>().text;
        if (_text == tempCorrect)
        {
            StartCoroutine(RevealAnswer(buttonObject, true));
            //buttonObject.GetComponent<Image>().color = Color.green;
            return;
        }
        StartCoroutine(RevealAnswer(buttonObject, false));
    }

    //change color of pushed button depend of its outcome wrong/correct
    private void ShowColor(GameObject buttonObject,bool isCorrect)
    {
        if (isCorrect) 
        {
            buttonObject.GetComponent<Image>().color = Color.green; 
            return;
        }
        buttonObject.GetComponent<Image>().color = Color.red;
    }

    //progress in the game depend of the answers outcome
    private void GoingOn(GameObject buttonObject,bool isCorrect)
    {
        if (isCorrect)
        {
            buttonObject.GetComponent<Image>().color = Color.white;
            GetRandomQuestion();
            stopRaycastObject.SetActive(false);
            return;
        }
        wrongAnswerResponse.SetActive(true);
    }

    //coroutine to time the steps after taking an answer
    private IEnumerator RevealAnswer(GameObject buttonObject, bool isCorrect)
    {
        stopRaycastObject.SetActive(true);
        yield return new WaitForSeconds(1);
        ShowColor(buttonObject, isCorrect);
        yield return new WaitForSeconds(1);
        GoingOn(buttonObject, isCorrect);
    }

}
