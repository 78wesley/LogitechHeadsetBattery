using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.IO;
using System.Linq;


namespace Battery
{
    public partial class Form1 : Form
    {
        last_battery_status lbs = new last_battery_status();
        string jstream = null;
        string settings = null;
        string[] SettingItems;
        string[] JItems;
        string[] InJItems;

        string ls = null;
        string perc = null;
        string remain = null;
        string time = null;


        public Form1()
        {
            InitializeComponent();
        }
        //public void 
        /// <summary>
        /// Laat alleen leeg als je ga debuggen zonder de hardware ;)
        /// </summary>
        /// <param name="Filename"></param>
        public void LoadJson(string Filename =@"settings.json")
        {
            
            try
            {
                #region Bullshit
                /*
                TextReader textReader = File.OpenText(@"Settings.json");
                JsonTextReader JTR = new JsonTextReader(textReader);
                */
                /*using (StreamReader r = new StreamReader("settings.json"))
                {
                    string json = r.ReadToEnd();
                    List<last_battery_status> Items = JsonConvert.DeserializeObject<List<last_battery_status>>(json);
                   // dynamic array = JsonConvert.DeserializeObject(json);
                    foreach (var item in Items)
                    {
                        Console.WriteLine("{0}", item);
                    }
                }*/
                #endregion
                StreamReader StR = File.OpenText(Filename);
                jstream = StR.ReadToEnd();
                StR.Close();
                JItems = jstream.Split(new char[] { '}' });
                string Block = Array.Find(JItems, s => s.Contains("\"4294967295\"")) + "}";
                InJItems = Block.Split(new char[] { '\n' });
                ls = Array.Find(InJItems, s => s.Contains("\"last_state\""));
                ls = ls.Split(new char[] { ':' })[1].Split(new char[] { '"' })[1];
                perc = Array.Find(InJItems, s => s.Contains("\"percentage\""));
                perc = perc.Split(new char[] { ':' })[1].Split(new char[] { ',' })[0]; ;
                remain = Array.Find(InJItems, s => s.Contains("\"remainminutes\""));
                remain = remain.Split(new char[] { ':' })[1].Split(new char[] { ',' })[0]; ;
                time = Array.Find(InJItems, s => s.Contains("\"time\""));
                time = time.Split(new char[] { ':' })[1].Split(new char[] { ',' })[0]; ;
                lbs.percentage = int.Parse(perc);
                lbs.last_state = ls;
                lbs.remainminutes = int.Parse(remain);
                #region CMD
                /* Console.Write("--BLOCK------------------------------------\n" +
                     "{0}\n" +
                     "-------------------------------------------\n" +
                     "Last State    | {1}\n" +
                     "Percentage    | {2}\n" +
                     "Remainminutes | {3}\n" +
                     "Time          | {4}\n" +
                     "-------------------------------------------", Block, ls, perc, remain, time);
                     */
                #endregion CMD
                GetValues();
                //MessageBox.Show(lbs.last_state);

            }
            catch(FileNotFoundException)
            {
                MessageBox.Show(string.Format("Can't find \"{0}\"", Filename), "Error!");
                Application.Exit();
            }
            catch(Exception)
            {

            }


        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //GetValues(trackBar1.Value);
            //GetValues(lbs.percentage);
        }
        private void GetValues()
        {
            Battery_Meter(lbs.percentage);
            string status;
            if (lbs.last_state == "discharge")
            {
                status = "Discharging"; 
            }
            else
            {
                status = "Charging";
            }
            label1.Text = string.Format("Battery: {0}%\nHours Remaining: {1}\nStatus: {2}", lbs.percentage, lbs.remainminutes/60, status);
            
            
            
            if (lbs.last_state == "discharge")
            {
                notifyIcon1.Text = string.Format("Your headset is at {0}%\nDischarging\n{1} hours remaining!", lbs.percentage, lbs.remainminutes/60);
            }
            else
            {
                notifyIcon1.Text = string.Format("Your headset is at {0}%\nCharging", lbs.percentage);
            }
        }
        private void Battery_Meter(int Percent)
        {
            if (Percent <= 100 && Percent >= 81)
            {
                this.notifyIcon1.Icon = System.Drawing.Icon.ExtractAssociatedIcon("icons\\1.ico");
            }
            if (Percent <= 80 && Percent >= 61)
            {
                this.notifyIcon1.Icon = System.Drawing.Icon.ExtractAssociatedIcon("icons\\2.ico");
            }
            if (Percent <= 60 && Percent >= 41)
            {
                this.notifyIcon1.Icon = System.Drawing.Icon.ExtractAssociatedIcon("icons\\3.ico");
            }
            if (Percent <= 40 && Percent >= 21)
            {
                this.notifyIcon1.Icon = System.Drawing.Icon.ExtractAssociatedIcon("icons\\4.ico");
            }
            if (Percent <= 20 && Percent >= 11)
            {
                this.notifyIcon1.Icon = System.Drawing.Icon.ExtractAssociatedIcon("icons\\5.ico");
            }
            if (Percent <= 10 && Percent >= 6)
            {
                this.notifyIcon1.Icon = System.Drawing.Icon.ExtractAssociatedIcon("icons\\6.ico");
            }
            if (Percent <= 5)
            {
                this.notifyIcon1.Icon = System.Drawing.Icon.ExtractAssociatedIcon("icons\\7.ico");
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void rcsettings_Click(object sender, EventArgs e)
        {

        }

        private void rcexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           // this.ClientSize = new System.Drawing.Size(212, 89);
           // WindowState = FormWindowState.Normal;
            //Opacity = 1;
            Visible = true;
            this.hideToolStripMenuItem.Text = "Hide";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                minimize();
            }

        }
        void minimize()
        {
                //Opacity = 0;
                //WindowState = FormWindowState.Minimized;
                Visible = false;
            this.hideToolStripMenuItem.Text = "Show";
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

#if DEBUG
            LoadJson();
#endif
#if !DEBUG
            LoadJson(String.Format("C:\\users\\{0}\\appdata\\local\\logitech\\logitech Gaming Software\\settings.json", Environment.UserName));
#endif
           
        }

        private void repeater_Tick(object sender, EventArgs e)
        {
#if DEBUG
            LoadJson();
#endif
#if !DEBUG
            LoadJson(String.Format("C:\\users\\{0}\\appdata\\local\\logitech\\logitech Gaming Software\\settings.json", Environment.UserName));
#endif
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Visible)
            {
                minimize();
            }
            else
            {
                
               /* WindowState = FormWindowState.Maximized;
                WindowState = FormWindowState.Normal;
                this.ClientSize = new System.Drawing.Size(212, 89);*/
                Visible = true;
                this.hideToolStripMenuItem.Text = "Hide";

            }
        }

        private void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Window state: {0}", WindowState);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
        }

        private void label1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void DelayTimer_Tick(object sender, EventArgs e)
        {
            DelayTimer.Enabled = false;
            DelayTimer.Stop();
            Close();
        }
    }
    public class last_battery_status{
        public string last_state { get; set; }
        public int percentage { get; set; }
        public int remainminutes { get; set;}
        public string time { get; set; }
        }
}
