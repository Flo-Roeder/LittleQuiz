using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "question")]
public class QuestionScriptables : ScriptableObject
{
    public string Question { get { return _question; } }
    [SerializeField] private string _question;

    public string[] FalseAnswer { get { return _falseAnswer; } }
    [SerializeField] private string[] _falseAnswer;

    public string[] CorrectAnswer { get { return _correctAnswer; } }
    [SerializeField] private string[] _correctAnswer;
}
