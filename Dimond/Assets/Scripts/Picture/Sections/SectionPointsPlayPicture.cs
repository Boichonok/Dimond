using UnityEngine;
using System.Collections;

public class SectionPointsPlayPicture : MonoBehaviour
{
    [SerializeField]
    private SectionData data = null;

    [SerializeField]
    private bool isPainted;
    public bool IsPainted { get { return isPainted; }}

    [SerializeField]
    private bool isSelected;
    public bool IsSelected { get { return isSelected; }}

    [SerializeField]
    private int brushId = -1;
    public int BrushId { get { return brushId; }}

    [SerializeField]
    private Color sectionColor;
    public Color SectionColor { get { return sectionColor; }}

    private SpriteRenderer PointView;
    private TextMesh text;
    private SpriteRenderer mask;
    private float brushPointViewSizeX = 0.0f;
    private float brushPointViewSizeY = 0.0f;
    private float brushMaskSizeX = 0.0f;
    private float brushMaskSizeY = 0.0f;
    // Use this for initialization
    void Start()
    {
        text = GetComponentInChildren<TextMesh>();
        text.text = data.SectionNumber.ToString();
        mask = gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        PointView = GetComponent<SpriteRenderer>();
        PointView.color = Color.white;
        isPainted = false;
    }

    public void SelectPoint()
    {
        PointView.color = Color.grey;
        isSelected = true;
    }

    public void UnSelectedPoint()
    {
        PointView.color = Color.white;
        isSelected = false;
    }
    public int ToColor(Brush brush)
    {
        brushId = brush.GetBrushId();
        PointView.sprite = brush.GetBrushSpriteView();
        sectionColor = new Color(brush.GetBrushCurrentColor().r, brush.GetBrushCurrentColor().g, brush.GetBrushCurrentColor().b, 255.0f);
        PointView.color = sectionColor;
        mask.sprite = brush.GetBrushSpriteMask();
        mask.color = brush.GetBrushCurrentColor();
        mask.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        mask.size = new Vector2(0.2f, 0.2f);
        text.gameObject.SetActive(false);
        isPainted = true;
        return 1;
    }

}
