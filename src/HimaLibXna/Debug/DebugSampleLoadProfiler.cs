using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DebugSample;
using Microsoft.Xna.Framework;

namespace HimaLib.Debug
{
    public class DebugSampleLoadProfiler : ILoadProfiler
    {
        TimeRuler TimeRuler
        {
            get
            {
                return DebugSampleAccessor.GetInstance().TimeRuler;
            }
        }

        int colorIndex;
        
        Color[] colorSet;

        Stack<string> markerNameStack;

        public DebugSampleLoadProfiler()
        {
            colorIndex = 0;

            colorSet = new Color[]{
                Color.Yellow, Color.Cyan,
                Color.Green, Color.Magenta, Color.Blue, Color.Orange,
                Color.Lime, Color.SkyBlue, Color.DarkOrange,
                Color.Purple, Color.Red, Color.Pink, Color.Violet,
            };

            markerNameStack = new Stack<string>();

            TimeRuler.ShowLog = true;
        }

        public void StartFrame()
        {
            colorIndex = 0;
            TimeRuler.StartFrame();
        }

        public void BeginMark(string markerName)
        {
            TimeRuler.BeginMark(markerNameStack.Count, markerName, colorSet[colorIndex]);
            markerNameStack.Push(markerName);
            
            if (++colorIndex >= colorSet.Length)
            {
                colorIndex = 0;
            }
        }

        public void EndMark()
        {
            var markerName = markerNameStack.Pop();
            TimeRuler.EndMark(markerNameStack.Count, markerName);
        }
    }
}
