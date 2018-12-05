﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Reflection;

namespace WeTest.U3DAutomation
{
    public enum MotionEventAction
    {
        ACTION_DOWN = 0,
        ACTION_UP = 1,
        ACTION_MOVE = 2
    }

    class MobileScreen
    {
        public int width { get; set; }
        public int height { get; set; }

        public MobileScreen()
        {
            width = 0;
            height = 0;
        }

        public MobileScreen(int w, int h)
        {
            width = w;
            height = h;
        }
    }

    class AndroidRobot
    {
        private static AndroidRobot instance = new AndroidRobot();

        //Unity 3.8版本
        private static AndroidJavaClass u3dautomation;

        private static MobileScreen mscreen = null;


        public static AndroidRobot INSTANCE
        {
            get { return instance; }
        }

        private AndroidRobot()
        {
            u3dautomation = new AndroidJavaClass("com.tencent.wetest.U3DAutomation");
        }


        public void InjectMotionEvent(float x, float y, MotionEventAction action)
        {
            Logger.v("Inject Motion event point: " + x + ", " + y);
            Logger.v("Inject Motion event touch type: " + action.ToString());

            try
            {
                u3dautomation.CallStatic("InjectTouchEvent", new object[] { (int)action, x, y });
            }
            catch (System.Exception ex)
            {
                Logger.e(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void TouchDown(float x, float y)
        {
            InjectMotionEvent(x, y, MotionEventAction.ACTION_DOWN);
        }

        public void TouchUp(float x, float y)
        {
            InjectMotionEvent(x, y, MotionEventAction.ACTION_UP);
        }

        public void TouchMove(float x, float y)
        {
            InjectMotionEvent(x, y, MotionEventAction.ACTION_MOVE);
        }


        public static MobileScreen getAndroidMScreen()
        {
            Logger.v("getAndroidMScreen, begin");

            if (mscreen != null)
            {
                return mscreen;
            }

            int width = -1;
            int height = -1;
            bool bMScreen = false;
            try
            {
                width = u3dautomation.CallStatic<int>("GetWidth");
                height = u3dautomation.CallStatic<int>("GetHeight");
                bMScreen = true;
            }
            catch (System.Exception ex)
            {
                Logger.e(ex.ToString());
            }

            if (bMScreen)
            {
                mscreen = new MobileScreen(width, height);
            }

            Logger.d("getAndroidMScreen: width=" + width + ", height=" + height);

            return mscreen;
        }

    }
}
