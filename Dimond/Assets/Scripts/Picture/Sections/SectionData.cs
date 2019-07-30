using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Play Objects /Section", fileName = "New Section")]
public class SectionData : ScriptableObject
{
    [SerializeField]
    private int sectionNumber;
    public int SectionNumber { get { return sectionNumber; }}

    [SerializeField]
    private Color color;
    public Color Color { get { return color; }}
}
