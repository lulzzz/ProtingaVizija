﻿using System;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using AForge.Video;

namespace FaceAnalysis
{
    public static class VideoSourceHandler
    {
        private static FaceProcessor processor = new FaceProcessor();
        public static void CompleteProcessor()
        {
            processor?.Complete();
        }
        public static List<ProcessableVideoSource> Sources { get; set; } = new List<ProcessableVideoSource>();
        public static void AddSource(ProcessableVideoSource source)
        {
            Sources.Add(source);
            processor.AddSource(source);
        }
        public static void RemoveSource(ProcessableVideoSource source)
        {
            Sources.Remove(source);
            processor.RemoveSource(source);
        }
    }
}