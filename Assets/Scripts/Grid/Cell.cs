using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [HideInInspector]
    public int index_x;

    [HideInInspector]
    public int index_y;

    [HideInInspector]
    public static float cellSize;

    [SerializeField]
    private TextMeshProUGUI positionUI;

    [HideInInspector]
    public bool isBlocked = false;

    private void Awake()
    {
        // set the cell size based on scale
        cellSize = transform.localScale.x;
    }

    void Start()
    {
        //set the UI text to display the cell's position
        positionUI.text = $"({index_x},{index_y})";
    }

    //Enables the UI element
    public void EnableUI()
    {
        positionUI.enabled = true;
    }

    //disable the UI element
    public void DisableUI() 
    {
        positionUI.enabled = false;
    }
}
