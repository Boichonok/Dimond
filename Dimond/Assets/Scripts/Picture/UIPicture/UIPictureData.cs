using UnityEngine;
using System.Collections;


[CreateAssetMenu(menuName = "ListView Items/Picture", fileName = "New Picture")]
public class UIPictureData : ScriptableObject
{
    [Tooltip("Picture ID for saving in DataBase")]
    [SerializeField]
    private int pictureId;
    public int PitureId { get { return pictureId; } }

    [Tooltip("Picture view on UI in List View")]
    [SerializeField]
    private Sprite image = null;
    public Sprite Image { get { return image; } set { image = value; }}


    [Tooltip("Picture prise")]
    [SerializeField]
    private int prise = 0;
    public int Prise { get { return prise; } }

    [SerializeField]
    private PictureType pictureType = PictureType.LockedPicture;
    public PictureType PictureType { get { return pictureType; }}

    public UIPictureData(int pictureId, Sprite Image, int prise, PictureType pictureType)
    {
        this.pictureId = pictureId;
        this.Image = Image;
        this.prise = prise;
        this.pictureType = PictureType;
    }

}
