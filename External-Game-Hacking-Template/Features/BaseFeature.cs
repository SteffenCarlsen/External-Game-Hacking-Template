using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace External_Game_Hacking_Template.Features
{
    /// <summary>
    /// Component which will have a separate thread for looping action.
    /// </summary>
    public abstract class BaseFeature : IDisposable
    {
        #region // storage

        /// <summary>
        /// Custom thread name.
        /// </summary>
        protected virtual string ThreadName => nameof(BaseFeature);

        /// <summary>
        /// Timeout for thread to finish.
        /// </summary>
        protected virtual TimeSpan ThreadTimeout { get; set; } = new TimeSpan(0, 0, 0, 3);

        /// <summary>
        /// How many updates per second should the thread perform
        /// </summary>
        protected virtual int ThreadFPS { get; set; } = 128;

        /// <summary>
        /// Thread for this component.
        /// </summary>
        private Thread Thread { get; set; }

        #endregion

        #region // ctor

        /// <summary />
        protected BaseFeature()
        {
            Thread = new Thread(ThreadStart)
            {
                Name = ThreadName, // Virtual member call in constructor
            };
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            Thread.Interrupt();
            if (!Thread.Join(ThreadTimeout))
            {
                Thread.Abort();
            }
            Thread = default;
        }

        #endregion

        #region // routines

        /// <summary>
        /// Launch thread for execute frames.
        /// </summary>
        public void Start()
        {
            Thread.Start();
        }

        /// <summary>
        /// Thread method.
        /// </summary>
        private void ThreadStart()
        {
            var maxTime = 60000 / ThreadFPS;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                while (true)
                {
                    stopWatch.Restart();
                    FrameAction();
                    var elapsedTime = (int)stopWatch.ElapsedMilliseconds;
                    if (elapsedTime > maxTime)
                    {
                        Console.WriteLine($"Bottleneck detected in {ThreadName}, execution took {elapsedTime}ms, expected a maximum of {maxTime}ms");
                    }
                    else
                    {
                        var expectedSleep = maxTime - elapsedTime;
                        Thread.Sleep(expectedSleep > 0 ? expectedSleep : 1);
                    }
                }
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine(ThreadName + " interrupted");
            }
        }

        /// <summary>
        /// Frame to loop inside a thread.
        /// </summary>
        protected abstract void FrameAction();

        #endregion
    }
}
