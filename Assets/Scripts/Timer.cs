using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToAnswerQuestion = 30f;
    [SerializeField] float timeToRevealAnswer = 10f;

    public bool loadNextQuestion;
    public bool isAnsweringQuestion = false;
    public float fillFractions;

    float timerValue;

    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer() {
        timerValue -= Time.deltaTime;

        if (timerValue <= 0) {
            if (isAnsweringQuestion) {
                isAnsweringQuestion = false;
                timerValue = timeToRevealAnswer;
            } else {
                isAnsweringQuestion = true;
                timerValue = timeToAnswerQuestion;
                loadNextQuestion = true;
            }
        } else {
            fillFractions = timerValue / (
                isAnsweringQuestion
                    ? timeToAnswerQuestion
                    : timeToRevealAnswer
            );
        }
    }

    public void CancelTimer() {
        timerValue = 0;
    }
}
