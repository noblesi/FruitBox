using TMPro;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int number;
    public TextMeshProUGUI numberText;

    public void SetNumber(int value)
    {
        number = value;
        numberText.text = number.ToString();
    }
}
