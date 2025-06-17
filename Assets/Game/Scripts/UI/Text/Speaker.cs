using UnityEngine;

[CreateAssetMenu(fileName = "New Speaker", menuName = "Dialogue System/Speaker")]
public class Speaker : ScriptableObject
{
    [field: SerializeField] public string SpeakerName { get; private set; }
    [SerializeField] private Color _nameColor;

    public string HexNameColor => ColorUtility.ToHtmlStringRGB(_nameColor);
}