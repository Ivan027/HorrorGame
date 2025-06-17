using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TextButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
    
    public void AddOnClickListener(UnityAction action)
    {
        _button?.onClick.AddListener(action);
    }
    
    public void RemoveOnClickListener(UnityAction action)
    {
        _button?.onClick.RemoveListener(action);
    }

    public void ClearOnClickListeners()
    {
        _button?.onClick.RemoveAllListeners();
    }
}
