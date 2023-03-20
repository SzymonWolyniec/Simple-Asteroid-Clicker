using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DesignManager
{
    private Label _scoreLabel;

    public void Init(UIDocument uiDoc)
    {
        _scoreLabel = uiDoc.rootVisualElement.Q<Label>("Score");
    }

    public void OnPointsUpdate(int points)
    {
        _scoreLabel.text = "Punkty: " + points.ToString("D2");
    }
}
