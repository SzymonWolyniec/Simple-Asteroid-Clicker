using UnityEngine.UIElements;

public class DesignManager
{
    private GameManager _gameManager;

    // Zmienne przechowujące referencję do elementów interfejsu użytkownika
    private Label _scoreLabel;
    private VisualElement _winContent;
    private Button _playAgainBtn;

    // Konstruktor klasy DesignManager przyjmujący jako argumenty dokument UI i referencję do klasy GameManager
    public DesignManager(UIDocument uiDoc, GameManager gameManager)
    {
        _gameManager = gameManager;

        // Przypisujemy referencje do elementów interfejsu użytkownika do odpowiednich zmiennych
        _scoreLabel = uiDoc.rootVisualElement.Q<Label>("Score");
        _winContent = uiDoc.rootVisualElement.Q<VisualElement>("WinContent");
        _playAgainBtn = uiDoc.rootVisualElement.Q<Button>("PlayAgain");

        // Dodajemy metodę OnPlayAgain jako reakcję na kliknięcie przycisku _playAgainBtn
        _playAgainBtn.clicked += _gameManager.OnPlayAgain;
        _playAgainBtn.clicked += OnPlayAgain;
    }

    // Metoda SetScoreLabel ustawiająca tekst w _scoreLabel na wartość punktów
    public void SetScoreLabel(int points)
    {
        _scoreLabel.text = "Punkty: " + points.ToString("D2");
    }

    // Metoda OnWin ustawiająca wartość visible na true dla _winContent
    public void OnWin()
    {
        _winContent.visible = true;
    }

    // Prywatna metoda OnPlayAgain, która ustawia wartość visible na false dla _winContent i resetuje wartość w _scoreLabel
    private void OnPlayAgain()
    {
        _winContent.visible = false;
        _scoreLabel.text = "Punkty: 00";
    }
}
