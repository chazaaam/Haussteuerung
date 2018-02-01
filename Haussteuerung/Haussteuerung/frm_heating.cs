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
using RaspberryPiDotNet;

namespace Haussteuerung
{
    public partial class frm_heating : Form
    {
        // Variables

        //kitchen
        public bool kitchen_aktiv;
        public int kitchen_ist;
        public int kitchen_soll;
        public DateTime kitchen_time;
        public bool kitchen_on;

        //living
        public bool living_aktiv;
        public int living_ist;
        public int living_soll;
        public DateTime living_time;
        public bool living_on;

        //bathbot
        public bool bathbot_aktiv;
        public int bathbot_ist;
        public int bathbot_soll;
        public DateTime bathbot_time;
        public bool bathbot_on;

        //corridor
        public bool corridor_aktiv;
        public int corridor_ist;
        public int corridor_soll;
        public DateTime corridor_time;
        public bool corridor_on;

        //office
        public bool office_aktiv;
        public int office_ist;
        public int office_soll;
        public DateTime office_time;
        public bool office_on;

        //bathtop
        public bool bathtop_aktiv;
        public int bathtop_ist;
        public int bathtop_soll;
        public DateTime bathtop_time;
        public bool bathtop_on;

        //parents
        public bool parents_aktiv;
        public int parents_ist;
        public int parents_soll;
        public DateTime parents_time;
        public bool parents_on;

        //stefan
        public bool stefan_aktiv;
        public int stefan_ist;
        public int stefan_soll;
        public DateTime stefan_time;
        public bool stefan_on;

        //andrea
        public bool andrea_aktiv;
        public int andrea_ist;
        public int andrea_soll;
        public DateTime andrea_time;
        public bool andrea_on;

        public Color color_on = Color.Green;
        public Color color_off = Color.Red;

        List<GPIOMem> pin_liste;

