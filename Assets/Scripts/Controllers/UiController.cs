using UnityEngine;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{
    private Label labelLevel;
    private Label labelScore;

    public void OnEnable()
    {
        // Bind ui elements to variables
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        labelLevel = root.Q<Label>("LabelLevel");
        labelScore = root.Q<Label>("LabelScore");
    }

    public void Start()
    {
        // Subscribe to events
        EventSystem.current.OnUpdateUi += OnUpdateUi;
    }

    // Update the top labels to show a specific score and level number, or pass -1 level index to show end screen text
    public void OnUpdateUi(int? levelIndex, int? score)
    {
        if (levelIndex != null)
        {
            if (levelIndex != -1)
            {
                labelLevel.text = $"Level {levelIndex + 1}";
            }
            else
            {
                labelLevel.text = "You win!";
            }
        }

        if (score != null)
        {
            if (levelIndex != -1)
            {
                labelScore.text = $"Level Score: {score}";
            }
            else
            {
                labelScore.text = $"Total Score: {score}";
            }
        }
    }
}
