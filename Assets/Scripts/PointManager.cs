using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;

    public int totalPoints = 0;
    public TextMeshProUGUI pointsText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int amount) {
        totalPoints += amount;
        if(pointsText != null) {
            UpdatePointsUI();
        }
    }

    private void UpdatePointsUI() {
        if(pointsText != null) {
            pointsText.text = $"Points: {totalPoints}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