        public frm_heating()
        {
            InitializeComponent();
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void frm_heating_Load(object sender, EventArgs e)
        {
            try
            {
                soll_andrea.Text = Properties.Settings.Default.andrea_soll.ToString();
                soll_bathbot.Text = Properties.Settings.Default.bathbot_soll.ToString();
                soll_bathtop.Text = Properties.Settings.Default.bathtop_soll.ToString();
                soll_corridor.Text = Properties.Settings.Default.corridor_soll.ToString();
                soll_kitchen.Text = Properties.Settings.Default.kitchen_soll.ToString();
                soll_living.Text = Properties.Settings.Default.living_soll.ToString();
                soll_office.Text = Properties.Settings.Default.office_soll.ToString();
                soll_parents.Text = Properties.Settings.Default.parents_soll.ToString();
                soll_stefan.Text = Properties.Settings.Default.stefan_soll.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Soll Temperaturen. Bitte erneut eingeben");
            }

            try
            {
                cmb_andrea.SelectedIndex = Properties.Settings.Default.andrea_type;
                cmb_bathbot.SelectedIndex = Properties.Settings.Default.bathbot_type;
                cmb_bathtop.SelectedIndex = Properties.Settings.Default.bathtop_type;
                cmb_corridor.SelectedIndex = Properties.Settings.Default.corridor_type;
                cmb_kitchen.SelectedIndex = Properties.Settings.Default.kitchen_type;
                cmb_living.SelectedIndex = Properties.Settings.Default.living_type;
                cmb_office.SelectedIndex = Properties.Settings.Default.office_type;
                cmb_parents.SelectedIndex = Properties.Settings.Default.parents_type;
                cmb_stefan.SelectedIndex = Properties.Settings.Default.stefan_type;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Feler beim Laden der Automatisierungsart. Bitte erneut einstellen");
            }

            try
            {
                date_andrea.Value = Properties.Settings.Default.andrea_date;
                date_bathbot.Value = Properties.Settings.Default.bathbot_date;
                date_bathtop.Value = Properties.Settings.Default.bathtop_date;
                date_corridor.Value = Properties.Settings.Default.corridor_date;
                date_kitchen.Value = Properties.Settings.Default.kitchen_date;
                date_living.Value = Properties.Settings.Default.living_date;
                date_office.Value = Properties.Settings.Default.office_date;
                date_parents.Value = Properties.Settings.Default.parents_date;
                date_stefan.Value = Properties.Settings.Default.stefan_date;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Automatisierungszeiten. Bitte erneut einstellen");
            }

            soll_kitchen.SelectionAlignment = ist_kitchen.SelectionAlignment = soll_living.SelectionAlignment = ist_living.SelectionAlignment = soll_bathbot.SelectionAlignment = ist_bathbot.SelectionAlignment = soll_corridor.SelectionAlignment = ist_corridor.SelectionAlignment = soll_office.SelectionAlignment = ist_office.SelectionAlignment = soll_bathtop.SelectionAlignment = ist_bathtop.SelectionAlignment = soll_parents.SelectionAlignment = ist_parents.SelectionAlignment = soll_stefan.SelectionAlignment = ist_stefan.SelectionAlignment = soll_andrea.SelectionAlignment = ist_andrea.SelectionAlignment = HorizontalAlignment.Center;

            Thread heater_thread = new Thread(new ThreadStart (heating_check));
            heater_thread.IsBackground = true;
            heater_thread.Start();

            Thread timing_thread = new Thread(new ThreadStart(timing_check));
            timing_thread.IsBackground = true;
            timing_thread.Start();
        }

        private void timing_check()
        {
            while(true)
            {
                DateTime today = DateTime.Today;

                if(Properties.Settings.Default.kitchen_type == 2)
                {
                    if(DateTime.Compare(today, date_kitchen.Value.Date) >= 0)
                    {
                        kitchen_aktiv = true;
                    }
                    else
                    {
                        kitchen_aktiv = false;
                    }
                }

                if (Properties.Settings.Default.living_type == 2)
                {
                    if (DateTime.Compare(today, date_living.Value.Date) >= 0)
                    {
                        living_aktiv = true;
                    }
                    else
                    {
                        living_aktiv = false;
                    }
                }

                if (Properties.Settings.Default.bathbot_type == 2)
                {
                    if (DateTime.Compare(today, date_bathbot.Value.Date) >= 0)
                    {
                        bathbot_aktiv = true;
                    }
                    else
                    {
                        bathbot_aktiv = false;
                    }
                }

                if (Properties.Settings.Default.corridor_type == 2)
                {
                    if (DateTime.Compare(today, date_corridor.Value.Date) >= 0)
                    {
                        corridor_aktiv = true;
                    }
                    else
                    {
                        corridor_aktiv = false;
                    }
                }

                if (Properties.Settings.Default.office_type == 2)
                {
                    if (DateTime.Compare(today, date_office.Value.Date) >= 0)
                    {
                        office_aktiv = true;
                    }
                    else
                    {
                        office_aktiv = false;
                    }
                }

                if (Properties.Settings.Default.bathtop_type == 2)
                {
                    if (DateTime.Compare(today, date_bathtop.Value.Date) >= 0)
                    {
                        bathtop_aktiv = true;
                    }
                    else
                    {
                        bathtop_aktiv = false;
                    }
                }

                if (Properties.Settings.Default.parents_type == 2)
                {
                    if (DateTime.Compare(today, date_parents.Value.Date) >= 0)
                    {
                        parents_aktiv = true;
                    }
                    else
                    {
                        parents_aktiv = false;
                    }
                }

                if (Properties.Settings.Default.stefan_type == 2)
                {
                    if (DateTime.Compare(today, date_stefan.Value.Date) >= 0)
                    {
                        stefan_aktiv = true;
                    }
                    else
                    {
                        stefan_aktiv = false;
                    }
                }

                if (Properties.Settings.Default.andrea_type == 2)
                {
                    if (DateTime.Compare(today, date_andrea.Value.Date) >= 0)
                    {
                        andrea_aktiv = true;
                    }
                    else
                    {
                        andrea_aktiv = false;
                    }
                }
            }
        }

        private void heating_check()
        {
            while (true)
            {
                // kitchen

                if (kitchen_aktiv)
                {
                    if (kitchen_soll > kitchen_ist)
                        kitchen_on = true;
                    else
                        kitchen_on = false;
                }
                else
                    kitchen_on = false;

                if (kitchen_on)
                    color_change(pnl_kitchen, true);
                else
                    color_change(pnl_kitchen, false);

                // living
                if (living_aktiv)
                {
                    if (living_soll > living_ist)
                        living_on = true;
                    else
                        living_on = false;
                }
                else
                    living_on = false;

                if (living_on)
                    color_change(pnl_living, true);
                else
                    color_change(pnl_living, false);

                // bathbot
                if (bathbot_aktiv)
                {
                    if (bathbot_soll > bathbot_ist)
                        bathbot_on = true;
                    else
                        bathbot_on = false;
                }
                else
                    bathbot_on = false;

                if (bathbot_on)
                    color_change(pnl_bathbot, true);
                else
                    color_change(pnl_bathbot, false);

                // corridor
                if (corridor_aktiv)
                {
                    if (corridor_soll > corridor_ist)
                        corridor_on = true;
                    else
                        corridor_on = false;
                }
                else
                    corridor_on = false;

                if (corridor_on)
                    color_change(pnl_corridor, true);
                else
                    color_change(pnl_corridor, false);

                // office
                if (office_aktiv)
                {
                    if (office_soll > office_ist)
                        office_on = true;
                    else
                        office_on = false;
                }
                else
                    office_on = false;

                if (office_on)
                    color_change(pnl_office, true);
                else
                    color_change(pnl_office, false);

                // bathtop
                if (bathtop_aktiv)
                {
                    if (bathtop_soll > bathtop_ist)
                        bathtop_on = true;
                    else
                        bathtop_on = false;
                }
                else
                    bathtop_on = false;

                if (bathtop_on)
                    color_change(pnl_bathtop, true);
                else
                    color_change(pnl_bathtop, false);

                // parents
                if (parents_aktiv)
                {
                    if (parents_soll > parents_ist)
                        parents_on = true;
                    else
                        parents_on = false;
                }
                else
                    parents_on = false;

                if (parents_on)
                    color_change(pnl_parents, true);
                else
                    color_change(pnl_parents, false);

                // stefan
                if (stefan_aktiv)
                {
                    if (stefan_soll > stefan_ist)
                        stefan_on = true;
                    else
                        stefan_on = false;
                }
                else
                    stefan_on = false;

                if (stefan_on)
                    color_change(pnl_stefan, true);
                else
                    color_change(pnl_stefan, false);

                // andrea
                if (andrea_aktiv)
                {
                    if (andrea_soll > andrea_ist)
                        andrea_on = true;
                    else
                        andrea_on = false;
                }
                else
                    andrea_on = false;

                if (andrea_on)
                    color_change(pnl_andrea, true);
                else
                    color_change(pnl_andrea, false);
            }

        }

        public void color_change(Panel pnl, bool status)
        {
            try
            {
                Func<int> del_color = delegate ()
                {
                    if (status)
                        pnl.BackColor = color_on;
                    else
                        pnl.BackColor = color_off;

                    return 0;
                };
                Invoke(del_color);
            }
            catch (Exception ex) { };

        }

        private void cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_kitchen.SelectedIndex == 1)
            {
                kitchen_aktiv = true;
                Properties.Settings.Default.kitchen_type = 1;
            }
            else if (cmb_kitchen.SelectedIndex == 2)
            {
                Properties.Settings.Default.kitchen_type = 2;
            }
            else if(cmb_kitchen.SelectedIndex == 0)
            {
                kitchen_aktiv = false;
                Properties.Settings.Default.kitchen_type = 0;
            }

