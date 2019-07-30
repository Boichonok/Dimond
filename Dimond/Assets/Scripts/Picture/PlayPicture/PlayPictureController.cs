using UnityEngine;
using System.Collections;

public class PlayPictureController : MonoBehaviour
{
    private PlayPictureData data;
    public PlayPictureData Data { get; private set; }

    [SerializeField]
    private Camera playCamera;

    private GameObject[] allSections;
    private SectionPointsPlayPicture[] allSectionsPoints;

    private Brush brush = null;
    public Brush Brush { get { return brush; } set { brush = value; } }

    private IPlayPictureControllerCalbacks callbacks;

    public int backgroundID { get; set; }

    private int lastSelectedSection = 0;

    private float delayTime = 1.0f;

    private float startDTime = 0.0f;

    private int countColorPoint = 0;

    private int AmountPointsInSections = 0;
    // Use this for initialization
    void Start()
    {
        allSections = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            allSections[i] = transform.GetChild(i).gameObject;
        }
        AmountPointsInSections = CountAllPoints();
        allSectionsPoints = GetAllPlaySectionsPoints();
    }

    public void InitPlayPicture(PlayPictureData data, Camera camera)
    {
        this.data = data;
        this.playCamera = camera;
    }

    public void InitCallbacks(IPlayPictureControllerCalbacks callbacks)
    {
        this.callbacks = callbacks;
    }

    public void SelectSection(int sectionNumber)
    {
        UnSelectedSection();
        for (int i = 0; i < allSections[sectionNumber - 1].transform.childCount; i++)
        {
            var point = allSections[sectionNumber - 1].transform.GetChild(i).GetComponent<SectionPointsPlayPicture>();
            if (!point.IsPainted)
            {
                point.SelectPoint();
            }
        }
        lastSelectedSection = sectionNumber - 1;
    }

    private void UnSelectedSection()
    {
        for (int i = 0; i < allSections[lastSelectedSection].transform.childCount; i++)
        {
            var point = allSections[lastSelectedSection].transform.GetChild(i).GetComponent<SectionPointsPlayPicture>();
            if (!point.IsPainted)
            {
                point.UnSelectedPoint();
            }
        }
    }

    private void FixedUpdate()
    {
        OnTap();
        OnHold();
        CountColorPointsInPercents();
    }

    private void OnTap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var selectedPoint = ClickPicturePoint();
            if (selectedPoint != null)
            {
                if (selectedPoint.IsSelected && !selectedPoint.IsPainted && brush != null)
                {
                    countColorPoint = countColorPoint + selectedPoint.ToColor(brush);
                    callbacks.OnTupColorFinished();
                }
            }
        }
    }



    private void OnHold()
    {
        if (Input.GetMouseButtonDown(0))
            startDTime = Time.time;
        if (Input.GetMouseButton(0))
        {
            if (startDTime + delayTime <= Time.time)
            {
                playCamera.GetComponent<PlayPictureZoomController>().CanSwipe = false;
                var selectedPoint = ClickPicturePoint();
                if (selectedPoint != null)
                {
                    if (selectedPoint.IsSelected && !selectedPoint.IsPainted && brush != null)
                    {
                        countColorPoint = countColorPoint + selectedPoint.ToColor(brush);
                        callbacks.OnHoldColorFinished();
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
            playCamera.GetComponent<PlayPictureZoomController>().CanSwipe = true;

    }


    private SectionPointsPlayPicture ClickPicturePoint()
    {

        RaycastHit2D hit = Physics2D.Raycast(playCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            var point = hit.collider.GetComponent<SectionPointsPlayPicture>();
            if (point != null)
                return point;
        }
        return null;
    }

    private int CountAllPoints()
    {
        var allPoints = 0;
        for (int i = 0; i < allSections.Length; i++)
        {
            allPoints = allPoints + allSections[i].transform.childCount;
        }
        return allPoints;
    }


    public int CountColorPointsInPercents()
    {
        return (100 * countColorPoint) / (AmountPointsInSections);
    }

    public void ColorFullLastSectionPoints()
    {
        if (lastSelectedSection >= 0)
        {
            for (int i = 0; i < allSections[lastSelectedSection].transform.childCount; i++)
            {
                var point = allSections[lastSelectedSection].transform.GetChild(i).GetComponent<SectionPointsPlayPicture>();
                if (!point.IsPainted)
                {
                    countColorPoint = countColorPoint + point.ToColor(brush);
                }
            }
        }
    }

    /* public bool[] GetAllPlaySections()
     {
         bool[] picturePoints = new bool[AmountPointsInSections];
         int k = 0;
         for (int i = 0; i < allSections.Length; i ++)
         {
             for (int j = 0; j < allSections[i].transform.childCount; j++)
             {
                 if(allSections[i].transform.GetChild(j).GetComponent<SectionPlayPicture>().IsPainted)
                 {
                     picturePoints[k] = true;
                 }
                 else
                 {
                     picturePoints[k] = false;
                 }
                 k++;
             }
         }
         return picturePoints;

     }*/
    public SectionPointsPlayPicture[] GetAllPlaySectionsPoints()
    {
        SectionPointsPlayPicture[] picturePoints = new SectionPointsPlayPicture[AmountPointsInSections];
        int k = 0;
        for (int i = 0; i < allSections.Length; i++)
        {
            for (int j = 0; j < allSections[i].transform.childCount; j++)
            {
                picturePoints[k] = allSections[i].transform.GetChild(j).GetComponent<SectionPointsPlayPicture>();
                k++;
            }
        }
        return picturePoints;

    }

    public void ReColorSectionPoint(int pointNumber)
    {
        countColorPoint = countColorPoint + allSectionsPoints[pointNumber].ToColor(brush);
    }
}
