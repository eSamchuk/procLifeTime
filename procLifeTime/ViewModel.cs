using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Windows.Threading;
using System.Threading;

namespace procLifeTime
{
    class ViewModel :INotifyPropertyChanged
    {
        private static Semaphore Semaphore;

        private ObservableCollection<Process> _procList;
        public ObservableCollection<Process> ProcList
        {
            get { return _procList; }
            set
            {
                if (value == _procList) return;
                _procList = value;
                OnPropertyChanged();
            }
        }

        private int _waitCount;
        public int waitCount
        {
            get { return _waitCount; }
            set
            {
                _waitCount = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Process> _waitingProcesses;
        public ObservableCollection<Process> WaitingProcesses
        {
            get
            {
                return _workingProcesses;
            }
            set
            {
                _waitingProcesses = value;
                OnPropertyChanged();
            }
        }

        private int _workCount;
        public int workCount
        {
            get { return _workCount; }
            set
            {
                _workCount = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Process> _workingProcesses;
        public ObservableCollection<Process> WorkingProcesses
        {
            get
            {
                return _workingProcesses;
            }
            set
            {
                _workingProcesses = value;
                OnPropertyChanged();
            }
        }

        private int _doneCount;
        public int doneCount
        {
            get { return _doneCount; }
            set
            {
                _doneCount = value;
                OnPropertyChanged();
            }
        }

        private int _allCount;
        public int AllCount
        {
            get { return ProcList.Count; }
            set
            {
                _allCount = value;
                OnPropertyChanged();
            }
        }




        private ObservableCollection<Process>_completedProcesses;
        public ObservableCollection<Process> CompletedProcesses
        {
            get
            {
                return _completedProcesses;
            }
            set
            {
                _completedProcesses = value;
                OnPropertyChanged();
            }
        }

        private relayCommand _startProcess;
        public relayCommand StartProcess
        {
            get
            {
                return _startProcess;
            }
            set
            {
                _startProcess = value;
            }
        }

        private ObservableCollection<String> _log;
        public ObservableCollection<String> Log
        {
            get { return _log; }
            set { _log = value; OnPropertyChanged(); }
        }

        public ViewModel()
        {
            Semaphore = new Semaphore(5, 10, Guid.NewGuid().ToString());
            ProcList = new ObservableCollection<Process>();
            StartProcess = new relayCommand(param => this.AddProcess());
        }
        private void AddProcess()
        {
            Process tmp = new Process(ProcProgress);
            ProcList.Add(tmp);
            tmp.Start(ProcList[ProcList.Count - 1]);
        }

        private void ProcProgress(object p)
        {
            Process pr = p as Process;

            Semaphore.WaitOne();

            UpdateLists(pr);

            while (pr.Status < 10)
            {
                Thread.Sleep(250);
                pr.Status++;
                UpdateLists(pr);
            }

            UpdateLists(pr);

            Semaphore.Release();
        }
      
        public void stop()
        {
            foreach (var item in ProcList)
            {
                item.Abort();
            }
            Semaphore.Release();
        }


        public void clear()
        {
            ProcList.Clear();
            WaitingProcesses.Clear();
            WorkingProcesses.Clear();
            CompletedProcesses.Clear();
            Semaphore.Release();


        }

        private void UpdateLists(Process p)
        {
            lock (this)
            {
                AllCount = ProcList.Count();

                WaitingProcesses = new ObservableCollection<Process>(ProcList.Where(x => x.Status == 0).ToList());
                waitCount = (WaitingProcesses == null) ? 0 : WaitingProcesses.Count();
                WorkingProcesses = new ObservableCollection<Process>(ProcList.Where(x => x.Status < 10 && x.Status >= 1).ToList());
                workCount = WorkingProcesses.Count();
                CompletedProcesses = new ObservableCollection<Process>(ProcList.Where(x => x.Status == 10).ToList());
                doneCount = CompletedProcesses.Count();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
