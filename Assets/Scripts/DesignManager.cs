using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DesignManager
{
    private GameManager _gameManager;
    private Label _scoreLabel;
    private VisualElement _winContent;
    private Button _playAgainBtn;

    public void Init(UIDocument uiDoc, GameManager gameManager)
    {
        _gameManager = gameManager;

        _scoreLabel = uiDoc.rootVisualElement.Q<Label>("Score");
        _winContent = uiDoc.rootVisualElement.Q<VisualElement>("WinContent");

        _playAgainBtn = uiDoc.rootVisualElement.Q<Button>("PlayAgain");
        _playAgainBtn.clicked += () => _gameManager.OnPlayAgain();
        _playAgainBtn.clicked += () => OnPlayAgain();
    }

    public void OnPointsUpdate(int points)
    {
        _scoreLabel.text = "Punkty: " + points.ToString("D2");
    }

    public void OnWin()
    {
        _winContent.visible = true;
    }

    private void OnPlayAgain()
    {
        _winContent.visible = false;
        _scoreLabel.text = "Punkty: 00";
    }
}
