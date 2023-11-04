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

    public void OnUpdateUi(int? levelIndex, int? score)
    {
        Debug.Log("update ui");
        if (levelIndex != null)
        {
            labelLevel.text = $"Level {levelIndex + 1}";
        }

        if (score != null)
        {
            labelScore.text = $"Score: {score}";
        }
    }
}
