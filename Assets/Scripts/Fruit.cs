using TMPro;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int number;
    public TextMeshProUGUI numberText;

    private bool isSelected = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetNumber(int value)
    {
        number = value;
        numberText.text = number.ToString();
    }

    public void ToggleSelection()
    {
        isSelected = !isSelected;
        spriteRenderer.color = isSelected ? Color.yellow : Color.white;
    }

    public bool IsSelected()
    {
        return isSelected;
    }
}
