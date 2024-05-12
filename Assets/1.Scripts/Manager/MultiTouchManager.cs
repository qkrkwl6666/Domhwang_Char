using UnityEngine;

public enum Dir
{
    None,
    RIGHT, DOWN, LEFT, UP
}

public class MultiTouchManager : Singleton<MultiTouchManager>
{

    public bool Tap { get; private set; }
    public bool LongTap { get; private set; }
    public bool DoubleTap { get; private set; }

    public bool Hold {  get; private set; }

    private int primayFingerId = int.MinValue;

    private float timeTap = 0.25f;
    private float timeLongTap = 0.5f;
    private float timeDoubleTap = 0.25f;

    private float primayStartTime = 0f;
    private float firstTapTime = 0f;
    private bool isFirstTap = false;

    // Ω∫øÕ¿Ã«¡ 
    public Vector3 Swipe { get; private set; }
    public float minSwipeDistanceInch = 0.25f; // Swipe ¿ŒΩƒ √÷º“ ∞≈∏Æ
    private float minSwipeDistancePixels;
    public float swpieMove = 5f;
    private Vector2 touchStartPos;
    private float swipeTime = 0.25f;

    // Pinch Zoom
    private Vector2 mainPos;
    private Vector2 subPos;
    private float firstDistance;
    public float pinchValue;
    private float speed = 1f;

    public float zoom;
    public float zoomMaxInch = 1f;
    public float zoomMaxPixel;

    // Rotate
    private Vector2 rotateMainPos;
    private Vector2 rotateSubPos;

    public float angle;

    private void Update()
    {
        Tap = false;
        LongTap = false;
        DoubleTap = false;
        Swipe = default;

        if (Input.touchCount == 2)
        {
            var first = Input.GetTouch(0);
            var second = Input.GetTouch(1);
            if ((first.phase == TouchPhase.Moved || first.phase == TouchPhase.Stationary) &&
                (second.phase == TouchPhase.Moved || second.phase == TouchPhase.Stationary))
            {
                var firstPrevPos = first.position - first.deltaPosition;
                var secondPrevPos = second.position - second.deltaPosition;
                var prevDiff = (firstPrevPos - secondPrevPos).magnitude;

                var Zoom = prevDiff - prevDiff;
                Zoom /= zoomMaxPixel;
                Zoom = Mathf.Clamp(Zoom, -10, 1f);
            }
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (primayFingerId == int.MinValue)
                    {
                        primayFingerId = touch.fingerId;
                        primayStartTime = Time.time;
                        touchStartPos = Input.touches[i].position;
                        mainPos = touchStartPos;
                    }
                    break;
                    ;
                case TouchPhase.Moved:

                    break;
                case TouchPhase.Stationary:

                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    pinchValue = 0f;
                    if (primayFingerId == touch.fingerId)
                    {
                        primayFingerId = int.MinValue;
                        float duration = Time.time - primayStartTime;

                        if (duration > timeLongTap) // ∑’ ≈«
                        {
                            LongTap = true;
                            break;
                        }

                        if (duration < timeTap) // ≈«
                        {
                            Tap = true;

                            if (isFirstTap && Time.time - firstTapTime > timeDoubleTap)
                            {
                                isFirstTap = false;
                            }

                            if (!isFirstTap)
                            {
                                isFirstTap = true;
                                firstTapTime = Time.time;
                            }
                            else if (Time.time - firstTapTime < timeDoubleTap) // ¥ı∫Ì≈«
                            {
                                isFirstTap = false;
                                DoubleTap = true;
                                firstTapTime = 0f;
                            }
                            else
                            {
                                isFirstTap = false;
                                firstTapTime = 0f;
                            }
                        }

                    }
                    break;
            }
        }
    }

    private void LateUpdate()
    {
        // Tap = false;
        // LongTap = false;
        // DoubleTap = false;
    }
}
