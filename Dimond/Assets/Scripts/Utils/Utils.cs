using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using System;

public class Utils : MonoBehaviour
{
    public static class TAGS
    {
        public const string BRUSHES_LIST_CPNTAINER = "brushes_list_container";
        public const string SECTION_LIST_CONTAINER = "section_list_container";
        public const string SCROLL_LIST_CONTAINER = "scroll_list_container";
        public const string EXITE_BUTTON = "exite_button";
        public const string MENU_BUTTONS_LIST = "menu_buttons_list";
        public const string BUTTON_BRUSHS = "button_brushs";
        public const string BUTTON_NUMBER = "button_number";
        public const string SCROLL_LIST_COL = "scroll_list_col";
        public const string MAIN_CAMERA = "MainCamera";
        public const string PICTURE = "picture";
        public const string BUY_BUTTON = "buy_button";
        public const string RESET_BUTTON = "reset_button";
        public const string SCROLL_LIST_ROW = "scroll_list_row";
        public const string CHOOSE_PICTURE_ACTIVITY = "choose_picture_activity";
        public const string CHOOSE_MATERIAL_ACTIVITY = "choose_material_activity";
        public const string PLAYING_FILD_ACTIVITY = "playing_fild_activity";
        public const string GALLERY_ACTIVITY = "gallery_activity";
        public const string CHOOSE_BG_BUTTON = "choose_bg_button";
        public const string VIBER_BUTTON = "viber_button";
        public const string FACEBOOK_BUTTON = "facebook_button";
        public const string INSTAGRAMM_BUTTON = "instagramm_button";
        public const string NEXT_ADVICE_BUTTON = "next_advice_button";
        public const string SKIP_ADVICE_BUTTON = "skip_advice_button";
        public const string START_ROULETTE = "start_roulette";
        public const string BONUS_X4 = "bonus_x4";
        public const string BONUS_x3 = "bonus_x3";
        public const string BONUS_Dimond = "bonus_dimond";
        public const string BONUS_Coloring = "bonus_coloring";
        public const string BONUS_Strands = "bonus_strands";
        public const string BONUS_Picture = "bonus_picture";
        public const string OK_DIALOG_BUTTON = "ok_dialog_button";
        public const string ROULETTE_ACTIVITY = "roulette_activity";

    }

    private static bool isDelayCoroutineExecuting = false;
    public delegate void DelayPredicate();

    public static IEnumerator Delay(float timeDelay, DelayPredicate predicate)
    {
        if (isDelayCoroutineExecuting)
            yield break;

        isDelayCoroutineExecuting = true;
        yield return new WaitForSeconds(timeDelay);
        predicate();
        isDelayCoroutineExecuting = false;

    }
    private static bool isWhaitWhileCoroutineExecuting = false;

    public static IEnumerator WhaiWhile(System.Func<bool> func)
    {
        if (isWhaitWhileCoroutineExecuting)
            yield break;
        isWhaitWhileCoroutineExecuting = true;
        yield return new WaitWhile(func);
        isWhaitWhileCoroutineExecuting = false;
    }

