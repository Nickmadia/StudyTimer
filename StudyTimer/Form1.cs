using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudyTimer
{
    public partial class Form1 : Form
    {
        public const string dataPath = "Data\\";
        public const string subjectsPath = "subject_data.json";
        private Session session;
        private StudyMetadata metadata;
        private SubjectsMetadata subjectsdata;

        public Form1()
        {
            InitializeComponent();
            session = session = new Session(1, 1, TbElapsedTime, TbNextPause, pauseStudy);
            this.FormClosing += (s, args) =>
            {
                if (session.IsValid())
                {
                    if (metadata != null)
                    {
                        metadata.TotalTime = (int.Parse(metadata.TotalTime.Split(' ').ToArray()[0]) + ((int)session.ElapsedTime / 1000) / 60).ToString() + " min";
                        var dataa = metadata.Sessions.Find(a => a.Date == DateTime.Now.ToShortDateString());
                        if (dataa == null)
                        {
                            metadata.Sessions.Add(new SessionMetadata(DateTime.Now.ToShortDateString(), session.CurrentSession));
                        }
                        else
                        {
                            foreach (var subj in session.CurrentSession)
                            {
                                if (dataa.Subjects.ContainsKey(subj.Key))
                                {
                                    dataa.Subjects[subj.Key] = (int.Parse(dataa.Subjects[subj.Key]) + int.Parse(dataa.Subjects[subj.Key])).ToString();
                                }
                                else
                                {
                                    dataa.Subjects.Add(subj.Key, subj.Value);
                                }
                            }
                        }
                        Serializer.SerializeJson(dataPath, "data.json", metadata);
                    }
                    else
                    {
                        metadata.TotalTime = (int)((session.ElapsedTime / 1000) / 60) + " min";
                        var types = (DateTime.Now.ToShortDateString(), session.CurrentSession);
                        metadata.Sessions.Add(new SessionMetadata(DateTime.Now.ToShortDateString(), session.CurrentSession));
                        Serializer.SerializeJson(dataPath, "data.json", metadata);
                    }
                    if (subjectsdata != null)
                    {
                        subjectsdata.LastSubject = comboBox1.SelectedItem.ToString();
                        Serializer.SerializeJson("", subjectsPath, subjectsdata);
                    }
                }
            };
            session.OnTimeElapsed += async () =>
            {
                Console.Beep();
                if (metadata.Subject.ContainsKey(session.Subject))
                {
                    metadata.Subject[session.Subject] = (int.Parse(metadata.Subject[session.Subject]) + int.Parse(session.GetStudyTime())).ToString();
                }
                else
                {
                    metadata.Subject.Add(session.Subject, session.GetStudyTime());
                }
                if (session.CurrentSession.ContainsKey(session.Subject))
                {
                    session.CurrentSession[session.Subject] = (int.Parse(session.CurrentSession[session.Subject]) + int.Parse(session.GetStudyTime())).ToString();
                }
                else
                {
                    session.CurrentSession.Add(session.Subject, session.GetStudyTime());
                }
            };
            session.OnPauseElapsed += async () =>
            {
                Console.Beep();
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Serializer.DeserializeJson<StudyMetadata>(dataPath, "data.json", out var studyMetadata))
            {
                metadata = studyMetadata;
            }
            else
            {
                metadata = new StudyMetadata();
                metadata.TotalTime = "0 min";
                metadata.Subject = new Dictionary<string, string>();
                metadata.Sessions = new List<SessionMetadata>();
                Serializer.SerializeJson(dataPath,
                "data.json", metadata);
            }
            if (Serializer.DeserializeJson<SubjectsMetadata>("", subjectsPath, out var subjects))
            {
                subjectsdata = subjects;
                comboBox1.DataSource = subjectsdata.Subjects;
                if (subjects.LastSubject != null)
                {
                    comboBox1.SelectedItem = subjectsdata.LastSubject;
                }
            }
        }

        private void Label1_Click(object sender, EventArgs e)
        {
        }

        private void Label2_Click(object sender, EventArgs e)
        {
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void Label5_Click(object sender, EventArgs e)
        {
        }

        private void StartStop(object sender, EventArgs e)
        {
            if (TbStudyTime.Text != "" && TbPauseTime.Text != "")
            {
                if (int.TryParse(TbStudyTime.Text, out var st) && int.TryParse(TbPauseTime.Text, out var pt))
                {
                    session.UpdateStudyPauseTime(st * 60000, pt * 60000);
                    if (session.Interrupt) session.Run();
                    else session.Stop();
                }
                else
                {
                    MessageBox.Show("Use only non character input for the time", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("study time & pause time are blank fill them before starting the timer", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetButton(object sender, EventArgs e)
        {
            if (session != null)
            {
                TbPauseTime.Text = "";
                TbStudyTime.Text = "";

                if (!session.Interrupt) session.Stop();
                session.Reset();
                session.UpdateStudyPauseTime(1, 1);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (session != null)
            {
                session.Subject = comboBox1.SelectedItem.ToString();
            }
        }
    }
}