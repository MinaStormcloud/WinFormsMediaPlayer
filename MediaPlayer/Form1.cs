using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;

namespace MediaPlayer
{
    public partial class Form1 : Form
    {
        Timer timer = new Timer();

        public Form1()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(timer_Tick);
        }

        string[] files;
        List<string> path = new List<string>();

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        //Start the media player
        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) //Select a file to play
        {
            axWindowsMediaPlayer1.URL = path[listBox1.SelectedIndex]; //Works with listBox1.Items.Add(files[i]);
        }

        private void timer_Tick(object sender, EventArgs e) //The timer method
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
            timer.Enabled = false;
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            /*media player states:
            0 = Undefined
            1 = Stopped (by User)
            2 = Paused
            3 = Playing
            4 = Scan Forward
            5 = Scan Backwards
            6 = Buffering
            7 = Waiting
            8 = Media Ended
            9 = Transitioning
            10 = Ready
            11 = Reconnecting
            12 = Last*/

            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsMediaEnded) //What to do when a track ends
            {
                timer.Interval = 100;
                timer.Start();
                timer.Enabled = true;
                timer.Tick += timer_Tick;

                if (listBox1.Items.Count == 0)//What to do if the list is cleared during playback
                {
                    //axWindowsMediaPlayer1.URL = openFileDialog1.FileName; //Plays the first track in the cleared list
                    axWindowsMediaPlayer1.Ctlcontrols.play(); //Repeats the current track when the list has been cleared
                }

                else if (listBox1.SelectedIndex == listBox1.Items.Count - 1) //What to do after playing the last item in the playlist
                {
                    //timer.Stop(); //Stops the playback at the end of the playlist
                    listBox1.SelectedIndex = 0;//Loops the playlist if the timer isn't stopped
                }

                else
                {
                    listBox1.SelectedIndex = listBox1.SelectedIndex + 1; //Skips to the next track in the playlist
                }
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e) //The File menu
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) //Menu option
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                files = openFileDialog1.SafeFileNames;
                path.AddRange(openFileDialog1.FileNames.ToList()); //Adds paths when files are added to the playlist
                for (int i = 0; i < files.Length; i++)
                {
                    listBox1.Items.Add(files[i]); //Adds files to the playlist, only file names are visible in the list box
                }
            }
        }

        private void clearPlaylistToolStripMenuItem_Click(object sender, EventArgs e) //Menu option
        {
            listBox1.Items.Clear(); //Clears the list box
            path.Clear(); //Clears the paths
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) //Menu option
        {
            Application.Exit();
        }
    }
}