using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour
{
    public Text questionText;  
    public Button[] answerButtons;  

    private QuestionList questionList;  
    private int currentQuestionIndex = 0;  

    public Transform firePoint;
    public GameObject bulletPrefab;
    public Transform firePointHero;
    public GameObject bulletPrefabHero;
    
    public HealthBar healthBar;
    public HealthBarHero healthBarHero;
    private Color correctColor = Color.green;
    private Color wrongColor = Color.red;
    private Color defaultColor;
    public Text scoreText;      
    private int score = 0;      


    void Start()
    {
        LoadQuestionsFromJson(); 
        DisplayQuestion(currentQuestionIndex);  
        UpdateScoreUI();
        defaultColor = answerButtons[0].GetComponent<Image>().color;
    }

    void LoadQuestionsFromJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("questions");
        if (jsonFile != null)
        {
            questionList = JsonUtility.FromJson<QuestionList>(jsonFile.text); 
        }
        else
        {
            Debug.LogError("JSON file not found!");
        }
    }

    void DisplayQuestion(int index)
    {
        if (index >= 0 && index < questionList.questions.Count)
        {
            Question currentQuestion = questionList.questions[index];
            questionText.text = currentQuestion.question;  
            
            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].GetComponentInChildren<Text>().text = currentQuestion.answers[i];
                int capturedIndex = i;  
                answerButtons[i].onClick.RemoveAllListeners();  
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(capturedIndex));  
            }
        }
    }
    
    public bool IsCorrectAnswer(int answerIndex)
    {
        return questionList.questions[currentQuestionIndex].correctAnswerIndex == answerIndex;
    }

    public void OnAnswerSelected(int answerIndex)
    {
        if (IsCorrectAnswer(answerIndex))
        {
            Debug.Log("Correct! Hero shoots.");
            Instantiate(bulletPrefabHero, firePointHero.position, firePointHero.rotation);
            
            score++;
            UpdateScoreUI();
            healthBar.DecreaseHealthWithDelay(2.4f, 1);
            StartCoroutine(HandleAnswerFeedback(answerButtons[answerIndex], true));
        }
        else
        {
            Debug.Log("Wrong! Enemy shoots.");
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            healthBarHero.DecreaseHealthWithDelay(2.4f, 1);
            StartCoroutine(HandleAnswerFeedback(answerButtons[answerIndex], false));
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score " + score.ToString();
    }

    IEnumerator HandleAnswerFeedback(Button selectedButton, bool isCorrect)
    {
        if (isCorrect)
        {
            selectedButton.GetComponent<Image>().color = correctColor; 
        }
        else
        {
            selectedButton.GetComponent<Image>().color = wrongColor; 
        }

        yield return new WaitForSeconds(2.4f);

        selectedButton.GetComponent<Image>().color = defaultColor;

        currentQuestionIndex++;
        if (currentQuestionIndex < questionList.questions.Count)
        {
            DisplayQuestion(currentQuestionIndex);
        }
        else
        {
            Debug.Log("No more questions.");
        }
    }
}
