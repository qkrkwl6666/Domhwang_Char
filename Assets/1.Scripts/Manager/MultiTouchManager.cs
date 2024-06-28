using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public enum Dirs
{
    None,
    Up, Down, Left, Right
}

public class MultiTouchManager : Singleton<MultiTouchManager>
{
    public bool Tap { get; private set; }
    public bool LongTap { get; private set; }
    public bool DoubleTap { get; private set; }

    public Dirs Swipe { get; private set; }
    public float Zoom { get; private set; }
    public float Rotation { get; private set; }


    private Finger primayFinger;

    private float timeTap = 0.25f;
    private float timeLongTap = 0.5f;
    private float timeDoubleTap = 0.25f;

    private bool isFirstTap = false;
    private float firstTapTime = 0f;

    public float minSwipeDistanceInch = 0.25f; // Inch
    private float minSwipeDistancePixels;

    private float swipeTime = 0.25f;

    public float zoomMaxInch = 1f; // Inch
    private float zoomMaxPixel;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();

        minSwipeDistancePixels = Screen.dpi * minSwipeDistanceInch;
        zoomMaxPixel = Screen.dpi * zoomMaxInch;
    }

    private void OnDestroy()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Update()
    {
        Tap = false;
        LongTap = false;
        DoubleTap = false;
        Swipe = Dirs.None;
        Zoom = 0f;
        Rotation = 0f;

        foreach (var touch in Touch.activeTouches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (primayFinger == null)
                    {
                        primayFinger = touch.finger;
                    }
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:

                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (primayFinger == touch.finger)
                    {
                        primayFinger = null;
                        var duration = Time.time - touch.startTime;

                        if (duration < swipeTime)
                        {
                            var diff = (touch.screenPosition - touch.startScreenPosition);
                            if (diff.magnitude > minSwipeDistancePixels)
                            {
                                if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
                                {
                                    Swipe = diff.x > 0 ? Dirs.Right : Dirs.Left;
                                }
                                else
                                {
                                    Swipe = diff.y > 0 ? Dirs.Up : Dirs.Down;
                                }
                            }
                        }

                        if (duration < timeTap)
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
                            else
                            {
                                DoubleTap = Time.time - firstTapTime < timeDoubleTap;
                                isFirstTap = false;
                            }
                        }

                        if (duration > timeLongTap)
                        {
                            LongTap = true;
                        }
                    }
                    break;
            }
        }


        if (Touch.activeTouches.Count == 2)
        {
            var fisrt = Touch.activeTouches[0];
            var second = Touch.activeTouches[1];
            if ((fisrt.phase == TouchPhase.Moved || fisrt.phase == TouchPhase.Stationary) &&
                (second.phase == TouchPhase.Moved || second.phase == TouchPhase.Stationary))
            {
                var firstPrevPos = fisrt.screenPosition - fisrt.delta;
                var secondPrevPos = second.screenPosition - second.delta;
                var prevDiff = (firstPrevPos - secondPrevPos).magnitude;
                var diff = (fisrt.screenPosition - second.screenPosition).magnitude;

                var Zoom = diff - prevDiff;
                Zoom /= zoomMaxPixel;
                Zoom = Mathf.Clamp(Zoom, -10, 1f);

                var prevDot = Vector2.Dot(Vector2.up, (firstPrevPos - secondPrevPos).normalized);
                var prevDgree = Mathf.Acos(prevDot) * Mathf.Rad2Deg;

                var dot = Vector2.Dot(Vector2.up, (fisrt.screenPosition - second.screenPosition).normalized);
                var dgree = Mathf.Acos(dot) * Mathf.Rad2Deg;

                Rotation = Mathf.Clamp(dgree, -180, 180);
            }
        }
    }

}
