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
                return _waitingProcesses;
            }
            set
            {
                _waitingProcesses = value;
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

        public ViewModel()
        {
            Semaphore = new Semaphore(1, 10, Guid.NewGuid().ToString());
            ProcList = new ObservableCollection<Process>();
            StartProcess = new relayCommand(param => this.AddProcess());

            WorkingProcesses = new ObservableCollection<Process>();
            WaitingProcesses = new ObservableCollection<Process>();
            CompletedProcesses = new ObservableCollection<Process>();
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

            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                WaitingProcesses.Add(pr);
            }));


            Semaphore.WaitOne();

            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                WaitingProcesses.Remove(pr);
                WorkingProcesses.Add(pr);
            }));


            while (pr.Status < 10)
            {
                Thread.Sleep(250);
                pr.Status++;
            }

            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                WorkingProcesses.Remove(pr);
            }));

            Semaphore.Release();

            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                CompletedProcesses.Add(pr);
            }));

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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