    public static Sprite MakeSpriteFromBytes(byte[] array, float w, float h,TextureFormat format, float pix_per_unit)
    {
        Texture2D texture = new Texture2D((int)w, (int)h, format, false);
        texture.LoadRawTextureData(array);
        texture.Apply();
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), pix_per_unit);
    }


    // сохраняем/выгружаем фотку (в байтах)
    public static byte[] GetPhotoBytes(int photoID)
    {
        string fileName = string.Format("{0}/img_{1}.png", "/Users/alexboicko/Documents/com.yovogames.diamands/Dimond/Assets/Resources/", photoID);
        if (File.Exists(fileName))
            return File.ReadAllBytes(fileName);
        return null;
    }

    public static void SetPhotoBytes(byte[] array, int photoID)
    {
        string fileName = string.Format("{0}/img_{1}.png", "/Users/alexboicko/Documents/com.yovogames.diamands/Dimond/Assets/Resources/", photoID);
        File.WriteAllBytes(fileName, array);
    }

    // скриним камеру в текстуру
    public static Texture2D CaptureSimple(Camera captureCam)
    {
        captureCam.enabled = true;
        Texture2D retval = null;

        int screenHeight = Screen.height;
        int screenWidth = (int)(screenHeight * (3f / 4f));

        RenderTexture rt = new RenderTexture(
            screenWidth,
            screenHeight,
            24);
        captureCam.targetTexture = rt;
        retval = new Texture2D(
            screenWidth,
            screenHeight,
            TextureFormat.RGB24,
            false);

        retval.filterMode = FilterMode.Point;

        captureCam.Render();
        RenderTexture.active = rt;
        retval.ReadPixels(new Rect(0, 0, screenWidth, screenHeight), 0, 0);
        retval.Apply();
        captureCam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        captureCam.enabled = false;
        return retval;
    }

    public enum ImageFilterMode : int
    {
        Nearest = 0,
        Biliner = 1,
        Average = 2
    }

    public static Texture2D Resize(Texture2D pSource, ImageFilterMode pFilterMode, float pScale)
    {
        float xWidth = Mathf.RoundToInt((float)pSource.width * pScale);
        float xHeight = Mathf.RoundToInt((float)pSource.height * pScale);
        return Resize(pSource, pFilterMode, xWidth, xHeight);
    }

    public static Texture2D Resize(Texture2D pSource, ImageFilterMode pFilterMode, float w, float h)
    {

        //*** Variables
        int i;

        //*** Get All the source pixels
        Color[] aSourceColor = pSource.GetPixels(0);
        Vector2 vSourceSize = new Vector2(pSource.width, pSource.height);

        //*** Calculate New Size
        float xWidth = w;
        float xHeight = h;

        //*** Make New
        Texture2D oNewTex = new Texture2D((int)xWidth, (int)xHeight, TextureFormat.RGBA32, false);

        //*** Make destination array
        int xLength = (int)xWidth * (int)xHeight;
        Color[] aColor = new Color[xLength];

        Vector2 vPixelSize = new Vector2(vSourceSize.x / xWidth, vSourceSize.y / xHeight);

        //*** Loop through destination pixels and process
        Vector2 vCenter = new Vector2();
        for (i = 0; i < xLength; i++)
        {

            //*** Figure out x&y
            float xX = (float)i % xWidth;
            float xY = Mathf.Floor((float)i / xWidth);

            //*** Calculate Center
            vCenter.x = (xX / xWidth) * vSourceSize.x;
            vCenter.y = (xY / xHeight) * vSourceSize.y;

            //*** Do Based on mode
            //*** Nearest neighbour (testing)
            if (pFilterMode == ImageFilterMode.Nearest)
            {

                //*** Nearest neighbour (testing)
                vCenter.x = Mathf.Round(vCenter.x);
                vCenter.y = Mathf.Round(vCenter.y);

                //*** Calculate source index
                int xSourceIndex = (int)((vCenter.y * vSourceSize.x) + vCenter.x);

                //*** Copy Pixel
                aColor[i] = aSourceColor[xSourceIndex];
            }

            //*** Bilinear
            else if (pFilterMode == ImageFilterMode.Biliner)
            {

                //*** Get Ratios
                float xRatioX = vCenter.x - Mathf.Floor(vCenter.x);
                float xRatioY = vCenter.y - Mathf.Floor(vCenter.y);

                //*** Get Pixel index's
                int xIndexTL = (int)((Mathf.Floor(vCenter.y) * vSourceSize.x) + Mathf.Floor(vCenter.x));
                int xIndexTR = (int)((Mathf.Floor(vCenter.y) * vSourceSize.x) + Mathf.Ceil(vCenter.x));
                int xIndexBL = (int)((Mathf.Ceil(vCenter.y) * vSourceSize.x) + Mathf.Floor(vCenter.x));
                int xIndexBR = (int)((Mathf.Ceil(vCenter.y) * vSourceSize.x) + Mathf.Ceil(vCenter.x));

                //*** Calculate Color
                aColor[i] = Color.Lerp(
                    Color.Lerp(aSourceColor[xIndexTL], aSourceColor[xIndexTR], xRatioX),
                    Color.Lerp(aSourceColor[xIndexBL], aSourceColor[xIndexBR], xRatioX),
                    xRatioY
                );
            }

            //*** Average
            else if (pFilterMode == ImageFilterMode.Average)
            {

                //*** Calculate grid around point
                int xXFrom = (int)Mathf.Max(Mathf.Floor(vCenter.x - (vPixelSize.x * 0.5f)), 0);
                int xXTo = (int)Mathf.Min(Mathf.Ceil(vCenter.x + (vPixelSize.x * 0.5f)), vSourceSize.x);
                int xYFrom = (int)Mathf.Max(Mathf.Floor(vCenter.y - (vPixelSize.y * 0.5f)), 0);
                int xYTo = (int)Mathf.Min(Mathf.Ceil(vCenter.y + (vPixelSize.y * 0.5f)), vSourceSize.y);

                //*** Loop and accumulate
                //Vector4 oColorTotal = new Vector4();
                Color oColorTemp = new Color();
                float xGridCount = 0;
                for (int iy = xYFrom; iy < xYTo; iy++)
                {
                    for (int ix = xXFrom; ix < xXTo; ix++)
                    {

                        //*** Get Color
                        oColorTemp += aSourceColor[(int)(((float)iy * vSourceSize.x) + ix)];

                        //*** Sum
                        xGridCount++;
                    }
                }

                //*** Average Color
                aColor[i] = oColorTemp / (float)xGridCount;
            }
        }

        //*** Set Pixels
        oNewTex.SetPixels(aColor);
        oNewTex.Apply();
        //*** Return
        return oNewTex;
    }
}