            if (cmb_living.SelectedIndex == 1)
            {
                living_aktiv = true;
                Properties.Settings.Default.living_type = 1;
            }
            else if (cmb_living.SelectedIndex == 2)
            {
                Properties.Settings.Default.living_type = 2;
            }
            else if (cmb_living.SelectedIndex == 0)
            {
                living_aktiv = false;
                Properties.Settings.Default.living_type = 0;
            }

            if (cmb_bathbot.SelectedIndex == 1)
            {
                bathbot_aktiv = true;
                Properties.Settings.Default.bathbot_type = 1;
            }
            else if (cmb_bathbot.SelectedIndex == 2)
            {
                Properties.Settings.Default.bathbot_type = 2;
            }
            else if (cmb_bathbot.SelectedIndex == 0)
            {
                bathbot_aktiv = false;
                Properties.Settings.Default.bathbot_type = 0;
            }


            if (cmb_corridor.SelectedIndex == 1)
            {
                corridor_aktiv = true;
                Properties.Settings.Default.corridor_type = 1;
            }
            else if (cmb_corridor.SelectedIndex == 2)
            {
                Properties.Settings.Default.corridor_type = 2;
            }
            else if (cmb_corridor.SelectedIndex == 0)
            {
                corridor_aktiv = false;
                Properties.Settings.Default.corridor_type = 0;
            }

