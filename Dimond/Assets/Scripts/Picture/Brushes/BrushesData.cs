using UnityEngine;
using System.Collections;

public enum BrushesType
{
    Stone,
    StrandSeam,
    Drawing
}

[CreateAssetMenu(menuName = "Play Objects /Brush", fileName = "New Brush")]
public class BrushesData : ScriptableObject
{
    [Tooltip("Brush's id ")]
    [SerializeField]
    private int brushId;
    public int ID { get { return brushId; } }
    [Tooltip("Brush's texture which display its")]
    [SerializeField]
    private Sprite brushView;
    public Sprite BrushView { get { return brushView; } }

    [Tooltip("Main brushes texture which will change its color")]
    [SerializeField]
    private Sprite brushMask;
    public Sprite BrushMask { get { return brushMask; } }

    [Tooltip("Setting brush'e type")]
    [SerializeField]
    private BrushesType type;
    public BrushesType Type { get { return type; } }

    [SerializeField]
    private bool isFreeBrush;
    public bool IsFreeBrush { get { return isFreeBrush; }}

    [SerializeField]
    private int brushPrice;
    public int BrushPrice { get { return brushPrice; } }
}
