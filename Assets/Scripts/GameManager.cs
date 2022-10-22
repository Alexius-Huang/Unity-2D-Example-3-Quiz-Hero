using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Quiz quiz;
    EndScreen endScreen;

    void Awake() {
        quiz = FindObjectOfType<Quiz>();
        endScreen = FindObjectOfType<EndScreen>();
    }

    void Start()
    {
        LoadQuizScene();
    }

    void Update()
    {
        if (quiz.isComplete) {
            LoadEndScene();
        }
    }

    void LoadQuizScene() {
        quiz.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);
    }

    void LoadEndScene() {
        quiz.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(true);
        endScreen.ShowFinalScore();
    }

    public void PlayAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
