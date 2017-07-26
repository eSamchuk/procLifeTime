using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace procLifeTime
{
    class Process : MyThread, INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        private int _status;
        public int Status
        {
            get { return _status; }
            set
            {
                if (value == _status) return;
                _status = value;
                OnPropertyChanged();
            }
        }

        //private ThreadStart _tStart;

        private ParameterizedThreadStart _tStart;

        //public Process(ThreadStart tStart) : base(tStart)
        //{
        //    Status = 0;
        //    Name = Guid.NewGuid().ToString();
        //    _tStart = tStart;
        //}

        public Process(ParameterizedThreadStart tStart) : base(tStart)
        {
            Status = 0;
            Name = Guid.NewGuid().ToString();
            _tStart = tStart;
        }



        public override void RunThread()
        {
            this.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{Name} {this.Status}";
        }


    }
}
