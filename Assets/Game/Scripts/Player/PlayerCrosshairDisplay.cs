using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCrosshairDisplay : MonoBehaviour
{
    [SerializeField] private Image _crosshairImage;
    [SerializeField] private TextMeshProUGUI _textUnderCrosshair;

    private Sprite _startCrosshair;
    private float _startCrosshairSize;

    private void Awake()
    {
        _startCrosshair = _crosshairImage.sprite;
        _startCrosshairSize = _crosshairImage.rectTransform.sizeDelta.x;
        
        HideText();
    }

    #region Crosshair

    public void HideCrosshair()
    {
        _crosshairImage.gameObject.SetActive(false);
    }

    public void ShowCrosshair()
    {
        _crosshairImage.gameObject.SetActive(true);
    }

    public void SetCrosshair(Sprite crosshair)
    {
        if (crosshair != null && _crosshairImage.sprite != crosshair)
            _crosshairImage.sprite = crosshair;
    }

    public void SetCrosshair(Sprite crosshair, Color color, float size = -1f)
    {
        size = Mathf.Approximately(size, -1f) ? _startCrosshairSize : size;

        SetCrosshair(crosshair);
        _crosshairImage.color = color;
        _crosshairImage.rectTransform.sizeDelta = new Vector2(size, size);
    }

    public void ReturnDefaultCrosshair()
    {
        SetCrosshair(_startCrosshair, Color.white);
    }

    #endregion

    #region TextUnderCrosshair

    public void HideText()
    {
        _textUnderCrosshair.gameObject.SetActive(false);
    }

    public void ShowText(string text)
    {
        _textUnderCrosshair.text = text;
        _textUnderCrosshair.gameObject.SetActive(true);
    }

    #endregion
}