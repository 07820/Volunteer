﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Form.Util
{
    public class ProgressableStreamContent : HttpContent
    {

        /// <summary>
        /// Lets keep buffer of 20kb
        /// </summary>
        public const int defaultBufferSize = 5 * 4096;

        public HttpContent content;
        public int bufferSize;
        //public bool contentConsumed;
        public Func<long, long, bool> progress;

        public ProgressableStreamContent(HttpContent content, Func<long, long, bool> progress) : this(content, defaultBufferSize, progress) { }

        public ProgressableStreamContent(HttpContent content, int bufferSize, Func<long, long, bool> progress)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }

            this.content = content;
            this.bufferSize = bufferSize;
            this.progress = progress;

            foreach (var h in content.Headers)
            {
                this.Headers.Add(h.Key, h.Value);
            }
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {

            return Task.Run(async () =>
            {
                var buffer = new Byte[this.bufferSize];
                long size;
                TryComputeLength(out size);
                var uploaded = 0;


                using (var sinput = await content.ReadAsStreamAsync())
                {
                    while (true)
                    {
                        var length = sinput.Read(buffer, 0, buffer.Length);
                        if (length <= 0) break;

                        //downloader.Uploaded = uploaded += length;
                        uploaded += length;
                        if(!progress.Invoke(uploaded, size))
                        {
                            throw new Exception("Stop uploading!");
                        }
                        //System.Diagnostics.Debug.WriteLine($"Bytes sent {uploaded} of {size}");

                        stream.Write(buffer, 0, length);
                        stream.Flush();
                    }
                }
                stream.Flush();
            });
        }

        protected override bool TryComputeLength(out long length)
        {
            length = content.Headers.ContentLength.GetValueOrDefault();
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                content.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
