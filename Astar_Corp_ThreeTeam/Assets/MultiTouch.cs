using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static UnityEngine.InputSystem.InputAction;

public class MultiTouch : MonoBehaviour
{
    private float touchTime;                // use swipe
    private float swipeDelayTime = 0.1f;      // use swipe
    public Vector2 touchStartPos;
    public Vector2 curTouchPos;
    public Vector2 primaryDeltaPos;
    public Vector2 curSecTouchPos;
    public Vector2 secondDeltaPos;
    public InputActionPhase phase;
    public float pressTime;
    public bool Tap { private set; get; }
    public bool DoubleTap { private set; get; }
    public bool LongTap { private set; get; }
    public float ZoomInOut { private set; get; }
    public Vector2 SwipeDirection { private set; get; }
    public float RotateAngle { private set; get; }

    public bool isPress { get => pressTime != 0; }

    void Update()
    {
        // ??
        //if (Tap)
        //{
        //    Debug.Log("Tap!");
        //}
        //
        //if (DoubleTap)
        //{
        //    Debug.Log("DoubleTap!");
        //}
        //
        //if (LongTap)
        //{
        //    Debug.Log("LongPress!");
        //}
    }

    private void LateUpdate()
    {
        Tap = false;
        DoubleTap = false;
        LongTap = false;
        ZoomInOut = 0f;
        RotateAngle = 0f;
        SwipeDirection = Vector2.zero;
    }
    public void OnTouch(CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Disabled:
                break;
            case InputActionPhase.Waiting:
                break;
            case InputActionPhase.Started:
                break;
            case InputActionPhase.Performed:
                switch (ctx.interaction)
                {
                    case TapInteraction:
                        Tap = true;
                        break;
                    case MultiTapInteraction:
                        DoubleTap = true;
                        break;
                    case SlowTapInteraction:
                        LongTap = true;
                        break;
                }
                break;
            case InputActionPhase.Canceled:
                break;
        }
    }

    public void OnPosition(CallbackContext ctx)
    {
        curTouchPos = ctx.ReadValue<Vector2>();
    }

    public void OnDelta_Primary(CallbackContext ctx)
    {
        phase = ctx.phase;
        switch (ctx.phase)
        {
            case InputActionPhase.Canceled:
                pressTime = 0f;
                break;
            case InputActionPhase.Performed:
                pressTime += Time.deltaTime;
                primaryDeltaPos = ctx.ReadValue<Vector2>();
                break;
        }
    }

    public void OnDelta_Second(CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Performed:
                secondDeltaPos = ctx.ReadValue<Vector2>();
                break;
        }
    }

    public void OnSecondPosition(CallbackContext ctx)
    {
        curSecTouchPos = ctx.ReadValue<Vector2>();
    }

    public void OnSwipe(CallbackContext ctx)
    {
        switch (ctx.phase)
        {
            case InputActionPhase.Started:
                touchTime = Time.time;
                touchStartPos = curTouchPos;
                break;
            case InputActionPhase.Performed:
                if (touchTime + swipeDelayTime < Time.time)
                {
                    var curTouch = curTouchPos;
                    var touchVector = curTouch - touchStartPos;
                    Debug.Log(touchVector.normalized);
                    SwipeDirection = touchVector.normalized;
                }
                break;
        }
        Debug.Log(SwipeDirection);
    }

    public void OnZoomInOut(CallbackContext ctx)
    {

        // ???? == zoom in
        // ???? == zoom out
        float currentDistance;
        float prevDistance;

        GetDistance(out currentDistance, out prevDistance);

        ZoomInOut = currentDistance - prevDistance;

        Debug.Log(ZoomInOut);
    }

    //// use Pinch, Zoom
    private void GetDistance(out float currentDistance, out float prevDistance)
    {
        var touch0Prev = curTouchPos - primaryDeltaPos;
        var touch1Prev = curSecTouchPos - secondDeltaPos;

        currentDistance = Vector2.Distance(curTouchPos, curSecTouchPos);
        prevDistance = Vector2.Distance(touch0Prev, touch1Prev);
    }

    public void OnRotate(CallbackContext ctx)
    {
        float angleCurr;
        float anglePrev;

        GetRotateAngle(out angleCurr, out anglePrev);

        RotateAngle = angleCurr - anglePrev;
    }

    // use Rotate
    private void GetRotateAngle(out float current, out float prev)
    {
        var touch0Prev = curTouchPos - primaryDeltaPos;
        var touch1Prev = curSecTouchPos - secondDeltaPos;

        var vectorCurr = curTouchPos - curSecTouchPos;
        var vectorPrev = touch0Prev - touch1Prev;

        current = GetAngleByUp(vectorCurr);
        prev = GetAngleByUp(vectorPrev);
    }

    // use Rotate
    private float GetAngleByUp(Vector2 vector)
    {
        var dot = Vector2.Dot(vector.normalized, Vector2.up);
        var angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        return angle;
    }
}