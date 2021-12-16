using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudyTimer
{
    internal class Session
    {
        public enum Status
        {
            Study,
            Pause
        }

        private Status status;
        private float studyTime;
        private float pauseTime;
        private float currentTime = 0;

        private Label elapsedLabel;

        private Label nextPause;

        private Label pauseStudy;

        private string hours = "";
        private string minutes = "";
        private string seconds = "";

        public float ElapsedTime { get; private set; }

        public bool Interrupt { get; set; }

        public string Subject { get; set; }

        public Dictionary<string, string> CurrentSession { get; set; }

        public Session(float sTime, float pTime, params Label[] labels)
        {
            studyTime = sTime;
            pauseTime = pTime;
            this.elapsedLabel = labels[0];
            this.nextPause = labels[1];
            this.pauseStudy = labels[2];
            CurrentSession = new Dictionary<string, string>();
            Interrupt = true;
        }

        public event Action OnTimeElapsed = delegate { };

        public event Action OnPauseElapsed = delegate { };

        public async void Run()
        {
            Interrupt = false;
            while (!Interrupt)
            {
                if (status == Status.Study && currentTime >= studyTime)
                {
                    status = Status.Pause;
                    currentTime = 0;
                    Task.Run(() => OnTimeElapsed?.Invoke());
                }
                else if (status == Status.Pause && currentTime >= pauseTime)
                {
                    status = Status.Study;
                    currentTime = 0;
                    Task.Run(() => OnPauseElapsed?.Invoke());
                }
                currentTime += 1000;
                ElapsedTime += 1000;
                elapsedLabel.Text = this.GetTime(ElapsedTime / 1000);
                if (status == Status.Study)
                {
                    nextPause.Text = this.GetTime((studyTime - currentTime) / 1000);
                    pauseStudy.Text = "To Next Pause";
                }
                else
                {
                    nextPause.Text = this.GetTime((pauseTime - currentTime) / 1000);
                    pauseStudy.Text = "To Next Session";
                }
                await Task.Delay(10);
            }
            currentTime = 0;
            Console.WriteLine(Interrupt);
        }

        public void UpdateStudyPauseTime(float studyTime, float pauseTime)
        {
            this.studyTime = studyTime;
            this.pauseTime = pauseTime;
        }

        public bool IsValid()
        {
            return this.studyTime != 1;
        }

        public void SaveData(string subject, string time)
        {
        }

        public string GetStudyTime()
        {
            return (studyTime / 60000).ToString();
        }

        public void Stop() => Interrupt = true;

        public void Reset()
        {
            elapsedLabel.Text = "";
            nextPause.Text = "";
        }

        public string GetTime(float secs)
        {
            if (secs < 3600)
            {
                if ((int)(secs / 60) < 10)
                {
                    minutes = "0" + ((int)secs / 60).ToString();
                    if (secs % 60 < 10)
                    {
                        seconds = "0" + ((int)secs % 60).ToString();
                    }
                    else { seconds = ((int)secs % 60).ToString(); }
                }
                else
                {
                    minutes = ((int)secs / 60).ToString();
                    if (secs % 60 < 10)
                    {
                        seconds = "0" + ((int)secs % 60).ToString();
                    }
                    else { seconds = ((int)secs % 60).ToString(); }
                }
            }
            else
            {
                if ((int)(secs / 60 % 60) < 10)
                {
                    minutes = "0" + ((int)secs / 60 % 60).ToString();
                    if (secs % 60 < 10)
                    {
                        seconds = "0" + ((int)secs % 60).ToString();
                    }
                    else { seconds = ((int)secs % 60).ToString(); }
                }
                else
                {
                    minutes = ((int)secs / 60 % 60).ToString();
                    if (secs % 60 < 10)
                    {
                        seconds = "0" + ((int)secs % 60).ToString();
                    }
                    else { seconds = ((int)secs % 60).ToString(); }
                }
            }
            hours = "0" + (int)secs / 3600;
            return hours + ":" + minutes + ":" + seconds;
        }
    }
}