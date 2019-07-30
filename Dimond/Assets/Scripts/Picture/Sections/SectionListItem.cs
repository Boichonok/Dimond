using UnityEngine;
using System.Collections;

public class SectionListItem : MonoBehaviour
{
    [SerializeField]
    private SectionData data;
    //public SectionData Data{ get { return data; } set { data = value; }}

    public void InitSection(SectionData data)
    {
        this.data = data;
        GetComponentInChildren<TextMesh>().text = data.name;
        GetComponent<SpriteRenderer>().color = data.Color;
    }

    public Color GetColor()
    {
        return data.Color;
    }

    public int SectionNumber()
    {
        return data.SectionNumber;

    }
}