            if (cmb_office.SelectedIndex == 1)
            {
                office_aktiv = true;
                Properties.Settings.Default.office_type = 1;
            }
            else if(cmb_office.SelectedIndex == 2)
            {
                Properties.Settings.Default.office_type = 2;
            }
            else if (cmb_office.SelectedIndex == 0)
            { 
                office_aktiv = false;
                Properties.Settings.Default.office_type = 0;
            }

            if (cmb_bathtop.SelectedIndex == 1)
            {
                bathtop_aktiv = true;
                Properties.Settings.Default.bathtop_type = 1;
            }
            else if(cmb_bathtop.SelectedIndex == 2)
            {
                Properties.Settings.Default.bathtop_type = 2;
            }
            else if (cmb_bathtop.SelectedIndex == 0)
            {
                bathtop_aktiv = false;
                Properties.Settings.Default.bathtop_type = 0;
            }

            if (cmb_parents.SelectedIndex == 1)
            {
                parents_aktiv = true;
                Properties.Settings.Default.parents_type = 1;
            }
            else if (cmb_parents.SelectedIndex == 2)
            {
                Properties.Settings.Default.parents_type = 2;
            }
            else if (cmb_parents.SelectedIndex == 0)
            {
                parents_aktiv = false;
                Properties.Settings.Default.parents_type = 0;
            }

            if (cmb_stefan.SelectedIndex == 1)
            {
                stefan_aktiv = true;
                Properties.Settings.Default.stefan_type = 1;
            }
            else if (cmb_stefan.SelectedIndex == 2)
            {
                Properties.Settings.Default.stefan_type = 2;
            }
            else if (cmb_stefan.SelectedIndex == 0)
            {
                stefan_aktiv = false;
                Properties.Settings.Default.stefan_type = 0;
            }

            if (cmb_andrea.SelectedIndex == 1)
            {
                andrea_aktiv = true;
                Properties.Settings.Default.andrea_type = 1;
            }
            else if (cmb_andrea.SelectedIndex == 2)
            {
                Properties.Settings.Default.andrea_type = 2;
            }
            else if (cmb_andrea.SelectedIndex == 0)
            {
                andrea_aktiv = false;
                Properties.Settings.Default.andrea_type = 0;
            }

            Properties.Settings.Default.Save();
        }

        private void soll_TextChanged(object sender, EventArgs e)
        {
            RichTextBox temptext = (RichTextBox)sender;
            switch(temptext.Name)
            {
                case "soll_kitchen":
                    Properties.Settings.Default.kitchen_soll = Int32.Parse(temptext.Text);                    
                    break;
                case "soll_living":
                    Properties.Settings.Default.living_soll = Int32.Parse(temptext.Text);
                    break;
                case "soll_bathbot":
                    Properties.Settings.Default.bathbot_soll = Int32.Parse(temptext.Text);
                    break;
                case "soll_corridor":
                    Properties.Settings.Default.corridor_soll = Int32.Parse(temptext.Text);
                    break;
                case "soll_office":
                    Properties.Settings.Default.office_soll = Int32.Parse(temptext.Text);
                    break;
                case "soll_bathtop":
                    Properties.Settings.Default.bathtop_soll = Int32.Parse(temptext.Text);
                    break;
                case "soll_parents":
                    Properties.Settings.Default.parents_soll = Int32.Parse(temptext.Text);
                    break;
                case "soll_stefan":
                    Properties.Settings.Default.stefan_soll = Int32.Parse(temptext.Text);
                    break;
                case "soll_andrea":
                    Properties.Settings.Default.andrea_soll = Int32.Parse(temptext.Text);
                    break;
            }
            Properties.Settings.Default.Save();
        }


