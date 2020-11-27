using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncrementalAndDecrementalController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField input = null;
    [SerializeField]
    private int min = 0;
    [SerializeField]
    private int max = 1;
    [SerializeField]
    private int defaultValue = 0;

    private int value = 0;

    private void Start()
    {
        value = defaultValue;
        setInputText(value);
    }

    public void increment()
    {
        value += 1;
        if (value > max) value = max;
        setInputText(value);
    }

    public void decrement()
    {
        value -= 1;
        if (value < min) value = min;
        setInputText(value);
    }

    private void setInputText(int value)
    {
        input.text = value.ToString();
    }
}
