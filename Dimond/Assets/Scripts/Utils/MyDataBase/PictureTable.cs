using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PictureTable
{
    [System.Serializable]
    public class PictureProxy
    {
        public int id;
        public PictureMaterial pictureMaterial;
        public bool IsFirstSelect;
        public bool IsBuying;
        public int drawingProgress;
        public SectionPlayObjectProxy[] picturePoints;
        public byte[] pictureInBytes;
        public byte[] defoultPictureInBytes;
        public TextureFormat pictureFormat;
        public TextureFormat defaultPictureFormat;
        public int backgroundID;
        public PictureProxy(int id, PictureMaterial pictureMaterial, bool isBuying, bool isFirstSelect, int drawingProgress, SectionPlayObjectProxy[] sections, byte[] pictureInBytes, byte[] defoultPictureInBytes, TextureFormat textureFormat, TextureFormat defaultPictureFormat, int backgroundId)
        {
            this.id = id;
            this.pictureMaterial = pictureMaterial;
            this.IsFirstSelect = isFirstSelect;
            this.IsBuying = isBuying;
            this.picturePoints = sections;
            this.drawingProgress = drawingProgress;
            this.pictureInBytes = pictureInBytes;
            this.defoultPictureInBytes = defoultPictureInBytes;
            this.pictureFormat = textureFormat;
            this.defaultPictureFormat = defaultPictureFormat;
            this.backgroundID = backgroundId;
        }

    }



    [System.Serializable]
    public class SectionPlayObjectProxy
    {
        public int brushId;
        public Color color;
        public bool isPainted;
        public SectionPlayObjectProxy(int brushId, bool isPainted, Color color)
        {
            this.brushId = brushId;
            this.isPainted = isPainted;
            this.color = color;
        }
    }

    public List<PictureProxy> rows;

    public bool InitTable(List<PictureProxy> rows)
    {
        this.rows = rows;
        return true;
    }

    public void AddRow(int id, PictureMaterial pictureMaterial, bool isBuying, bool isFirstSelect, int drawingProgress, SectionPlayObjectProxy[] sectionPoints, byte[] pictureInbytes, byte[] defoultPictureInBytes, TextureFormat textureFormat, TextureFormat defaultPictureFormat, int backgroundId)
    {
        var isExistsRow = rows.Exists((PictureProxy obj) => obj.id == id);
        if (!isExistsRow)
        {
            rows.Add(new PictureProxy(id, pictureMaterial, isBuying, isFirstSelect, drawingProgress, sectionPoints, pictureInbytes, defoultPictureInBytes, textureFormat, defaultPictureFormat, backgroundId));
        }
        else
        {
            var row = rows.Find((PictureProxy obj) => obj.id == id);
            row.pictureMaterial = pictureMaterial;
            row.IsBuying = isBuying;
            row.IsFirstSelect = isFirstSelect;
            row.drawingProgress = drawingProgress;
            row.picturePoints = sectionPoints;
            row.pictureInBytes = pictureInbytes;
            row.defoultPictureInBytes = defoultPictureInBytes;
            row.pictureFormat = textureFormat;
            row.defaultPictureFormat = defaultPictureFormat;
            row.backgroundID = backgroundId;
        }
    }

    public bool UpdateIsBuyingInRow(int id, bool isBuying)
    {
        var isExistsRow = rows.Exists((PictureProxy obj) => obj.id == id);
        if (!isExistsRow)
        {
            return false;
        }
        else
        {
            var row = rows.Find((PictureProxy obj) => obj.id == id);
            row.IsBuying = isBuying;

            return true;
        }
    }

    public bool UpdateIsFirstSelectInRow(int id, bool isFirstSelect)
    {
        var isExistsRow = rows.Exists((PictureProxy obj) => obj.id == id);
        if (!isExistsRow)
        {
            return false;
        }
        else
        {
            var row = rows.Find((PictureProxy obj) => obj.id == id);
            row.IsFirstSelect = isFirstSelect;

            return true;
        }
    }

    public PictureProxy GetRowById(int id)
    {
        var TableLength = rows.Count;
        for (int i = 0; i < TableLength; i++)
        {
            if (rows[i].id == id)
            {
                return rows[i];
            }
        }
        return null;
    }

    public PictureProxy GetRowById(int id, PictureMaterial material)
    {
        var TableLength = rows.Count;
        for (int i = 0; i < TableLength; i++)
        {
            if (rows[i].id == id && rows[i].pictureMaterial == material)
            {
                return rows[i];
            }
        }
        return null;
    }

    public PictureProxy GetRowByIndex(int index)
    {
        return rows[index];
    }

    public bool DeleteRowById(int id)
    {
        var TableLength = rows.Count;
        for (int i = 0; i < TableLength; i++)
        {
            if (rows[i].id == id)
            {
                rows.Remove(rows[i]);
                return true;
            }
        }
        return false;
    }

    public int GetTableSize()
    {
        if (rows != null)
            return rows.Count;
        return 0;
    }

    public List<PictureProxy> GetPicturesForGallery()
    {
        var pictures = new List<PictureProxy>();
        var rowsCount = rows.Count;
        for (int i = 0; i < rowsCount; i++)
        {
            if (rows[i].drawingProgress >= 100)
            {
                pictures.Add(rows[i]);
            }
        }
        return pictures;
    }
}
