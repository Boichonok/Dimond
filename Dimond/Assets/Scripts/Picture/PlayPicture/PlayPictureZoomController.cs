using UnityEngine;
using System.Collections;

public class PlayPictureZoomController : MonoBehaviour
{
    private float orthoZoomSpeed = 0.001f;
    private float swipeSpeed = 0.02f;
    private Camera camera = null;

    private float lastCameraZoom;
    private float startRightSidePos;
    private float startLeftSidePos;
    private float startUpSidePos;
    private float startBottonSidePos;
    private IPlayPictureCameraListener listener;
    public void InitIPlayPicrtureZoomListener(IPlayPictureCameraListener listener)
    {
        this.listener = listener;
    }

    public bool CanSwipe { get; set; }

    private void Start()
    {
        camera = GetComponent<Camera>();
        CanSwipe = true;
        startRightSidePos = camera.ScreenToWorldPoint(new Vector2(Screen.width, 0.0f)).x;
        startLeftSidePos = camera.ScreenToWorldPoint(new Vector2(0.0f, 0.0f)).x;
        startUpSidePos = camera.ScreenToWorldPoint(new Vector2(0.0f, Screen.height)).y;
        startBottonSidePos = camera.ScreenToWorldPoint(new Vector2(0.0f, 0.0f)).y;
    }
    void Update()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            if (camera.orthographic)
            {

                // ... change the orthographic size based on the change in distance between the touches.
                camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                // Make sure the orthographic size never drops below zero.
                camera.orthographicSize = Mathf.Clamp(Mathf.Max(camera.orthographicSize, 0.1f), 0.1f, 0.9f);
                if (camera.orthographicSize - lastCameraZoom < 0)
                {
                    listener.UpdateZoome(camera.orthographicSize, true);

                }
                else
                {
                    listener.UpdateZoome(camera.orthographicSize, false);
                }
                lastCameraZoom = camera.orthographicSize;


            }

        }
        if (Input.touchCount == 1 &&
            Input.GetTouch(0).phase == TouchPhase.Moved &&
            CanSwipe)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * swipeSpeed * Time.deltaTime, -touchDeltaPosition.y * swipeSpeed * Time.deltaTime, 0);
            var x = Mathf.Clamp(transform.position.x, startLeftSidePos, startRightSidePos);
            var y = Mathf.Clamp(transform.position.y, startBottonSidePos, startUpSidePos);
            transform.position = new Vector2(x, y);
            if (touchDeltaPosition.x > 20f)
            {
                listener.UpdateSwipe(true, false, false, false);
            }
            else if (touchDeltaPosition.x < -20f)
            {
                listener.UpdateSwipe(false, true, false, false);
            }
            else if (touchDeltaPosition.y > 20f)
            {
                listener.UpdateSwipe(false, false, true, false);
            }
            else if (touchDeltaPosition.y < -20f)
            {
                listener.UpdateSwipe(false, false, false, true);
            }
        }
    }
}