        #region UpdOwnHandling
        private void up_kitchen_Click(object sender, EventArgs e)
        {
            soll_kitchen.Text = (Int32.Parse(soll_kitchen.Text) + 1).ToString();
            soll_kitchen.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.kitchen_soll = Int32.Parse(soll_kitchen.Text);
            kitchen_soll = Int32.Parse(soll_kitchen.Text);
            Properties.Settings.Default.Save();
        }

        private void down_kitchen_Click(object sender, EventArgs e)
        {
            soll_kitchen.Text = (Int32.Parse(soll_kitchen.Text) - 1).ToString();
            soll_kitchen.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.kitchen_soll = Int32.Parse(soll_kitchen.Text);
            kitchen_soll = Int32.Parse(soll_kitchen.Text);
            Properties.Settings.Default.Save();
        }

        private void up_living_Click(object sender, EventArgs e)
        {
            soll_living.Text = (Int32.Parse(soll_living.Text) + 1).ToString();
            soll_living.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.living_soll = Int32.Parse(soll_living.Text);
            living_soll = Int32.Parse(soll_living.Text);
            Properties.Settings.Default.Save();
        }

        private void down_living_Click(object sender, EventArgs e)
        {
            soll_living.Text = (Int32.Parse(soll_living.Text) - 1).ToString();
            soll_living.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.living_soll = Int32.Parse(soll_living.Text);
            living_soll = Int32.Parse(soll_living.Text);
            Properties.Settings.Default.Save();
        }

        private void up_bathbot_Click(object sender, EventArgs e)
        {
            soll_bathbot.Text = (Int32.Parse(soll_bathbot.Text) + 1).ToString();
            soll_bathbot.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.bathbot_soll = Int32.Parse(soll_bathbot.Text);
            bathbot_soll = Int32.Parse(soll_bathbot.Text);
            Properties.Settings.Default.Save();
        }

        private void down_bathbot_Click(object sender, EventArgs e)
        {
            soll_bathbot.Text = (Int32.Parse(soll_bathbot.Text) - 1).ToString();
            soll_bathbot.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.bathbot_soll = Int32.Parse(soll_bathbot.Text);
            bathbot_soll = Int32.Parse(soll_bathbot.Text);
            Properties.Settings.Default.Save();
        }

        private void up_corridor_Click(object sender, EventArgs e)
        {
            soll_corridor.Text = (Int32.Parse(soll_corridor.Text) + 1).ToString();
            soll_corridor.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.corridor_soll = Int32.Parse(soll_corridor.Text);
            corridor_soll = Int32.Parse(soll_corridor.Text);
            Properties.Settings.Default.Save();
        }

        private void down_corridor_Click(object sender, EventArgs e)
        {
            soll_corridor.Text = (Int32.Parse(soll_corridor.Text) - 1).ToString();
            soll_corridor.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.corridor_soll = Int32.Parse(soll_corridor.Text);
            corridor_soll = Int32.Parse(soll_corridor.Text);
            Properties.Settings.Default.Save();
        }

        private void up_office_Click(object sender, EventArgs e)
        {
            soll_office.Text = (Int32.Parse(soll_office.Text) + 1).ToString();
            soll_office.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.office_soll = Int32.Parse(soll_office.Text);
            office_soll = Int32.Parse(soll_office.Text);
            Properties.Settings.Default.Save();
        }

        private void down_office_Click(object sender, EventArgs e)
        {
            soll_office.Text = (Int32.Parse(soll_office.Text) - 1).ToString();
            soll_office.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.office_soll = Int32.Parse(soll_office.Text);
            office_soll = Int32.Parse(soll_office.Text);
            Properties.Settings.Default.Save();
        }

