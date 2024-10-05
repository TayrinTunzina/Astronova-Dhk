using System; 
using System.Collections.Generic; 
[Serializable]
public class Question
{
    public string question;  
    public List<string> answers;  
    public int correctAnswerIndex;  
}

[Serializable]
public class QuestionList
{
    public List<Question> questions;
}

