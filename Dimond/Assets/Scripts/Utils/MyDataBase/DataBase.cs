using UnityEngine;
using System.Collections;
using System.IO;

public class DataBase : MonoBehaviour
{
    private PictureTable pictureTable;
    private BrushTable brushTable;
    private GameSettings gameSettings;
    private string fileNameDBPictureTable;
    private string fileNameDBBrushTable;
    private string fileNameGS;

    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine("Init");
    }

    private IEnumerator Init()
    {
        //fileNameDBPictureTable = string.Format("{0}/PictureTable.mdb", Application.persistentDataPath);
        //fileNameGS = string.Format("{0}/GameSettings.gs", Application.persistentDataPath);
        //fileNameDBBrushTable = string.Format("{0}/BrushTable.mdb", Application.persistentDataPath);
        fileNameDBPictureTable = string.Format("/Users/alexboicko/Documents/com.yovogames.diamands/Dimond/Assets/Resources/PictureTable.mdb");
        fileNameGS = string.Format("/Users/alexboicko/Documents/com.yovogames.diamands/Dimond/Assets/Resources/GameSettings.gs");
        fileNameDBBrushTable = string.Format("/Users/alexboicko/Documents/com.yovogames.diamands/Dimond/Assets/Resources/BrushTable.mdb");

        pictureTable = CreateTable();
        brushTable = CreateBrushTable();
        gameSettings = LoadSettings();
        yield return null;
    }

    public void UpdatePictureTable(int id, PictureMaterial pictureMaterial, bool isBuying, bool isFirstSelect, int drawingProgress, PictureTable.SectionPlayObjectProxy[] sections, byte[] pictureInBytes, byte[] defoultPictureInBytes, TextureFormat textureFormat, TextureFormat defaultPictureFormat, int backgroundID)
    {
        pictureTable.AddRow(id, pictureMaterial, isBuying, isFirstSelect, drawingProgress, sections, pictureInBytes, defoultPictureInBytes, textureFormat, defaultPictureFormat, backgroundID);
        var tableInJson = JsonUtility.ToJson(pictureTable);
        File.WriteAllText(fileNameDBPictureTable, tableInJson);
    }

    public bool ResetRowFromPictureTableByID(int id, byte[] pictureInBytes, TextureFormat textureFormat)
    {
        pictureTable.AddRow(id, PictureMaterial.NONE, true, true, 0, null, pictureInBytes, pictureInBytes, textureFormat, textureFormat, 0);
        var tableInJson = JsonUtility.ToJson(pictureTable);
        File.WriteAllText(fileNameDBPictureTable, tableInJson);
        return true;
    }

    public void UpdateBrushTable(int id, bool isFreeBrush, BrushesType material)
    {
        brushTable.AddRow(id, isFreeBrush, material);
        var tableInJson = JsonUtility.ToJson(brushTable);
        File.WriteAllText(fileNameDBBrushTable, tableInJson);
    }

    public GameSettings GetSettings()
    {
        return gameSettings;
    }

    public void SaveSettings()
    {
        if (gameSettings != null)
        {
            var gameSettingsInJson = gameSettings.SaveToJson();
            File.WriteAllText(fileNameGS, gameSettingsInJson);
        }
    }

    private BrushTable CreateBrushTable()
    {
        if (!File.Exists(fileNameDBBrushTable))
        {
            var brushTable_ = new BrushTable();
            brushTable_.InitTable(new System.Collections.Generic.List<BrushTable.BrushProxy>());
            var tableInJson = JsonUtility.ToJson(brushTable_);
            Debug.Log("Was Created Brush Table: " + tableInJson);
            File.WriteAllText(fileNameDBBrushTable, tableInJson);
            return brushTable_;
        }
        else
        {
            return GetBrushTableFromFile();
        }
    }

    public BrushTable GetBrushTableFromFile()
    {
        if (File.Exists(fileNameDBBrushTable))
        {
            var tableInJson = File.ReadAllText(fileNameDBBrushTable);
            return JsonUtility.FromJson<BrushTable>(tableInJson);
        }
        return null;
    }

    private PictureTable CreateTable()
    {
        if (!File.Exists(fileNameDBPictureTable))
        {
            var pictureTable_ = new PictureTable();
            pictureTable_.InitTable(new System.Collections.Generic.List<PictureTable.PictureProxy>());
            var tableInJson = JsonUtility.ToJson(pictureTable_);
            Debug.Log("Was Created Picture Table: " + tableInJson);
            File.WriteAllText(fileNameDBPictureTable, tableInJson);
            return pictureTable_;
        }
        else
        {
            return GetPictureTableFromFile();
        }
    }

    public PictureTable GetPictureTableFromFile()
    {
        if (File.Exists(fileNameDBPictureTable))
        {
            var tableInJson = File.ReadAllText(fileNameDBPictureTable);
            return JsonUtility.FromJson<PictureTable>(tableInJson);
        }
        return null;
    }

    private GameSettings LoadSettings()
    {
        var isExsist = File.Exists(fileNameGS);

        if (!isExsist)
        {
            var gameSettings_ = new GameSettings();
            gameSettings_.CoinsCount = 0;
            gameSettings_.isFirstStartApp = true;
            gameSettings_.isAdvicersActive = true;
            string gameSettingsInJson = gameSettings_.SaveToJson();
            File.WriteAllText(fileNameGS, gameSettingsInJson);
            return gameSettings_;
        }
        else
        {
            string gameSettingsInJson = File.ReadAllText(fileNameGS);
            return JsonUtility.FromJson<GameSettings>(gameSettingsInJson);
        }
    }
}
