using System;
public interface IPlayPictureCameraListener
{
    void UpdateZoome(float currentZoom,bool isZooming);
    void UpdateSwipe(bool rightSwipe,bool leftSwipe,bool upSwipe,bool bottomSwipe);
}

