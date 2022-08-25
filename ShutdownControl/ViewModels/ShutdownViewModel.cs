using ShutdownControl.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace ShutdownControl.ViewModels
{
    class ShutdownViewModel : ViewModelBase
    {
        private ShutdownModel Shutdown;
        public ShutdownViewModel()          // Construct
        {
            Shutdown = new ShutdownModel()
            {
                _StartVis = "Hidden",
                _SecondVis = "Hidden",
            };
            Hours = new ObservableCollection<HourModel>
            {
                new HourModel() { _Hour = 0 },
                new HourModel() { _Hour = 1 },
                new HourModel() { _Hour = 2 },
                new HourModel() { _Hour = 3 },
                new HourModel() { _Hour = 4 },
                new HourModel() { _Hour = 5 },
                new HourModel() { _Hour = 6 },
                new HourModel() { _Hour = 7 },
                new HourModel() { _Hour = 8 },
                new HourModel() { _Hour = 9 },
                new HourModel() { _Hour = 10 },
                new HourModel() { _Hour = 11 },
                new HourModel() { _Hour = 12 }
            };
            Minutes = new ObservableCollection<MinuteModel>
            {
                new MinuteModel() { _Minute =  0 },
                new MinuteModel() { _Minute =  1 },
                new MinuteModel() { _Minute = 10 },
                new MinuteModel() { _Minute = 20 },
                new MinuteModel() { _Minute = 30 },
                new MinuteModel() { _Minute = 40 },
                new MinuteModel() { _Minute = 50 },
            };
        }
        public ObservableCollection<HourModel> Hours { get; set; }
        public HourModel SelectHour { get; set; }
        public ObservableCollection<MinuteModel> Minutes { get; set; }
        public MinuteModel SelectMinute { get; set; }
        public string SecondVis
        {
            get { return Shutdown._SecondVis; }
            set
            {
                Shutdown._SecondVis = value;
                OnPropertyChanged();
            }
        }
        public string StartVis
        {
            get { return Shutdown._StartVis; }
            set
            {
                Shutdown._StartVis = value;
                OnPropertyChanged();
            }
        }
        public string EndTime
        {
            get { return Shutdown._EndTime; }
            set
            {
                Shutdown._EndTime = value;
                OnPropertyChanged();
            }
        }
        public int End10Second
        {
            get { return Shutdown._End10Second; }
            set
            {
                Shutdown._End10Second = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler<AlwaysTopEventArgs> AlwaysTop;
        public event EventHandler<ExitArgs> Exit;
        public void OnExit()
        {
            if (Exit != null)
                Exit(this, new ExitArgs());
        }
        public void OnAlwaysTop()
        {
            if (AlwaysTop != null)
                AlwaysTop(this, new AlwaysTopEventArgs());
        }

        Timer timer = new Timer();
        public ICommand TimeStartCommand
        {
            get { return new RelayCommand(TimeStart, CanExecute); }
        }
        public void TimeStart()
        {
            try
            {
                if (SelectHour._Hour == 0 && SelectMinute._Minute == 0)
                {
                    System.Windows.MessageBox.Show("Please use regular program to shutdown.", "Information", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Asterisk);
                }
                else
                {
                    timer.Interval = 1000;
                    timer.Elapsed += timer_Elapsed;
                    timer.Start();
                    int EndHour = DateTime.Now.Hour + SelectHour._Hour;
                    int EndMinute = DateTime.Now.Minute + SelectMinute._Minute;
                    if (EndMinute >= 60)
                    {
                        EndMinute = EndMinute - 60;
                        EndHour++;
                    }
                    if (EndHour > 23)
                        EndHour = EndHour - 24;
                    EndTime = EndHour.ToString("D2") + " : " + EndMinute.ToString("D2");
                    StartVis = "Visable";
                }
            }
            catch (NullReferenceException e)
            {
                System.Windows.MessageBox.Show("Please input hour and minute.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        public void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime EndTimeDT = Convert.ToDateTime(EndTime);
            TimeSpan Ts = EndTimeDT.Subtract(DateTime.Now);
            int EndSecond = Convert.ToInt32(Ts.TotalSeconds);
            if (EndSecond <= 10)
            {
                OnAlwaysTop();
                SecondVis = "Visable";
                if (EndSecond < 0)
                {
                    End10Second = 0;
                    Process.Start("C:\\WINDOWS\\system32\\shutdown.exe", "-f -s -t 0");
                    timer.Stop();
                    timer.Enabled = false;
                    OnExit();
                }
                else
                    End10Second = EndSecond;
            }

        }

        public ICommand ShowWindowCommand
        {
            get { return new RelayCommand(ShowWindow, CanExecute); }
        }
        public void ShowWindow()
        {
            Application.Current.MainWindow.Show();
            OnAlwaysTop();
        }

        public ICommand CloseWindowCommand
        {
            get { return new RelayCommand(CloseWindow, CanExecute); }
        }
        public void CloseWindow()
        {
            OnExit();
        }
        public bool CanExecute()
        {
            return true;
        }
    }
    public class AlwaysTopEventArgs : EventArgs
    {
        public AlwaysTopEventArgs() { }
    }
    public class ExitArgs : EventArgs
    {
        public ExitArgs() { }
    }
}
