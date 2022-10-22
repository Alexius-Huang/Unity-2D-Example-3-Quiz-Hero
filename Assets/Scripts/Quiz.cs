using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questionList = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    bool hasAnsweredEarly = true;

    [Header("Button Sprites")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Progress Bar")]
    [SerializeField] Slider progressBar;
    public bool isComplete;

    void Awake() {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    void Start()
    {
        progressBar.maxValue = questionList.Count;
        progressBar.value = 0;
        isComplete = false;
    }

    void Update() {
        timerImage.fillAmount = timer.fillFractions;

        if (timer.loadNextQuestion) {
            if (progressBar.value == progressBar.maxValue) {
                isComplete = true;
                return;
            }
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        } else if (!hasAnsweredEarly && !timer.isAnsweringQuestion) {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    void GetNextQuestion() {
        if (questionList.Count != 0) {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        }
    }

    void GetRandomQuestion() {
        int index = Random.Range(0, questionList.Count);
        currentQuestion = questionList[index];

        if (questionList.Contains(currentQuestion)) {
            questionList.Remove(currentQuestion);
        }
    }

    void DisplayQuestion() {
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++) {
            TextMeshProUGUI tmp = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = currentQuestion.GetAnswer(i);
        }
    }

    void SetButtonState(bool state) {
        for (int i = 0; i < answerButtons.Length; i++) {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprites() {
        for (int i = 0; i < answerButtons.Length; i++) {
            Image image = answerButtons[i].GetComponent<Image>();
            image.sprite = defaultAnswerSprite;
        }
    }

    void DisplayAnswer(int index) {
        int correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
        Image buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
        buttonImage.sprite = correctAnswerSprite;
        if (index == correctAnswerIndex) {
            questionText.text = "Correct!";
            scoreKeeper.IncrementCorrectAnswers();
        } else {
            questionText.text = "Wrong! The correct answer is: " + currentQuestion.GetAnswer(correctAnswerIndex);
        }
    }

    public void OnAnswerSelected(int index) {
        DisplayAnswer(index);
        SetButtonState(false);
        hasAnsweredEarly = true;
        timer.CancelTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }
}
