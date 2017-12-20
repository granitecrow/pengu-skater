﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    private const float DEADZONE = 200; // in pixels

    public static MobileInput Instance { set; get; }

    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private Vector2 swipeDelta, startTouch;

    public bool Tap { get { return tap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // reset all the booleans to false
        tap = false;
        swipeLeft = false;
        swipeRight = false;
        swipeUp = false;
        swipeDown = false;

        // check for input

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Hello! Position is " + Input.mousePosition);
            tap = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero;
        }
        #endregion

        #region Mobile Inputs
        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startTouch = Input.mousePosition;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTouch = swipeDelta = Vector2.zero;
            }
        }
        #endregion

        // calculate distance between startTouch and where you are as of this frame
        swipeDelta = Vector2.zero;
        if (startTouch != Vector2.zero)
        {
            // check mobile
            if (Input.touches.Length != 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            // check standalone
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
                //Debug.Log(swipeDelta);
            }
        }

        // check if beyond deadzone
        if (swipeDelta.magnitude > DEADZONE)
        {
            // this is a confirmed swipe
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // left or right
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {
                // up or down
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
               
            }

            startTouch = swipeDelta = Vector2.zero;
        }
    }
}
