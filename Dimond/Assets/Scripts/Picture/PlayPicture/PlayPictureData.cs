using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Play Objects /Picture", fileName = "New Picture")]
public class PlayPictureData : ScriptableObject
{

    [Tooltip("Picture ID for saving in DataBase")]
    [SerializeField]
    private int pictureId;
    public int PitureId{ get { return pictureId; }}

    [Tooltip("Picture material")]
    [SerializeField]
    private PictureMaterial material = PictureMaterial.Stounes;
    public PictureMaterial Material { get { return material; } }

    [Tooltip("Can change background in game?")]
    [SerializeField]
    private bool canChangeBackground = false;
    public bool CanChangeBackground { get { return canChangeBackground; } }

    [Tooltip("Picture object for instance on scene")]
    [SerializeField]
    private GameObject picturePlayObject = null;
    public GameObject PicturePlayObject { get {return picturePlayObject;}}

    [Tooltip("Ampunt of section on the Picture (For coloring picture")]
    [SerializeField]
    private SectionData[] amountSectionsType = null;
    public SectionData[] AmountSectionsType { get { return amountSectionsType; }}

    [Tooltip("All picture Brushes")]
    [SerializeField]
    private BrushesData[] brushes = null;
    public BrushesData[] Brushes{ get { return brushes; }}

}
