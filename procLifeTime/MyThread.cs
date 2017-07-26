using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace procLifeTime
{
    abstract class MyThread
    {
        private Thread _thread;
        public void Start()
        {
            _thread.Start();
        }

        public void Start(object o)
        {
            _thread.Start(o);
        }

        public void Join()
        {
            _thread.Join();
        }

        public void Abort()
        {
            _thread.Abort();

        }

        public bool IsAlive
        {
            get
            {
                return _thread.IsAlive;
            }
        }
        public abstract void RunThread();
        public MyThread(ThreadStart tStart)
        {
            _thread = new Thread(tStart) { IsBackground = true };
            
        }

        public MyThread(ParameterizedThreadStart tStart)
        {
            _thread = new Thread(tStart) { IsBackground = true };
        }

    }
}