        private void up_bathtop_Click(object sender, EventArgs e)
        {
            soll_bathtop.Text = (Int32.Parse(soll_bathtop.Text) + 1).ToString();
            soll_bathtop.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.bathtop_soll = Int32.Parse(soll_bathtop.Text);
            bathtop_soll = Int32.Parse(soll_bathtop.Text);
            Properties.Settings.Default.Save();
        }

        private void down_bathtop_Click(object sender, EventArgs e)
        {
            soll_bathtop.Text = (Int32.Parse(soll_bathtop.Text) - 1).ToString();
            soll_bathtop.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.bathtop_soll = Int32.Parse(soll_bathtop.Text);
            bathtop_soll = Int32.Parse(soll_bathtop.Text);
            Properties.Settings.Default.Save();
        }

        private void up_parents_Click(object sender, EventArgs e)
        {
            soll_parents.Text = (Int32.Parse(soll_parents.Text) + 1).ToString();
            soll_parents.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.parents_soll = Int32.Parse(soll_parents.Text);
            parents_soll = Int32.Parse(soll_parents.Text);
            Properties.Settings.Default.Save();
        }

        private void down_parents_Click(object sender, EventArgs e)
        {
            soll_parents.Text = (Int32.Parse(soll_parents.Text) - 1).ToString();
            soll_parents.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.parents_soll = Int32.Parse(soll_parents.Text);
            parents_soll = Int32.Parse(soll_parents.Text);
            Properties.Settings.Default.Save();
        }

        private void up_stefan_Click(object sender, EventArgs e)
        {
            soll_stefan.Text = (Int32.Parse(soll_stefan.Text) + 1).ToString();
            soll_stefan.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.stefan_soll = Int32.Parse(soll_stefan.Text);
            stefan_soll = Int32.Parse(soll_stefan.Text);
            Properties.Settings.Default.Save();
        }

        private void down_stefan_Click(object sender, EventArgs e)
        {
            soll_stefan.Text = (Int32.Parse(soll_stefan.Text) - 1).ToString();
            soll_stefan.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.stefan_soll = Int32.Parse(soll_stefan.Text);
            stefan_soll = Int32.Parse(soll_stefan.Text);
            Properties.Settings.Default.Save();
        }

        private void up_andrea_Click(object sender, EventArgs e)
        {
            soll_andrea.Text = (Int32.Parse(soll_andrea.Text) + 1).ToString();
            soll_andrea.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.andrea_soll = Int32.Parse(soll_andrea.Text);
            andrea_soll = Int32.Parse(soll_andrea.Text);
            Properties.Settings.Default.Save();
        }

        private void down_andrea_Click(object sender, EventArgs e)
        {
            soll_andrea.Text = (Int32.Parse(soll_andrea.Text) - 1).ToString();
            soll_andrea.SelectionAlignment = HorizontalAlignment.Center;
            Properties.Settings.Default.andrea_soll = Int32.Parse(soll_andrea.Text);
            andrea_soll = Int32.Parse(soll_andrea.Text);
            Properties.Settings.Default.Save();
        }


        #endregion

        private void date_kitchen_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.kitchen_date = date_kitchen.Value;
            Properties.Settings.Default.Save();
        }

        private void date_living_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.living_date = date_living.Value;
            Properties.Settings.Default.Save();
        }

        private void date_bathbot_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.bathbot_date = date_bathbot.Value;
            Properties.Settings.Default.Save();
        }

        private void date_corridor_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.corridor_date = date_corridor.Value;
            Properties.Settings.Default.Save();
        }

        private void date_office_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.office_date = date_office.Value;
            Properties.Settings.Default.Save();
        }

        private void date_bathtop_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.bathtop_date = date_bathtop.Value;
            Properties.Settings.Default.Save();
        }

        private void date_parents_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.parents_date = date_parents.Value;
            Properties.Settings.Default.Save();
        }

        private void date_stefan_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.stefan_date = date_stefan.Value;
            Properties.Settings.Default.Save();
        }

        private void date_andrea_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.andrea_date = date_andrea.Value;
            Properties.Settings.Default.Save();
        }
    }
}
