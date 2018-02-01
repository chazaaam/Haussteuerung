using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Haussteuerung
{
    public partial class frm_main : Form
    {
        public frm_heating heating;

        public frm_main()
        {
            InitializeComponent();
        }

        private void btn_heat_Click(object sender, EventArgs e)
        {
            if (heating.IsDisposed)
            {
                heating = new frm_heating();
                heating.Show();
            }
            heating.WindowState = FormWindowState.Maximized;
        }

        private void frm_main_Load(object sender, EventArgs e)
        {
            heating = new frm_heating();
            heating.Show();
            heating.WindowState = FormWindowState.Minimized;

            Thread alerts_thread = new Thread(new ThreadStart(alerts_check));
            alerts_thread.IsBackground = true;
            alerts_thread.Start();
        }

        public void alerts_check()
        {
            while(true)
            {
                try
                {

                    if (heating.IsDisposed)
                    {
                        Func<int> del_alert = delegate ()
                        {
                            txt_alert.Text = "Heizungsüberwachung wurde geschlossen. Bitte neu starten!";
                            return 0;
                        };
                        Invoke(del_alert);
                    }
                    else
                    {
                        Func<int> del_alert = delegate ()
                        {
                            txt_alert.Text = "";
                            return 0;
                        };
                        Invoke(del_alert);
                    }
                }
                catch (Exception ex) { }
            }
        }
    }
}
