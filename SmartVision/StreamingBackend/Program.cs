﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AForge.Video;
using FaceAnalysis;
using Objects.CameraProperties;

namespace StreamingBackend
{
    public static class MJPEGStreamManager
    {
        private static readonly ConcurrentDictionary<ProcessableVideoSource, MJPEGServer> streamServers = new ConcurrentDictionary<ProcessableVideoSource, MJPEGServer>();
        public static FaceProcessor Processor { get; } = new FaceProcessor();
        static void Main(string[] args)
        {
            new System.Threading.AutoResetEvent(false).WaitOne();
        }

        public static (string url, string id) AddStream(IVideoSource source)
        {
            return AddStream(new ProcessableVideoSource(source));
        }

        public static (string url, string id) AddStream(ProcessableVideoSource source)
        {
            var server = new MJPEGServer(source, start: true);
            streamServers[source] = server;
            Processor.AddSource(source);
            return (server.Url, source.Id.ToString());
            
        }
        
        public static void RemoveStream(string sourceId)    
        {
            foreach(var source in streamServers.Keys)
                if (source.Id.ToString() == sourceId)
                {
                    RemoveStream(source);
                    return;
                }

        }

        public static void RemoveStream(ProcessableVideoSource source)
        {
            streamServers.TryRemove(source, out var server);
            server.Stop();
            Processor.RemoveSource(source);
        }

        public static IEnumerable<(string url, string id)> GetStreams()
        {
            return streamServers.Select(pair => (pair.Value.Url, pair.Key.Id.ToString()));
        }
    }
}
