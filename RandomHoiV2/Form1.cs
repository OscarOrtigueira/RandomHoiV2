using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Threading;


namespace RandomHoiV2
{
    public partial class Form1 : Form
    {

        bool validRefreshThr = true;
        int totalP = 0;

        //ALL TEXT FILES 
        static int numberProvinces = 10608;
        string[] provinces = new string[numberProvinces];        //10608 provinces. this it is and it dont should be changed unless the region.txt has got some new provinces or its to adapt other map.
        string[] provincesAux = new string[numberProvinces];     //aux to order the first and the last province in a random way
        public ArrayList provincesAuxArrayList = new ArrayList();
        public ArrayList provincesAuxArrayListNoDelete = new ArrayList();
        public ArrayList countries = new ArrayList();          //countries
        public ArrayList navalProvinces = new ArrayList();     //provinces that may have a port
        public ArrayList articProvinces = new ArrayList();     //artic provinces
        public ArrayList landTraits = new ArrayList();         //land traits
        public ArrayList airTraits = new ArrayList();          //air traits
        public ArrayList navalTraits = new ArrayList();        //naval traits
        public ArrayList mhTraits = new ArrayList();           //head and first minsiter traits
        public ArrayList mIntelTraits = new ArrayList();
        public ArrayList mForeTraits = new ArrayList();
        public ArrayList mArmamentTraits = new ArrayList();
        public ArrayList mSecurityTraits = new ArrayList();
        public ArrayList mStaffTraits = new ArrayList();
        public ArrayList mArmyTraits = new ArrayList();
        public ArrayList mNavalTraits = new ArrayList();
        public ArrayList mAirTraits = new ArrayList();
        public ArrayList names = new ArrayList();              //names for leaders and ministers
        public ArrayList lastnames = new ArrayList();          //lastnames for leaders and ministers
        public ArrayList ministerPics = new ArrayList();       //ids of the minsiter's pics
        public ArrayList leadersPics = new ArrayList();        //ids of the leaders's pics
        public ArrayList conCountries = new ArrayList();       //all the conmitern countries. 
        public ArrayList alliesCountries = new ArrayList();    //all the allies countries. 
        public ArrayList axisCountries = new ArrayList();      //all the axis countries. 
        public ArrayList provincePacific = new ArrayList();
        public ArrayList provinceVP = new ArrayList();

        public ArrayList preferenceCountries = new ArrayList(); //countries that go first in generation 

        public ArrayList provinceAdj = new ArrayList(); //new generator
        public ArrayList provinceWithOwner = new ArrayList(); //new generator
        public Dictionary<int, province> provincesGeneratedDict = new Dictionary<int, province>();//new generator

        public ArrayList strategic = new ArrayList();

        public Random ran = new Random();// to genereate pseudorandom numbers and stats, its better to inizialize this at the start of the program and always use this. If you want to know why, look the documentation.
        public Random ran2 = new Random(DateTime.Now.Millisecond * 2);


        public ArrayList provincesGenerated = new ArrayList();//array list with provinces object
        public ArrayList countriesGenerated = new ArrayList();//array list with countries object
        public ArrayList countriesPuppetableGenerated = new ArrayList();//array list with puppetale countries object

        //CHECKS
        bool first;                                     //is this the first run of the generation? 

        //FACTIONS
        public string aTag; //allied leader tag
        public string axTag; //axis leader tag
        public string cTag; //con leader tag

        string capitalATag; //allied leader capital
        string capitalAxTag; //axis leader capital
        string capitalCTag; //con leader capital

        //COUNT IDS
        public int unitID;                                     //Id used for units. We dont want to repeat this.
        public int leadersID;                                  //Id used for leaders. We dont want to repeat this.                           
        public int ministersID;                                //Id used for ministers. We dont want to repeat this.

        //stats

        int totalIC = 0;
        double totalMP = 0;
        int totalProvinces = 0;
        double totalLeadership = 0;

        //GENERATING RESOURCES
        public double por_Res = 1;                             //multiplier for resources
        public double por_IC = 1;                              //multiplier for IC
        public double por_MP = 1;                              //multiplier for MP
        public double por_lea = 1;                             //multiplier for leadership
        public double por_oil = 1;                             //multiplier for oil
        public int probOil = 29; //of 31      was 22                        //chance to oil
        public int probOil2 = 99;  //of 61     was 60 of 61                       //chance to oil in non core provinces (higers doesnt mean more chance. Look at the resource generator for probabilities.

        //bool tabCrazy = true; //unused


        public Form1()
        {
            InitializeComponent();

            listView1.Columns.Add(new ColHeader("Tag", 50, HorizontalAlignment.Left, true));
            listView1.Columns.Add(new ColHeader("IC", 40, HorizontalAlignment.Left, true));
            listView1.Columns.Add(new ColHeader("Leader", 60, HorizontalAlignment.Left, true));
            listView1.Columns.Add(new ColHeader("ManPower", 80, HorizontalAlignment.Left, true));
            listView1.Columns.Add(new ColHeader("Provinces", 80, HorizontalAlignment.Left, true));
            listView1.Columns.Add(new ColHeader("Army", 40, HorizontalAlignment.Left, true));
            listView1.Columns.Add(new ColHeader("Naval", 50, HorizontalAlignment.Left, true));
            listView1.Columns.Add(new ColHeader("Air", 40, HorizontalAlignment.Left, true));
            listView1.Columns.Add(new ColHeader("Focus", 50, HorizontalAlignment.Left, true));
            listView1.Columns.Add(new ColHeader("Faction", 60, HorizontalAlignment.Left, true));

            this.listView1.ColumnClick += new ColumnClickEventHandler(listView1_ColumnClick);
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            // Create an instance of the ColHeader class.
            ColHeader clickedCol = (ColHeader)this.listView1.Columns[e.Column];

            // Set the ascending property to sort in the opposite order.
            clickedCol.ascending = !clickedCol.ascending;

            // Get the number of items in the list.
            int numItems = this.listView1.Items.Count;

            // Turn off display while data is repoplulated.
            this.listView1.BeginUpdate();

            // Populate an ArrayList with a SortWrapper of each list item.
            ArrayList SortArray = new ArrayList();
            for (int i = 0; i < numItems; i++)
            {
                SortArray.Add(new SortWrapper(this.listView1.Items[i], e.Column));
            }

            // Sort the elements in the ArrayList using a new instance of the SortComparer
            // class. The parameters are the starting index, the length of the range to sort,
            // and the IComparer implementation to use for comparing elements. Note that
            // the IComparer implementation (SortComparer) requires the sort
            // direction for its constructor; true if ascending, othwise false.
            SortArray.Sort(0, SortArray.Count, new SortWrapper.SortComparer(clickedCol.ascending));

            // Clear the list, and repopulate with the sorted items.
            this.listView1.Items.Clear();
            for (int i = 0; i < numItems; i++)
                this.listView1.Items.Add(((SortWrapper)SortArray[i]).sortItem);

            // Turn display back on.
            this.listView1.EndUpdate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                readFiles();
                comboNOWAR.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + ex.StackTrace, "Read the readme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

        }

        private void readFiles()
        {




            string[] prov;

            String line = "0";

            int numProv;
            int count1;
            count1 = 0;
            numProv = 0;
            int count2;
            count2 = 0;
            int count3;


            Random ra = new Random(); //you ll see a lot of randoms in this code
            FileStream file;
            StreamReader sw;
            try
            {

                file = new FileStream(Application.StartupPath + "/nump.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);


                while (!(sw.EndOfStream))
                {
                    line = sw.ReadLine();

                    provinces = new string[Convert.ToInt32(line)];
                    provincesAux = new string[Convert.ToInt32(line)];
                    numberProvinces = Convert.ToInt32(line);

                }

                sw.Close();

                labenump.Text = "nump.txt detected. " + Convert.ToInt32(line) + " provinces";
            }
            catch (Exception ex)
            {
                if (line != "0")
                {
                    labenump.Text = "error in nump.txt, you need only one single number at the very start of the file. Now using vanilla map";
                }
                provinces = new string[numberProvinces];//default vanilla map
                provincesAux = new string[numberProvinces];//default
            }

            try{
            file = new FileStream(Application.StartupPath + "/region.txt", FileMode.Open, FileAccess.Read);
            sw = new StreamReader(file);

            string[] split = new string[1];
            split[0] = ";";

            first = true;
            comboBox1.SelectedIndex = 1;
  
                #region readFiles
                //we read the provinces from the region.txt
        
                while (!(sw.EndOfStream))
                {
                  
                    line = sw.ReadLine();
                    if (line.Contains("{"))
                    {
                        line = sw.ReadLine();
                        while (!line.Contains("}"))
                        {
                            prov = line.Split(split, StringSplitOptions.RemoveEmptyEntries);

                            for (count1 = 0; count1 <= prov.Length - 1; count1++)
                            {
                                bool repe = false;
                                for (count3 = 0; count3 <= provincesAux.Length - 1; count3++)
                                {
                                    if (repe== false && prov[count1] == provincesAux[count3])
                                    {
                                      //  MessageBox.Show("Some provinces are duplicated! You can run the mod anyway, but this is a warning.");
                                        repe = true;
                                        
                                    }
                                }
                                if (!repe)
                                {
                                    provincesAuxArrayList.Add(prov[count1]);
                                    provincesAuxArrayListNoDelete.Add(prov[count1]);
                                    provincesAux[numProv] = prov[count1];

                                    numProv++;
                                }

                            }
                            line = sw.ReadLine();

                        }
                    }

                }
                sw.Close();
    

                //starting province
                int ini;
                ini = ra.Next(0, provincesAux.Length - 5);

                //we copy the provinces in the new order
                for (count1 = ini; count1 <= provincesAux.Length - 1; count1++)
                {
                    provinces[count2] = provincesAux[count1];
                    count2++;
                }

                for (count1 = 0; count1 <= ini - 1; count1++)
                {
                    provinces[count2] = provincesAux[count1];
                    count2++;
                }

                count2 = 0;


                //READ THE REST OF THE FILES

                file = new FileStream(Application.StartupPath + "/strategic_resources.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);


                while (!(sw.EndOfStream))
                {
                    strategic.Add(sw.ReadLine());
                }

                sw.Close();

                file = new FileStream(Application.StartupPath + "/preferenceCountries.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);


                while (!(sw.EndOfStream))
                {
                    preferenceCountries.Add(sw.ReadLine());
                }

                sw.Close();


                file = new FileStream(Application.StartupPath + "/coastal_provinces.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);


                while (!(sw.EndOfStream))
                {
                    navalProvinces.Add(sw.ReadLine());
                }

                sw.Close();

                file = new FileStream(Application.StartupPath + "/vp.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);


                while (!(sw.EndOfStream))
                {
                    provinceVP.Add(sw.ReadLine());
                }

                sw.Close();


                file = new FileStream(Application.StartupPath + "/newGenerator.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);


                while (!(sw.EndOfStream))
                {


                    provinceAdj.Add(sw.ReadLine());
                }

                sw.Close();


                file = new FileStream(Application.StartupPath + "/arctic.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);


                while (!(sw.EndOfStream))
                {
                    articProvinces.Add(sw.ReadLine());
                }


                sw.Close();

                file = new FileStream(Application.StartupPath + "/countries.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {
                    string lineco = sw.ReadLine().Substring(0, 3);
                    if (!countries.Contains(lineco))
                    {
                        countries.Add(lineco);
                    }

                }

                sw.Close();


                file = new FileStream(Application.StartupPath + "/pacific.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);


                string[] provP = sw.ReadLine().Split(split, StringSplitOptions.RemoveEmptyEntries);

                for (int contaPac = 0; contaPac <= provP.Length - 1; contaPac++)
                {
                    provincePacific.Add(provP[contaPac]);
                }



                sw.Close();


                file = new FileStream(Application.StartupPath + "/traits.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    landTraits.Add(sw.ReadLine());


                }

                sw.Close();

                file = new FileStream(Application.StartupPath + "/Airtraits.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    airTraits.Add(sw.ReadLine());


                }

                sw.Close();

                file = new FileStream(Application.StartupPath + "/Navaltraits.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    navalTraits.Add(sw.ReadLine());


                }

                sw.Close();


                file = new FileStream(Application.StartupPath + "/minister_types_head.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    mhTraits.Add(sw.ReadLine());


                }

                sw.Close();

                file = new FileStream(Application.StartupPath + "/minister_types_foreign.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    mForeTraits.Add(sw.ReadLine());


                }

                sw.Close();


                file = new FileStream(Application.StartupPath + "/minister_types_security.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    mSecurityTraits.Add(sw.ReadLine());


                }

                sw.Close();

                file = new FileStream(Application.StartupPath + "/minister_types_armament.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    mArmamentTraits.Add(sw.ReadLine());


                }

                sw.Close();


                file = new FileStream(Application.StartupPath + "/minister_types_intel.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    mIntelTraits.Add(sw.ReadLine());


                }

                sw.Close();

                file = new FileStream(Application.StartupPath + "/minister_types_army.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    mArmyTraits.Add(sw.ReadLine());


                }

                sw.Close();


                file = new FileStream(Application.StartupPath + "/minister_types_staff.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    mStaffTraits.Add(sw.ReadLine());


                }

                sw.Close();

                file = new FileStream(Application.StartupPath + "/minister_types_navy.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    mNavalTraits.Add(sw.ReadLine());


                }

                sw.Close();


                file = new FileStream(Application.StartupPath + "/minister_types_air.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    mAirTraits.Add(sw.ReadLine());


                }

                sw.Close();

                file = new FileStream(Application.StartupPath + "/lastNames.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {
                    lastnames.Add(sw.ReadLine());

                }

                sw.Close();


                file = new FileStream(Application.StartupPath + "/maleFirstNames.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {
                    names.Add(sw.ReadLine());

                }

                sw.Close();

                file = new FileStream(Application.StartupPath + "/MinisterPics.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    ministerPics.Add(sw.ReadLine());

                }


                sw.Close();


                file = new FileStream(Application.StartupPath + "/LeaderPics.txt", FileMode.Open, FileAccess.Read);
                sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    leadersPics.Add(sw.ReadLine());

                }


                sw.Close();
                #endregion

                preferenceCountries.Reverse();
                countries = ScrambleArrayList(countries);

                for (int contaRemove = 0; contaRemove <= preferenceCountries.Count - 1; contaRemove++)
                {
                    countries.Remove(preferenceCountries[contaRemove]);
                }

                for (int contaRemove = 0; contaRemove <= preferenceCountries.Count - 1; contaRemove++)
                {
                    countries.Add(preferenceCountries[contaRemove]);
                }

                countries.Reverse();



                for (count2 = 1; count2 <= countries.Count; count2++)
                {
                    listBox1.Items.Add(count2);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
                throw ex;
            }
        }



        #region UTILS
        // ---- ScrambleArrayList ----------------------------
        //
        // returns a randomly "sorted" ArrayList from the input ArrayList



        private ArrayList ScrambleArrayList(ArrayList AList)
        {


            ArrayList ScrambledList = new ArrayList();

            Int32 Index;

            // randomly remove items from the first list and
            // put them in the second list
            while (AList.Count > 0)
            {
                Index = ran.Next(AList.Count);
                ScrambledList.Add(AList[Index]);
                AList.RemoveAt(Index);
            }
            return ScrambledList;
        }

        //FROM RANROAD
        public String[] RandomSizeAssignProvinces()
        {
            Random rando = new Random();
            String[] assignedProvs = new String[countries.Count];

            //We add 0 to start since the first country will run from 0 to x.
            //We add provList.Count to the list to end the last country.
            //A random start index with a wrap around may be added later.
            ArrayList startProvs = new ArrayList();
            // startProvs.Add(0);
            //startProvs.Add(provinces.Length);

            //This section randomly selects indexes for the province list
            //to determine where countries will end.
            //The list can't contain duplicates.
            int count = 0;
            while (count < countries.Count - 1)
            {
                int newIndex = rando.Next(provinces.Length);
                if (!startProvs.Contains(newIndex))
                {
                    startProvs.Add(newIndex);
                    count++;
                }//end if
            }//end while

            //The list is sorted to make it easier to find the beginning and
            //end while assigning the provinces to countries.
            startProvs.Sort();

            //This section creates a string for each countries list of provinces
            //It adds the provinces from the provList file between one of the
            //random indexes and the next.
            for (int country = 0; country < countries.Count; country++)
            {
                assignedProvs[country] = "";
                int index = (int)startProvs[country];

                while (index < (int)startProvs[country + 1])
                {
                    assignedProvs[country] = assignedProvs[country] + " " + provinces[index];
                    index++;
                }//end while
            }//end for each country

            return assignedProvs;
        }
        #endregion

        #region CHECKS



        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            int num;
            try
            {
                num = Convert.ToInt32(textBox3.Text.ToString());
                if (num < 0 || num > 100)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                MessageBox.Show("Number between 0 and 100");
                textBox3.Text = "100";
            }
        }

        private void textBox2_Validating_1(object sender, CancelEventArgs e)
        {
            int num;
            try
            {
                num = Convert.ToInt32(textBox2.Text.ToString());
                if (num < 0 || num > 500)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                MessageBox.Show("Number between 0 and 500");
                textBox2.Text = "500";
            }
        }



        private void textInfra_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textInfra.Text) > Convert.ToInt32(textmaxInfra.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun Infrastucture must be equal or less than Maximun Infra");
                    textInfra.Text = textmaxInfra.Text;
                }

                if (Convert.ToInt32(textInfra.Text) < 1)
                {
                    e.Cancel = true;
                    MessageBox.Show("Infrastucture must be between 1 and max infra");
                    textInfra.Text = "1";
                }
            }
            catch (Exception ex)
            {
                textInfra.Text = textmaxInfra.Text;
            }
        }

        private void textmaxInfra_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textmaxInfra.Text) < Convert.ToInt32(textInfra.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun Infrastucture must be equal or more than Minimun Infra");
                    textmaxInfra.Text = textInfra.Text;
                }

                if (Convert.ToInt32(textmaxInfra.Text) > 10)
                {
                    e.Cancel = true;
                    MessageBox.Show("Infrastucture must be between min infra and 10");
                    textmaxInfra.Text = "10";
                }
            }
            catch (Exception ex)
            {
                textmaxInfra.Text = textInfra.Text;
            }
        }


        //private void textMSLL_Validating(object sender, CancelEventArgs e)
        //{
        //    try
        //    {

        //        if (Convert.ToInt32(textMSLL.Text) > Convert.ToInt32(textMxSLL.Text))
        //        {
        //            e.Cancel = true;
        //            MessageBox.Show("Minimun Skill must be equal or less than Maximun Skill");
        //            textMSLL.Text = textMxSLL.Text;
        //        }

        //        if (Convert.ToInt32(textMSLL.Text) < 0)
        //        {
        //            e.Cancel = true;
        //            MessageBox.Show("Skill must be between 0 and max Skill");
        //            textMSLL.Text = "1";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        textMSLL.Text = textMxSLL.Text;
        //    }
        //}

        //private void textMSAL_Validating(object sender, CancelEventArgs e)
        //{
        //    try
        //    {

        //        if (Convert.ToInt32(textMSAL.Text) > Convert.ToInt32(textMxSAL.Text))
        //        {
        //            e.Cancel = true;
        //            MessageBox.Show("Minimun Skill must be equal or less than Maximun Skill");
        //            textMSAL.Text = textMxSAL.Text;
        //        }

        //        if (Convert.ToInt32(textMSAL.Text) < 0)
        //        {
        //            e.Cancel = true;
        //            MessageBox.Show("Skill must be between 0 and max Skill");
        //            textMSAL.Text = "1";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        textMSAL.Text = textMxSAL.Text;
        //    }
        //}

        //private void textMSNL_Validating(object sender, CancelEventArgs e)
        //{
        //    try
        //    {

        //        if (Convert.ToInt32(textMSNL.Text) > Convert.ToInt32(textMxSNL.Text))
        //        {
        //            e.Cancel = true;
        //            MessageBox.Show("Minimun Skill must be equal or less than Maximun Skill");
        //            textMSNL.Text = textMxSNL.Text;
        //        }

        //        if (Convert.ToInt32(textMSNL.Text) < 0)
        //        {
        //            e.Cancel = true;
        //            MessageBox.Show("Skill must be between 0 and max Skill");
        //            textMSNL.Text = "1";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        textMSNL.Text = textMxSNL.Text;
        //    }
        //}

        private void textMxSLL_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textMxSLL.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun Skill must be equal or more than 0");
                    textMxSLL.Text = "0";
                }

                if (Convert.ToInt32(textMxSLL.Text) > 10)
                {
                    e.Cancel = true;
                    MessageBox.Show("Skill must be between 0 and 10");
                    textMxSLL.Text = "10";
                }
            }
            catch (Exception ex)
            {
                textMxSLL.Text = "6";
            }
        }

        private void textMxSAL_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textMxSAL.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun Skill must be equal or more than 0");
                    textMxSAL.Text = "0";
                }

                if (Convert.ToInt32(textMxSAL.Text) > 10)
                {
                    e.Cancel = true;
                    MessageBox.Show("Skill must be between 0 and 10");
                    textMxSAL.Text = "10";
                }
            }
            catch (Exception ex)
            {
                textMxSAL.Text = "6";
            }
        }

        private void textMxSNL_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(textMxSNL.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun Skill must be equal or more than 0");
                    textMxSAL.Text = "0";
                }

                if (Convert.ToInt32(textMxSNL.Text) > 10)
                {
                    e.Cancel = true;
                    MessageBox.Show("Skill must be between 0 and 10");
                    textMxSNL.Text = "10";
                }
            }
            catch (Exception ex)
            {
                textMxSAL.Text = "6";
            }
        }

        private void radioMinimun_CheckedChanged(object sender, EventArgs e)
        {
            if (radioMinimun.Checked == true)
            {

                checkTechFocus.Enabled = true;

            }
            else
            {
                checkTechFocus.Checked = false;
                checkTechFocus.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            int count;
            listBox1.Items.Clear();
            countries.Clear();
            if (checkBox2.Checked)
            {
                FileStream file = new FileStream(Application.StartupPath + "/countriesNewNations.txt", FileMode.Open, FileAccess.Read);
                StreamReader sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    string lineco = sw.ReadLine().Substring(0, 3);
                    if (!countries.Contains(lineco))
                    {
                        countries.Add(lineco);
                    }


                }
                // sw.Flush();
                sw.Close();


                countries = ScrambleArrayList(countries);

                for (int contaRemove = 0; contaRemove <= preferenceCountries.Count - 1; contaRemove++)
                {
                    countries.Remove(preferenceCountries[contaRemove]);
                }

                for (int contaRemove = 0; contaRemove <= preferenceCountries.Count - 1; contaRemove++)
                {
                    countries.Add(preferenceCountries[contaRemove]);
                }

                countries.Reverse();

                for (count = 1; count <= countries.Count; count++)
                {
                    listBox1.Items.Add(count);
                }


            }
            else
            {
                FileStream file = new FileStream(Application.StartupPath + "/countries.txt", FileMode.Open, FileAccess.Read);
                StreamReader sw = new StreamReader(file);

                while (!(sw.EndOfStream))
                {

                    string lineco = sw.ReadLine().Substring(0, 3);
                    if (!countries.Contains(lineco))
                    {
                        countries.Add(lineco);
                    }


                }
                // sw.Flush();
                sw.Close();

                countries = ScrambleArrayList(countries);

                for (int contaRemove = 0; contaRemove <= preferenceCountries.Count - 1; contaRemove++)
                {
                    countries.Remove(preferenceCountries[contaRemove]);
                }

                for (int contaRemove = 0; contaRemove <= preferenceCountries.Count - 1; contaRemove++)
                {
                    countries.Add(preferenceCountries[contaRemove]);
                }

                countries.Reverse();

                for (count = 1; count <= countries.Count; count++)
                {
                    listBox1.Items.Add(count);
                }




            }


            for (int contaRemove = 0; contaRemove <= preferenceCountries.Count - 1; contaRemove++)
            {
                countries.Remove(preferenceCountries[contaRemove]);
            }

            for (int contaRemove = 0; contaRemove <= preferenceCountries.Count - 1; contaRemove++)
            {
                countries.Add(preferenceCountries[contaRemove]);
            }

            countries.Reverse();




        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            int num;
            try
            {
                num = Convert.ToInt32(textBox1.Text.ToString());
                if (num < 0 || num > 1500)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                MessageBox.Show("Number between 0 and 1500");
                textBox1.Text = "350";
            }
        }

        private void textMinisters_Validating(object sender, CancelEventArgs e)
        {
            int num;
            try
            {
                num = Convert.ToInt32(textMinisters.Text.ToString());
                if (num < 0 || num > 200)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                MessageBox.Show("Number between 0 and 200");
                textMinisters.Text = "70";
            }

        }

        private void textMinisters_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioSUCL_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSUCL.Checked)
            {
                textSUML.Enabled = true;
                textSUMxL.Enabled = true;
                textSUML.Text = "15";
                textSUMxL.Text = "25";
            }
            else
            {
                textSUML.Enabled = false;
                textSUMxL.Enabled = false;
                textSUML.Text = "";
                textSUMxL.Text = "";
            }
        }

        private void radioSUCA_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSUCA.Checked)
            {
                textSUMA.Enabled = true;
                textSUMxA.Enabled = true;
                textSUMA.Text = "4";
                textSUMxA.Text = "8";
            }
            else
            {
                textSUMA.Enabled = false;
                textSUMxA.Enabled = false;
                textSUMA.Text = "";
                textSUMxA.Text = "";
            }
        }

        private void radioSUCN_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSUCN.Checked)
            {
                textSUMN.Enabled = true;
                textSUMxN.Enabled = true;
                textSUMN.Text = "4";
                textSUMxN.Text = "8";

            }
            else
            {
                textSUMN.Enabled = false;
                textSUMxN.Enabled = false;
                textSUMN.Text = "4";
                textSUMxN.Text = "8";
            }
        }

        private void textSUML_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textSUML.Text) > Convert.ToInt32(textSUMxL.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun unit must be equal or less than Maximun unit");
                    textSUML.Text = textSUMxL.Text;
                }

                if (Convert.ToInt32(textSUML.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimal unit must be between 0 and max Unit");
                    textSUML.Text = "0";
                }
            }
            catch (Exception ex)
            {
                textSUML.Text = textSUMxL.Text;
            }
        }

        private void textSUMxL_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textSUMxL.Text) < Convert.ToInt32(textSUML.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun unit must be equal or less than Minimun unit");
                    textSUMxL.Text = textSUML.Text;
                }

                if (Convert.ToInt32(textSUMxL.Text) > 200)
                {
                    e.Cancel = true;
                    MessageBox.Show("Max Unit must be between min Unit and 200");
                    textSUMxL.Text = "200";
                }
            }
            catch (Exception ex)
            {
                textSUMxL.Text = textSUML.Text;
            }
        }

        private void textSUMA_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(textSUMA.Text) > Convert.ToInt32(textSUMxA.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun unit must be equal or less than Maximun unit");
                    textSUMA.Text = textSUMxA.Text;
                }

                if (Convert.ToInt32(textSUMA.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimal unit must be between 0 and max Unit");
                    textSUMA.Text = "0";
                }

            }
            catch (Exception ex)
            {
                textSUMA.Text = textSUMxA.Text;
            }
        }

        private void textSUMxA_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(textSUMxA.Text) < Convert.ToInt32(textSUMA.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun unit must be equal or less than Minimun unit");
                    textSUMxA.Text = textSUMA.Text;
                }

                if (Convert.ToInt32(textSUMxA.Text) > 200)
                {
                    e.Cancel = true;
                    MessageBox.Show("Max Unit must be between min Unit and 200");
                    textSUMxA.Text = "200";
                }
            }
            catch (Exception ex)
            {
                textSUMxA.Text = textSUMA.Text;
            }
        }

        private void textSUMN_Validating(object sender, CancelEventArgs e)
        {
            if (Convert.ToInt32(textSUMN.Text) > Convert.ToInt32(textSUMxN.Text))
            {
                e.Cancel = true;
                MessageBox.Show("Minimun unit must be equal or less than Maximun unit");
                textSUMN.Text = textSUMxN.Text;
            }

            if (Convert.ToInt32(textSUMN.Text) < 0)
            {
                e.Cancel = true;
                MessageBox.Show("Minimal unit must be between 0 and max Unit");
                textSUMN.Text = "0";
            }
        }

        private void textSUMxN_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(textSUMxN.Text) < Convert.ToInt32(textSUMN.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun unit must be equal or less than Minimun unit");
                    textSUMxN.Text = textSUMN.Text;
                }

                if (Convert.ToInt32(textSUMxN.Text) > 200)
                {
                    e.Cancel = true;
                    MessageBox.Show("Max Unit must be between min Unit and 200");
                    textSUMxN.Text = "200";
                }
            }
            catch (Exception ex)
            {
                textSUMxN.Text = textSUMN.Text;
            }
        }

        private void radioSUNN_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSUNN.Checked == true)
            {
                comboBox1.Enabled = false;
            }
            else
            {
                comboBox1.Enabled = true;
            }
        }



        private void MinNU_Validating_1(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(MinNU.Text) > Convert.ToInt32(MaxNU.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun national unity must be equal or less than Maximun national unity");
                    MinNU.Text = MaxNU.Text;
                }

                if (Convert.ToInt32(MinNU.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimal national unity must be between 0 and max national unity");
                    MinNU.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MinNU.Text = MaxNU.Text;
            }
        }

        private void MaxNU_Validating_1(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(MaxNU.Text) < Convert.ToInt32(MinNU.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun national unity must be equal or higher than Minimun national unity");
                    MaxNU.Text = MinNU.Text;
                }

                if (Convert.ToInt32(MaxNU.Text) > 100)
                {
                    e.Cancel = true;
                    MessageBox.Show("Max national unity must be between min national unity and 100");
                    MaxNU.Text = "100";
                }
            }
            catch (Exception ex)
            {
                MaxNU.Text = MinNU.Text;
            }
        }

        private void MinNe_Validating_1(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(MinNe.Text) > Convert.ToInt32(MaxNe.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun neutrality must be equal or less than Maximun neutrality");
                    MinNe.Text = MaxNe.Text;
                }

                if (Convert.ToInt32(MinNe.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimal neutrality must be between 0 and max neutrality");
                    MinNe.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MinNe.Text = MaxNe.Text;
            }
        }

        private void MaxNe_Validating_1(object sender, CancelEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(MaxNe.Text) < Convert.ToInt32(MinNe.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun neutrality must be equal or less than Minimun neutrality");
                    MaxNe.Text = MinNe.Text;
                }

                if (Convert.ToInt32(MaxNe.Text) > 100)
                {
                    e.Cancel = true;
                    MessageBox.Show("Max neutrality must be between min neutrality and 100");
                    MaxNe.Text = "100";
                }
            }
            catch (Exception ex)
            {
                MaxNe.Text = MinNe.Text;
            }
        }

        private void textOffRa_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textOffRa.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Officers ratio must be between 0 and 200");
                    textOffRa.Text = "0";
                }

                if (Convert.ToInt32(textOffRa.Text) > 200)
                {
                    e.Cancel = true;
                    MessageBox.Show("Officers ratio must be between 0 and 200");
                    textOffRa.Text = "200";
                }
            }
            catch (Exception ex)
            {
                textOffRa.Text = "125";
            }
        }

        private void radioFactNo_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxFL.Enabled = !radioFactNo.Checked;
            groupBoxLFBonus.Enabled = !radioFactNo.Checked;
        }


        private void textICLand_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(textICLand.Text) > 10)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun IC for land fort beetwen 1 and 10");
                    textICLand.Text = "10";
                }

                if (Convert.ToInt32(textICLand.Text) < 1)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun IC for land fort beetwen 1 and 10");
                    textICLand.Text = "1";
                }
            }
            catch (Exception ex)
            {
                textICLand.Text = "7";
            }
        }

        private void textchanceICLand_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(textchanceICLand.Text) > 100)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun chance for land fort beetwen 10 and 100");
                    textchanceICLand.Text = "100";
                }

                if (Convert.ToInt32(textchanceICLand.Text) < 10)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun chance for land fort beetwen 10 and 100");
                    textchanceICLand.Text = "10";
                }
            }
            catch (Exception ex)
            {
                textchanceICLand.Text = "60";
            }
        }

        private void textMinICLand_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textMinICLand.Text) > Convert.ToInt32(textMaxICLand.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun IC land fort must be equal or less than maximun IC land fort");
                    textMinICLand.Text = textMaxICLand.Text;
                }

                if (Convert.ToInt32(textMinICLand.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun IC land  must be between 0 and max IC land fort");
                    textMinICLand.Text = "0";
                }
            }
            catch (Exception ex)
            {
                textMinICLand.Text = textMaxICLand.Text;
            }
        }

        private void textMaxICLand_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textMaxICLand.Text) < Convert.ToInt32(textMinICLand.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun IC land fort must be equal or more than minimun IC land fort");
                    textMaxICLand.Text = textMinICLand.Text;
                }

                if (Convert.ToInt32(textMaxICLand.Text) > 10)
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun IC land  must be between min IC land fort and 10");
                    textMaxICLand.Text = "10";
                }
            }
            catch (Exception ex)
            {
                textMaxICLand.Text = textMinICLand.Text;
            }
        }


        private void textMaxNVNaval_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textMaxNVNaval.Text) < Convert.ToInt32(textMinNVNaval.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun NV naval fort must be equal or more than minimun NV naval fort");
                    textMaxNVNaval.Text = textMinNVNaval.Text;
                }

                if (Convert.ToInt32(textMaxNVNaval.Text) > 10)
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun NV naval must be between min NV naval fort and 10");
                    textMaxNVNaval.Text = "10";
                }
            }
            catch (Exception ex)
            {
                textMaxNVNaval.Text = textMinNVNaval.Text;
            }
        }

        private void textMinNVNaval_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textMinNVNaval.Text) > Convert.ToInt32(textMaxNVNaval.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun NV naval fort must be equal or less than maximun NV navalfort");
                    textMinNVNaval.Text = textMaxNVNaval.Text;
                }

                if (Convert.ToInt32(textMinNVNaval.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun NV naval  must be between 0 and max NV naval fort");
                    textMinNVNaval.Text = "0";
                }
            }
            catch (Exception ex)
            {
                textMinNVNaval.Text = textMaxNVNaval.Text;
            }
        }

        private void textchanceNVNaval_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(textchanceNVNaval.Text) > 100)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun chance for naval fort beetwen 10 and 100");
                    textchanceNVNaval.Text = "100";
                }

                if (Convert.ToInt32(textchanceNVNaval.Text) < 10)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun chance for naval fort beetwen 10 and 100");
                    textchanceNVNaval.Text = "10";
                }
            }
            catch (Exception ex)
            {
                textchanceNVNaval.Text = "60";
            }
        }

        private void textNVNaval_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(textNVNaval.Text) > 10)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun naval for naval fort beetwen 1 and 10");
                    textNVNaval.Text = "10";
                }

                if (Convert.ToInt32(textNVNaval.Text) < 1)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun naval for naval fort beetwen 1 and 10");
                    textNVNaval.Text = "1";
                }
            }
            catch (Exception ex)
            {
                textNVNaval.Text = "7";
            }
        }

        private void textMaxClaims_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textMaxClaims.Text) < Convert.ToInt32(textMinClaims.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun claims must be equal or more than minimun claims");
                    textMaxClaims.Text = textMinClaims.Text;
                }

                if (Convert.ToInt32(textMaxClaims.Text) > 950)
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun claims must be between min claims and 950");
                    textMaxClaims.Text = "950";
                }
            }
            catch (Exception ex)
            {
                textMaxClaims.Text = textMinClaims.Text;
            }
        }

        private void textMinClaims_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textMinClaims.Text) > Convert.ToInt32(textMaxClaims.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun claims must be equal or less than maximun claims");
                    textMinClaims.Text = textMaxClaims.Text;
                }

                if (Convert.ToInt32(textMinClaims.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun claims must be between 0 and max claims");
                    textMinClaims.Text = "0";
                }
            }
            catch (Exception ex)
            {
                textMinClaims.Text = textMaxClaims.Text;
            }
        }



        #endregion


        private void refreshList()
        {



            try
            {


                progressBar1.Visible = true;
                //men1.Show();

                ////  checkRef d = new checkRef(rRef);


                //// this.Invoke(d, null);


                ////300000
                //_timerMensajes = new System.Timers.Timer(300);

                //_timerMensajes.AutoReset = true;
                //_timerMensajes.Enabled = true;

                //_timerMensajes.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                //_timerMensajes.Start();



            }
            catch (Exception ex)
            {

                //   throw new Exception(ex.Message);
            }


            //

        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                /*  checkRef d = new checkRef(rRef);
                  this.Refresh();
                  Application.DoEvents();
                  this.Invoke(d, null);*/

            }
            catch (Exception ex)
            {

            }

        }

        private void rRef()
        {



            // Thread.Sleep(300);

            // men1.changeLabel(labelStat.Text, totalP);
            /* this.Refresh();
             listView1.Refresh();
             labelStat.Refresh();*/



            /*
                        if (!validRefreshThr)
                        {

                           // break;

                            _timerMensajes.Stop();
                            men1.Close();
                        }*/
            //}


        }

        private void butGenerate_Click(object sender, EventArgs e)
        {
            try
            {

                ArrayList startProvs = new ArrayList(); //starting provinces. This will say what size has every country!

                province provinceGen;
                country countryGen;
                writing write;

                int count;
                int countUsedProv;
                int ult; //last province of a country    
                int pri; //first province of a country


                //inicialize variables every time you generate

                //better we force to restart..



                ministersID = 1;
                unitID = 1;
                leadersID = 1;

                countUsedProv = 0;
                pri = 0;



                if (first)
                {
                    MessageBox.Show("Wait for the message of finish please (Click ok to start)");
                }
                else
                {
                    MessageBox.Show("Delete the history folder and restart the generator. Thx");
                    return;
                }
                try
                {

                    //create directories
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "/common/countries");
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "/history");
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "/history/countries");
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "/history/units");
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "/history/provinces");
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "/history/provinces/z");
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "/history/leaders");
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "/history/wars");
                    System.IO.Directory.CreateDirectory(Application.StartupPath + "/history/diplomacy");
                }

                catch (Exception ex)
                {
                    throw new Exception("Error creating folders and subfolders: " + ex.Message);

                }



                try
                {
                    dummyMinsiterFiles();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creating minsister files: " + ex.Message);
                }






                if (listBox1.SelectedIndex == -1)
                {
                    throw new Exception("Select number of countries");
                }


                tabControl1.SelectTab("TabStats");
                labelStat.Text = "Making countries";
                tabControl1.Enabled = false;

                labelStat.Refresh();
                this.Refresh();
                Application.DoEvents();

                if (!radioButtonMapOri.Checked)
                {
                    refreshList();
                }
                totalP = 10;
                progressBar1.PerformStep();
                Application.DoEvents();

                listView1.Visible = true;
                butGenerate.Enabled = false;

                first = false;
                //we read the user selected options for resources and change the probability
                readResourcesValues();


                //FROM RANROAD (more o less)
                //This section randomly selects indexes for the province list
                //to determine where countries will end.
                //The list can't contain duplicates.
                count = 0;
                int add = 0;
                //if (this.radioButtonMapNew.Checked)
                //{
                //    add = 2;
                //}
                //if (checkBox1.Checked)
                //{

                  //  int prov;
                  //  int numProvs = provinces.Length / (listBox1.SelectedIndex + 1 + add);  //provinces per country
                   // prov = numProvs;
                    //for (int i = 0; i < listBox1.SelectedIndex; i++)
                   // {
                    //    startProvs.Add(prov);
                    //    prov = prov + numProvs;
                   // }

               // }
               // else
               // {

                    while (count <= (listBox1.SelectedIndex + add))
                    {
                        int newIndex = ran.Next(provinces.Length);
                        if (!startProvs.Contains(provinces[newIndex]))
                        {
                            //startProvs.Add(newIndex);
                            startProvs.Add(provinces[newIndex]);
                            count++;
                        }
                    }

                //}
                // startProvs.Add(Convert.ToInt32(provinces[provinces.Length - 1]));


                // startProvs.Add(provinces[0]);



                if (radioButtonMapOri.Checked)
                {

                    startProvs.Sort(); //now we have the starts of every country AND in order
                    //GENERATING PROVINCES
                    for (countUsedProv = 0; countUsedProv <= provinces.Length - 1; countUsedProv++)
                    {
                        provinceGen = new province(provinces[countUsedProv]);

                        if (checkArtic(provinces[countUsedProv]))
                        {
                            provinceGen.articSG = true;
                        }

                        if (checkCoastal(provinces[countUsedProv]))
                        {
                            provinceGen.coastalSG = true;
                        }


                        //if a country hasnt got any important provinces later this may change and the resources will be recalculated
                        if (ran.NextDouble() >= 0.93)
                        { //8% of probability

                            provinceGen.importantSG = true;
                        }

                        //creating the resources
                        provinceGen.createResources(this, false);

                        provincesGenerated.Add(provinceGen);

                    }

                    countUsedProv = 0;

                    bool firstCountry = true; ;
                    //CREATE Country and assing provinces to them
                    for (count = 0; count <= listBox1.SelectedIndex; count++)
                    {
                        countryGen = new country(count, countries[count].ToString());

                        //Provinces

                        pri = Convert.ToInt32(startProvs[count].ToString()); //first province of this country (isnt the id of the province, only his position in the province array!!)


                        ult = Convert.ToInt32(startProvs[count + 1].ToString()); //last province of this country (isnt the id of the province, only his position in the province array!!)

                        countryGen.assingProvinces(pri, ult, this, firstCountry);

                        if (checkExtraFort.Checked)
                        {
                            province prov;
                            prov = (province)provincesGenerated[pri];
                            prov.landFort = prov.landFort + 2;
                            prov = (province)provincesGenerated[pri + 1];
                            prov.landFort = prov.landFort + 2;
                            prov = (province)provincesGenerated[pri + 2];
                            prov.landFort = prov.landFort + 2;
                            prov = (province)provincesGenerated[pri + 3];
                            prov.landFort = prov.landFort + 2;

                            prov = (province)provincesGenerated[ult];
                            prov.landFort = prov.landFort + 2;
                            prov = (province)provincesGenerated[ult - 1];
                            prov.landFort = prov.landFort + 2;
                            prov = (province)provincesGenerated[ult - 2];
                            prov.landFort = prov.landFort + 2;
                            prov = (province)provincesGenerated[ult - 3];
                            prov.landFort = prov.landFort + 2;
                        }



                        countryGen.generatePolitics(this);
                        countryGen.generateTechs(this);
                        countryGen.createLeaders(this);
                        countryGen.createMinisters(this);
                        countryGen.createStartingArmy(this);

                        countriesGenerated.Add(countryGen);

                        firstCountry = false;

                        /*update stats*/
                        listView1.Items.Add(countryGen.tag);

                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(countryGen.totalIC.ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(countryGen.totalLeadership.ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(countryGen.totalManpower.ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(countryGen.numberProvinces.ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(countryGen.countryArmy.Count.ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(countryGen.countryNaval.Count.ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(countryGen.countryAir.Count.ToString());


                        Color color = Color.White;

                        switch (countryGen.intGovernment)
                        {
                            case 1:
                                color = Color.FromArgb(0, 0, 0);
                                break;
                            case 2:
                                color = Color.FromArgb(50, 50, 50);
                                break;
                            case 3:
                                color = Color.FromArgb(100, 100, 100);
                                break;
                            case 4:
                                color = Color.FromArgb(125, 125, 125);
                                break;
                            case 5:
                                color = Color.FromArgb(150, 150, 150);
                                break;
                            case 6:
                                color = Color.FromArgb(107, 114, 255);
                                break;
                            case 7:
                                color = Color.FromArgb(86, 173, 255);
                                break;
                            case 8:
                                color = Color.FromArgb(117, 201, 201);
                                break;
                            case 9:
                                color = Color.FromArgb(197, 140, 255);
                                break;
                            case 10:
                                color = Color.FromArgb(255, 163, 190);
                                break;
                            case 11:
                                color = Color.FromArgb(255, 109, 119);
                                break;
                            case 12:
                                color = Color.FromArgb(255, 87, 68);
                                break;


                        }

                        listView1.Items[listView1.Items.Count - 1].ForeColor = color;
                        listView1.Items[listView1.Items.Count - 1].ToolTipText = countryGen.government + " - " + countryGen.ideology;


                        string tf = "N/A";
                        switch (countryGen.techFocus)
                        {
                            case 0:
                                tf = "Ta";
                                break;

                            case 1:
                                tf = "In";
                                break;

                            case 2:
                                tf = "Ai";
                                break;

                            case 3:
                                tf = "Nv";
                                break;


                        }

                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(tf);

                        listView1.Refresh();


                        totalIC = totalIC + countryGen.totalIC;
                        totalLeadership = totalLeadership + countryGen.totalLeadership;
                        totalMP = totalMP + countryGen.totalManpower;
                        totalProvinces = totalProvinces + countryGen.numberProvinces;

                        /*****/


                        int count2;
                        int firstP;
                        int lastP;

                        firstP = ran.Next(0, provincesGenerated.Count - 1);
                        lastP = ran.Next(firstP, firstP + 50);

                        if (lastP > provincesGenerated.Count - 1)
                        {

                            lastP = provincesGenerated.Count;
                        }

                        for (count2 = count; count2 <= listBox1.Items.Count - 1; count2++)
                        {

                            firstP = ran.Next(0, provincesGenerated.Count - 1);
                            bool littleCountry = false;

                            //first make claims!!!

                            //claims for provinces before the country
                            littleCountry = Convert.ToBoolean(ran.Next(0, 2)); //little claims,not littlecountry
                            if (littleCountry)
                            {
                                lastP = ran.Next(firstP, firstP + 1); //max size 1 provinces + "claims".
                            }
                            else
                            {
                                lastP = ran.Next(firstP, firstP + 10); //max size 10 provinces + "claims".
                            }


                            if (lastP > provincesGenerated.Count - 1)
                            {

                                lastP = provincesGenerated.Count;
                            }

                            countryGen = new country(count2, countries[count2].ToString());
                            countryGen.createLeaders(this);
                            countryGen.createMinisters(this);
                            countryGen.assingProvincesPuppetable(firstP, lastP, this, littleCountry);

                            countriesPuppetableGenerated.Add(countryGen);
                        }
                    }

                }
                else
                {
                    //new generator

                    // first, we create the basic of every country
                    for (count = 0; count <= listBox1.SelectedIndex; count++)
                    {
                        int rate = 1;
                        if (!checkBox1.Checked)
                        {
                            switch (ran.Next(0, 11))
                            {

                                case 5:
                                    rate = 2;
                                    break;
                                case 6:
                                    rate = 2;
                                    break;
                                case 7:
                                    rate = 3;
                                    break;
                                case 8:
                                    rate = 3;
                                    break;
                                case 9:
                                    rate = 4;
                                    break;
                                case 10:
                                    rate = 5;
                                    break;

                                default:
                                    rate = 1;
                                    break;

                            }
                        }
                        countryGen = new country(count, countries[count].ToString(), rate);
                        countriesGenerated.Add(countryGen);
                    }

                    //assing one starting province for every country
                    country Country;
                    int numStop = 12000;
                    labelStat.Text = "Assigning starting provinces..";
                    labelStat.Refresh();
                    Application.DoEvents();

                    this.Refresh();
                    for (count = 0; count <= countriesGenerated.Count - 1; count++)
                    {



                        numStop = numStop - 100;

                        Country = (country)countriesGenerated[count];

                        provinceWithOwner.Add(startProvs[count]);

                        provinceGen = new province(startProvs[count].ToString(), true, this, false);

                        if (checkArtic(startProvs[count].ToString()))
                        {
                            provinceGen.articSG = true;
                        }

                        if (checkCoastal(startProvs[count].ToString()))
                        {
                            provinceGen.coastalSG = true;
                        }


                        //if a country hasnt got any important provinces later this may change and the resources will be recalculated
                        if (ran.NextDouble() >= 0.93)
                        { //8% of probability

                            provinceGen.importantSG = true;
                        }

                        //creating the resources
                        provinceGen.createResources(this, false);

                        provinceGen.controllerTag = Country.tag;
                        provinceGen.ownerTag = Country.tag;
                        provinceGen.countriesCore.Add(Country.tag);

                        provincesGenerated.Add(provinceGen);
                        provincesGeneratedDict.Add(Convert.ToInt32(provinceGen.id), provinceGen);
                        Country.provincesALL.Add(provinceGen);
                        Country.provinceswithadj.Add(provinceGen);

                        provincesAuxArrayList.Remove(startProvs[count].ToString());


                    }

                    //start to grow
                    if (numStop < 5000)
                    {
                        numStop = 5000;
                    }
                    bool stop = false;
                    int countStop = 0;
                    labelStat.Text = "Assigning provinces.. (slow..) (from 30s to 5 minutes)";

                    labelStat.Refresh();
                    this.Refresh();
                    int contavueltas = -1;
                    do
                    {
                        contavueltas++;

                        if (contavueltas == 1000)
                        {
                            totalP = 20;
                            progressBar1.PerformStep();
                        }

                        if (contavueltas == 5000)
                        {
                            totalP = 30;
                            progressBar1.PerformStep();
                        }

                        if (contavueltas == 10000)
                        {
                            totalP = 40;
                            progressBar1.PerformStep();
                        }

                        if (contavueltas == 15000)
                        {
                            totalP = 40;
                            progressBar1.PerformStep();
                        }

                        if (contavueltas == 20000)
                        {
                            totalP = 60;
                            progressBar1.PerformStep();
                        }

                        Application.DoEvents();
                        //add one (or rate) province(s)!
                        for (count = 0; count <= countriesGenerated.Count - 1; count++)
                        {




                            // labelStat.Text = "Assigning provinces.. (slow..) (from 30s to 5 minutes) Adding to country " + (count + 1).ToString() + " of " + countriesGenerated.ToString();
                            // Application.DoEvents();
                            Country = (country)countriesGenerated[count];

                            for (int contaRate = 1; contaRate <= Country.growRate; contaRate++)
                            {
                                province oriProv;
                                int possibleProv = 0;


                                if (Country.provinceswithadj.Count > 0)
                                {
                                    oriProv = (province)Country.provinceswithadj[(ran.Next(Country.provinceswithadj.Count))];

                                    int ranInt = 0;
                                    if (oriProv.adje.Count > 0)
                                    {
                                        ranInt = ran.Next(oriProv.adje.Count);
                                        if (oriProv.adje[ranInt].ToString() == "")
                                        {
                                            oriProv.adje.Remove(possibleProv);
                                        }
                                        else
                                        {
                                            possibleProv = Convert.ToInt32(oriProv.adje[ranInt]);

                                            if (!provinceWithOwner.Contains(possibleProv))
                                            {
                                                oriProv.adje.Remove(possibleProv);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Country.provinceswithadj.Remove(oriProv);
                                    }




                                    //check if used

                                    if (!provinceWithOwner.Contains(possibleProv) && provincesAuxArrayList.Contains(possibleProv.ToString()))
                                    {
                                        provinceGen = new province(possibleProv.ToString(), true, this, false);

                                        provinceWithOwner.Add(possibleProv);


                                        if (checkArtic(possibleProv.ToString()))
                                        {
                                            provinceGen.articSG = true;
                                        }

                                        if (checkCoastal(possibleProv.ToString()))
                                        {
                                            provinceGen.coastalSG = true;
                                        }


                                        //if a country hasnt got any important provinces later this may change and the resources will be recalculated
                                        if (ran.NextDouble() >= 0.93)
                                        { //8% of probability

                                            provinceGen.importantSG = true;
                                        }

                                        //creating the resources

                                        provinceGen.controllerTag = Country.tag;
                                        provinceGen.ownerTag = Country.tag;
                                        provinceGen.countriesCore.Add(Country.tag);

                                        provinceGen.createResources(this, false);

                                        provincesGenerated.Add(provinceGen);
                                        provincesGeneratedDict.Add(Convert.ToInt32(provinceGen.id), provinceGen);
                                        Country.provincesALL.Add(provinceGen);
                                        Country.provinceswithadj.Add(provinceGen);

                                        provincesAuxArrayList.Remove(possibleProv.ToString());
                                        countStop = 0;
                                    }
                                    else
                                    {


                                        oriProv.adje.Remove(possibleProv); //in case is sea adj
                                        countStop++;

                                        if (countStop == numStop)
                                        {
                                            stop = true;
                                        }
                                    }
                                }
                                else
                                {
                                    countStop++;

                                    if (countStop == numStop)
                                    {
                                        stop = true;
                                    }
                                }

                            }


                        }

                    } while (stop == false);
                    totalP = 65;
                    labelStat.Text = "Calculating stats of continental area";
                    labelStat.Refresh();
                    progressBar1.Step = 5;
                    progressBar1.PerformStep();
                    Application.DoEvents();
                    this.Refresh();
                    for (int count1 = 0; count1 <= countriesGenerated.Count - 1; count1++)
                    {
                        Country = (country)countriesGenerated[count1];
                        Country.assingProvincesNewGenerator(this, true);
                    }







                    //islands

                    labelStat.Text = "Assigning Islands..";
                    totalP = 70;
                    labelStat.Refresh();
                    progressBar1.PerformStep();
                    Application.DoEvents();
                    this.Refresh();
                    if (navalProvinces.Count > 0)
                    {
                        while (provincesAuxArrayList.Count > 0)
                        {
                            for (int contaprov1 = 0; contaprov1 <= provincesAuxArrayList.Count - 1; contaprov1++)
                            {
                                if (!provinceWithOwner.Contains(provincesAuxArrayList[contaprov1]))
                                {

                                    int possibleProv = Convert.ToInt32(provincesAuxArrayList[contaprov1]);


                                    provinceWithOwner.Add(possibleProv.ToString());

                                    provinceGen = new province(possibleProv.ToString(), true, this, true);

                                    if (checkArtic(possibleProv.ToString()))
                                    {
                                        provinceGen.articSG = true;
                                    }

                                    if (checkCoastal(possibleProv.ToString()))
                                    {
                                        provinceGen.coastalSG = true;
                                    }


                                    //if a country hasnt got any important provinces later this may change and the resources will be recalculated
                                    if (ran.NextDouble() >= 0.93)
                                    { //8% of probability

                                        provinceGen.importantSG = true;
                                    }

                                    //creating the resources
                                    provinceGen.createResources(this, false);

                                    bool genNaval = false;
                                    if (provinceGen.navalBase < 6 && provinceGen.coastal)
                                    {
                                        provinceGen.navalBase = provinceGen.navalBase + 1;
                                        genNaval = true;
                                    }


                                    bool valid = false;
                                    int contabreak = 0;
                                    while (!valid)
                                    {
                                        Country = (country)countriesGenerated[ran.Next(countriesGenerated.Count)];

                                        if (Country.hasNavalBases)
                                        {

                                            provinceGen.controllerTag = Country.tag;
                                            provinceGen.ownerTag = Country.tag;
                                            provinceGen.countriesCore.Add(Country.tag);

                                            Country.provincesALL.Add(provinceGen);
                                            provincesGenerated.Add(provinceGen);
                                            provincesGeneratedDict.Add(Convert.ToInt32(provinceGen.id), provinceGen);
                                            provincesAuxArrayList.Remove(possibleProv.ToString());
                                            provinceGen.adjeProvinceGenerated.Add(provinceGen);
                                            for (int contaTest = 0; contaTest <= 30; contaTest++)
                                            {

                                                if (provinceGen.adje.Count > 0)
                                                {
                                                    int ranInt = ran.Next(provinceGen.adje.Count);
                                                    if (provinceGen.adje[ranInt].ToString() == "")
                                                    {
                                                        provinceGen.adje.Remove(possibleProv);
                                                    }
                                                    else
                                                    {
                                                        possibleProv = Convert.ToInt32(provinceGen.adje[ranInt]);

                                                        if (!provinceWithOwner.Contains(possibleProv))
                                                        {
                                                            provinceGen.adje.Remove(possibleProv);
                                                        }
                                                    }
                                                }


                                                //check if used

                                                if (!provinceWithOwner.Contains(possibleProv) && provincesAuxArrayList.Contains(possibleProv.ToString()))
                                                {

                                                    provinceGen = new province(possibleProv.ToString(), true, this, true);

                                                    provinceWithOwner.Add(possibleProv.ToString());

                                                    if (checkArtic(possibleProv.ToString()))
                                                    {
                                                        provinceGen.articSG = true;
                                                    }

                                                    if (checkCoastal(possibleProv.ToString()))
                                                    {
                                                        provinceGen.coastalSG = true;
                                                    }


                                                    //if a country hasnt got any important provinces later this may change and the resources will be recalculated
                                                    if (ran.NextDouble() >= 0.93)
                                                    { //8% of probability

                                                        provinceGen.importantSG = true;
                                                    }

                                                    //creating the resources
                                                    provinceGen.createResources(this, false);

                                                    if (provinceGen.navalBase < 6 && provinceGen.coastal && !genNaval)
                                                    {
                                                        provinceGen.navalBase = provinceGen.navalBase + 1;
                                                        genNaval = true;
                                                    }

                                                    provinceGen.controllerTag = Country.tag;
                                                    provinceGen.ownerTag = Country.tag;
                                                    provinceGen.countriesCore.Add(Country.tag);
                                                    provinceGen.adjeProvinceGenerated.Add(provinceGen);
                                                    provincesGenerated.Add(provinceGen);
                                                    provincesGeneratedDict.Add(Convert.ToInt32(provinceGen.id), provinceGen);
                                                    Country.provincesALL.Add(provinceGen);
                                                    provincesAuxArrayList.Remove(possibleProv.ToString());
                                                    contabreak = 0;
                                                }
                                                else
                                                {
                                                    provinceGen.adje.Remove(possibleProv);
                                                    contabreak++;

                                                    if (contabreak > 40)
                                                    {
                                                        break;
                                                    }
                                                }
                                            }


                                            valid = true;
                                        }

                                    }



                                }
                            }




                        }//end islands
                    }
                    labelStat.Text = "Claims and fort bordes.. (VERY slow if base chance is high and claims level > 5) ";
                    labelStat.Refresh();
                    this.Refresh();
                    totalP = 80;
                    Application.DoEvents();
                    progressBar1.Step = 10;
                    progressBar1.PerformStep();
                    foreach (province auxprovi in provincesGenerated)
                    {
                        for (int contaT = 0; contaT <= auxprovi.adjeNoDel.Count - 1; contaT++)
                        {
                            if (provincesAuxArrayListNoDelete.Contains(auxprovi.adjeNoDel[contaT]) && !provincesAuxArrayList.Contains(auxprovi.adjeNoDel[contaT]))
                            {
                                try
                                {
                                    auxprovi.adjeProvinceGenerated.Add((province)provincesGeneratedDict[Convert.ToInt32(auxprovi.adjeNoDel[contaT])]);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }

                    }
                    totalP = 90;

                    progressBar1.PerformStep();
                    Application.DoEvents();
                    int levelClaims = Convert.ToInt32(textLevelClaims.Text);
                    int CurrentlevelClaims = 1;
                    int cooooo = 0;
                    int test2 = 0;
                    foreach (province auxprovi in provincesGenerated)
                    {
                        if (test2 == 1000)
                        {
                            if (progressBar1.Value < 80)
                            {
                                progressBar1.Value = progressBar1.Value + 1;
                            }
                            test2 = 0;
                        }
                        test2++;
                        if (cooooo > 10000 || cooooo % 100 == 0 || cooooo == 1)
                        {
                            labelStat.Text = "Claims and fort bordes.. (VERY slow if base chance is high and claims level > 5) " + cooooo + "/" + numberProvinces;
                            labelStat.Refresh();
                            this.Refresh();
                            Application.DoEvents();
                        }
                        cooooo++;
                        bool noaddedfort = true;
                        string tag = auxprovi.ownerTag;
                        bool changeOwner = true;
                        string changeTag = "";
                        foreach (province adjauxprovi in auxprovi.adjeProvinceGenerated)
                        {
                            if (tag != adjauxprovi.ownerTag && provincesAuxArrayListNoDelete.Contains(adjauxprovi.id.ToString()))
                            {

                                if (checkExtraFort.Checked && noaddedfort && !auxprovi.isColony) //only continental!!
                                {
                                    if (ran.Next(1, 101) <= Convert.ToInt32(textChanceFortsBorders.Text)) //chance
                                    {
                                        int addFort = 0;

                                        addFort = ran.Next(Convert.ToInt32(textMinFortBorder.Text), Convert.ToInt32(textMaxFortBorder.Text) + 1); //min and max fort (ADD to current level)

                                        auxprovi.landFort = auxprovi.landFort + addFort;

                                        if (auxprovi.landFort > 10)
                                        {
                                            auxprovi.landFort = 10;
                                        }

                                    }
                                    noaddedfort = false;
                                }
                                if (levelClaims > 0)
                                {
                                    if (!adjauxprovi.countriesCore.Contains(tag))
                                    {
                                        adjauxprovi.addCountryCore(tag);
                                    }



                                    addClaimLevel(auxprovi, levelClaims, CurrentlevelClaims, tag);

                                }

                            }

                            if (auxprovi.isColony && !auxprovi.coastal && changeOwner)
                            {
                                if (tag == adjauxprovi.ownerTag && tag != "")
                                {
                                    changeOwner = false;
                                }
                                else
                                {
                                    if (adjauxprovi.ownerTag != "")
                                    {
                                        changeTag = adjauxprovi.ownerTag;
                                    }
                                }
                            }


                        }

                        if (changeOwner && auxprovi.isColony && !auxprovi.coastal)
                        {
                            if (changeTag != "")
                            {
                                auxprovi.ownerTag = changeTag;
                                auxprovi.controllerTag = changeTag;
                                auxprovi.countriesCore.Add(changeTag);
                                auxprovi.countriesCore.Remove(tag);
                            }
                        }

                    }



                    for (int count1 = 0; count1 <= countriesGenerated.Count - 1; count1++)
                    {
                        Country = (country)countriesGenerated[count1];
                        Country.assingProvincesNewGenerator(this, false);
                    }




                    totalP = 95;
                    labelStat.Text = "Generating politics,techs,leaders,ministers and army..";
                    labelStat.Refresh();
                    progressBar1.Step = 5;
                    progressBar1.PerformStep();
                    Application.DoEvents();
                    this.Refresh();
                    for (int count1 = 0; count1 <= countriesGenerated.Count - 1; count1++)
                    {
                        Country = (country)countriesGenerated[count1];

                        Country.generatePolitics(this);
                        Country.generateTechs(this);
                        Country.createLeaders(this);
                        Country.createMinisters(this);
                        Country.createStartingArmy(this);
                        Country.setEliteUnit(this);



                        /*update stats*/
                        listView1.Items.Add(Country.tag);

                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(Country.totalIC.ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(Country.totalLeadership.ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(Country.totalManpower.ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(Country.numberProvinces.ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(Country.countryArmy.Count.ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(Country.countryNaval.Count.ToString());
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(Country.countryAir.Count.ToString());


                        string tf = "N/A";
                        switch (Country.techFocus)
                        {
                            case 0:
                                tf = "Ta";
                                break;

                            case 1:
                                tf = "In";
                                break;

                            case 2:
                                tf = "Ai";
                                break;

                            case 3:
                                tf = "Nv";
                                break;


                        }

                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(tf);


                        Color color = Color.White;

                        switch (Country.intGovernment)
                        {
                            case 1:
                                color = Color.FromArgb(0, 0, 0);
                                break;
                            case 2:
                                color = Color.FromArgb(50, 50, 50);
                                break;
                            case 3:
                                color = Color.FromArgb(100, 100, 100);
                                break;
                            case 4:
                                color = Color.FromArgb(125, 125, 125);
                                break;
                            case 5:
                                color = Color.FromArgb(150, 150, 150);
                                break;
                            case 6:
                                color = Color.FromArgb(107, 114, 255);
                                break;
                            case 7:
                                color = Color.FromArgb(86, 173, 255);
                                break;
                            case 8:
                                color = Color.FromArgb(117, 201, 201);
                                break;
                            case 9:
                                color = Color.FromArgb(197, 140, 255);
                                break;
                            case 10:
                                color = Color.FromArgb(255, 163, 190);
                                break;
                            case 11:
                                color = Color.FromArgb(255, 109, 119);
                                break;
                            case 12:
                                color = Color.FromArgb(255, 87, 68);
                                break;


                        }

                        listView1.Items[listView1.Items.Count - 1].ForeColor = color;
                        listView1.Items[listView1.Items.Count - 1].ToolTipText = Country.government + " - " + Country.ideology;

                        listView1.Refresh();


                        totalIC = totalIC + Country.totalIC;
                        totalLeadership = totalLeadership + Country.totalLeadership;
                        totalMP = totalMP + Country.totalManpower;
                        totalProvinces = totalProvinces + Country.numberProvinces;



                        //create puppetable countries




                        /*****/
                    }


                    for (int count2 = countriesGenerated.Count; count2 <= listBox1.Items.Count - 1; count2++)
                    {
                        province auxProv;
                        string capital = "0";
                        countryGen = new country(count2, countries[count2].ToString());

                        countryGen.createLeaders(this);
                        countryGen.createMinisters(this);

                        auxProv = (province)provincesGenerated[ran.Next(0, provincesGenerated.Count)];

                        auxProv.countriesCore.Add(countryGen.tag);

                        if (auxProv.important)
                        {
                            capital = auxProv.id;
                        }
                        switch (ran.Next(0, 6))
                        {
                            case 0:
                            case 1:
                                foreach (province provadjaux in auxProv.adjeProvinceGenerated)
                                {
                                    if (ran.Next(0, 100) < 30)
                                    {
                                        provadjaux.countriesCore.Add(countryGen.tag);

                                        if (provadjaux.important && capital == "0")
                                        {
                                            capital = provadjaux.id;
                                        }
                                    }

                                }
                                break;
                            case 2:
                            case 3:
                                foreach (province provadjaux in auxProv.adjeProvinceGenerated)
                                {
                                    if (ran.Next(0, 100) < 80)
                                    {
                                        provadjaux.countriesCore.Add(countryGen.tag);
                                        if (provadjaux.important && capital == "0")
                                        {
                                            capital = provadjaux.id;
                                        }
                                    }

                                }
                                break;
                            case 4:
                                foreach (province provadjaux in auxProv.adjeProvinceGenerated)
                                {
                                    provadjaux.countriesCore.Add(countryGen.tag);
                                    foreach (province provadjaux2 in provadjaux.adjeProvinceGenerated)
                                    {

                                        if (provadjaux.important && capital == "0")
                                        {
                                            capital = provadjaux.id;
                                        }
                                        if (!provadjaux2.countriesCore.Contains(countryGen.tag))
                                        {
                                            if (ran.Next(0, 100) < 30)
                                            {
                                                provadjaux2.countriesCore.Add(countryGen.tag);
                                                if (provadjaux2.important && capital == "0")
                                                {
                                                    capital = provadjaux2.id;
                                                }
                                            }
                                        }
                                    }

                                }
                                break;
                            case 5:
                                foreach (province provadjaux in auxProv.adjeProvinceGenerated)
                                {
                                    provadjaux.countriesCore.Add(countryGen.tag);
                                    foreach (province provadjaux2 in provadjaux.adjeProvinceGenerated)
                                    {

                                        if (provadjaux.important && capital == "0")
                                        {
                                            capital = provadjaux.id;
                                        }
                                        if (!provadjaux2.countriesCore.Contains(countryGen.tag))
                                        {
                                            if (ran.Next(0, 100) < 80)
                                            {
                                                provadjaux2.countriesCore.Add(countryGen.tag);
                                                if (provadjaux2.important && capital == "0")
                                                {
                                                    capital = provadjaux2.id;
                                                }
                                            }
                                        }
                                    }

                                }
                                break;

                        }

                        if (capital == "0")
                        {
                            countryGen.capitalID = auxProv.id;
                        }
                        else
                        {
                            countryGen.capitalID = capital;
                        }
                        countriesPuppetableGenerated.Add(countryGen);
                    }


                }

                lTIC.Text = totalIC.ToString();
                lTLD.Text = totalLeadership.ToString();
                lTMP.Text = totalMP.ToString();
                lTPR.Text = totalProvinces.ToString();
                totalP = 100;
                progressBar1.PerformStep();

                progressBar1.Value = 80;

                labelStat.Text = "Writing..";
                labelStat.Refresh();
                this.Refresh();
                tabControl1.Enabled = true;
                Application.DoEvents();




                //leader of the faction
                selectFactionLeaders();

                bonusLeaders();


                //strategic resources
                bool ignoreLimit = false;
                int contaResL = 0;
                for (int contares = 0; contares <= strategic.Count - 1; contares++)
                {
                    for (int contaresLoop = 0; contaresLoop <= (ran.Next(Convert.ToInt32(this.textSRMIN.Text), Convert.ToInt32(this.textSRMAX.Text))); contaresLoop++)
                    {
                        while (1 == 1)
                        {
                            int ranProv = ran.Next(0, provincesGenerated.Count);
                            province ranProvcl = (province)provincesGenerated[ranProv];
                            if ((ranProvcl.importantSG && ranProvcl.resource == "") || ignoreLimit)
                            {
                                ranProvcl.resource = strategic[contares].ToString();
                                ranProvcl.points = ranProvcl.points + 20;
                                break;
                            }

                            if (contaResL > 30000)
                            {
                                ignoreLimit = true;
                            }
                            else
                            {
                                contaResL++;
                            }
                        }


                    }





                }
                progressBar1.Value = progressBar1.Value + 5;

                //writing

                write = new writing(this);
                write.writeAutoexec();

                write.writeProvinces();
                progressBar1.Value = progressBar1.Value + 5;
                write.writeCountries(); //include leaders,ministers, and units for that country
                progressBar1.Value = progressBar1.Value + 5;
                write.writePuppetableCountries(); //include leaders,ministers for that country
                progressBar1.Value = progressBar1.Value + 5;
                write.writeScenBookMark();
                write.writeScenBookMarkCGM();
                validRefreshThr = false;
                MessageBox.Show("Finish! Enjoy!");
                progressBar1.Visible = false;

                labelStat.Text = "Finish!";
                labelStat.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void addClaimLevel(province currentProvince, int levelClaims, int currentLevelClaims, string originalTag)
        {
            currentLevelClaims++;
            double minusChance = 0;

            minusChance = (currentLevelClaims * 0.07);


            foreach (province adjauxprovi in currentProvince.adjeProvinceGenerated)
            {



                if ((ran.Next(1, 101) <= ((Convert.ToInt32(textChanceLevelClaims.Text) - (Convert.ToInt32(textChanceLevelClaims.Text) * minusChance)))) || Convert.ToInt32(textChanceLevelClaims.Text) == 100)
                {
                    if (originalTag != adjauxprovi.ownerTag && provincesAuxArrayListNoDelete.Contains(adjauxprovi.id.ToString()))
                    {
                        if (!adjauxprovi.countriesCore.Contains(originalTag))
                        {
                            adjauxprovi.addCountryCore(originalTag);
                        }



                        if (currentLevelClaims <= levelClaims)
                        {
                            addClaimLevel(adjauxprovi, levelClaims, currentLevelClaims, originalTag);
                        }
                    }

                }
            }

        }


        private void bonusLeaders()
        {
            int count = 0;
            province bonusProvince;

            if (checkInfraLeaders.Checked || checkLFNuR.Checked)
            {
                for (count = 0; count <= provincesGenerated.Count - 1; count++)
                {
                    bonusProvince = (province)provincesGenerated[count];
                    if (checkInfraLeaders.Checked)
                    {
                        bonusProvince.addInfraToMajors(1, axTag, aTag, cTag);
                    }

                    if (checkLFNuR.Checked)
                    {
                        bonusProvince.addNuclearToMajors(1, capitalAxTag, capitalATag, capitalCTag);
                    }

                }
            }




        }

        private void selectFactionLeaders()
        {

            int count;
            int count2;




            country possibleLeader;

            country possibleLeaderAuxA = new country(0, "---");
            country possibleLeaderAuxAx = new country(0, "---");
            country possibleLeaderAuxC = new country(0, "---");


            ////random! //IMPORTANT if anyone uses this remember to capture the capital id
            //if (radioLFRandom.Checked){
            //    axTag = axisCountries[form.ran.Next(0,axisCountries.Count -1)].ToString();
            //    aTag = alliesCountries[form.ran.Next(0, alliesCountries.Count - 1)].ToString();
            //    cTag = conCountries[form.ran.Next(0, conCountries.Count - 1)].ToString();

            //    for (count2 = 0; count2 <= countriesGenerated.Count - 1; count++)
            //    {
            //        possibleLeader = (country)countriesGenerated[count2];

            //        for (count = 0; count <= axisCountries.Count - 1; count++)
            //        {
            //            if (possibleLeader.tag == axTag)
            //            {
            //                possibleLeader.major = true;
            //                possibleLeader.alignmentX = 200;
            //                possibleLeader.alignmentY = 200;

            //                if (checkLFNU.Checked)
            //                {
            //                    possibleLeader.national_unity = possibleLeader.national_unity + 10;
            //                }


            //                if (possibleLeader.national_unity > 100)
            //                {
            //                    possibleLeader.national_unity = 100;
            //                }

            //                if (checkLFNeu)
            //                {
            //                    possibleLeader.neutrality = possibleLeader.neutrality - 10;
            //                }



            //                if (possibleLeader.neutrality > 0)
            //                {
            //                    possibleLeader.neutrality = 0;
            //                }

            //                break;
            //            }
            //        }


            //        for (count = 0; count <= alliesCountries.Count - 1; count++)
            //        {
            //            if (possibleLeader.tag == aTag)
            //            {
            //                possibleLeader.major = true;
            //                possibleLeader.alignmentX = 0;
            //                possibleLeader.alignmentY = -146;

            //                if (checkLFNU.Checked)
            //                {
            //                    possibleLeader.national_unity = possibleLeader.national_unity + 10;
            //                }


            //                if (possibleLeader.national_unity > 100)
            //                {
            //                    possibleLeader.national_unity = 100;
            //                }

            //                if (checkLFNeu)
            //                {
            //                    possibleLeader.neutrality = possibleLeader.neutrality - 10;
            //                }

            //                if (possibleLeader.neutrality > 0)
            //                {
            //                    possibleLeader.neutrality = 0;
            //                }
            //                break;
            //            }
            //        }


            //        for (count = 0; count <= conCountries.Count - 1; count++)
            //        {
            //            if (possibleLeader.tag == cTag)
            //            {
            //                possibleLeader.major = true;
            //                possibleLeader.alignmentX = -200;
            //                possibleLeader.alignmentY = 200;

            //                if (checkLFNU.Checked)
            //                {
            //                    possibleLeader.national_unity = possibleLeader.national_unity + 10;
            //                }


            //                if (possibleLeader.national_unity > 100)
            //                {
            //                    possibleLeader.national_unity = 100;
            //                }

            //                if (checkLFNeu)
            //                {
            //                    possibleLeader.neutrality = possibleLeader.neutrality - 10;
            //                }

            //                if (possibleLeader.neutrality > 0)
            //                {
            //                    possibleLeader.neutrality = 0;
            //                }

            //                break;
            //            }
            //        }

            //    }


            //}

            //more leadership
            if (radioLFMoreLD.Checked)
            {

                for (count2 = 0; count2 <= countriesGenerated.Count - 1; count2++)
                {
                    possibleLeader = (country)countriesGenerated[count2];

                    for (count = 0; count <= axisCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == axisCountries[count].ToString())
                        {
                            if (possibleLeader.totalLeadership > possibleLeaderAuxAx.totalLeadership)
                            {
                                possibleLeaderAuxAx = possibleLeader;
                            }

                        }

                    }

                    for (count = 0; count <= alliesCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == alliesCountries[count].ToString())
                        {
                            if (possibleLeader.totalLeadership > possibleLeaderAuxA.totalLeadership)
                            {
                                possibleLeaderAuxA = possibleLeader;
                            }

                        }
                    }

                    for (count = 0; count <= conCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == conCountries[count].ToString())
                        {
                            if (possibleLeader.totalLeadership > possibleLeaderAuxC.totalLeadership)
                            {
                                possibleLeaderAuxC = possibleLeader;
                            }

                        }
                    }

                }

            }



            if (radioLFMoreIC.Checked)
            {

                for (count2 = 0; count2 <= countriesGenerated.Count - 1; count2++)
                {
                    possibleLeader = (country)countriesGenerated[count2];

                    for (count = 0; count <= axisCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == axisCountries[count].ToString())
                        {
                            if (possibleLeader.totalIC > possibleLeaderAuxAx.totalIC)
                            {
                                possibleLeaderAuxAx = possibleLeader;
                            }

                        }

                    }

                    for (count = 0; count <= alliesCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == alliesCountries[count].ToString())
                        {
                            if (possibleLeader.totalIC > possibleLeaderAuxA.totalIC)
                            {
                                possibleLeaderAuxA = possibleLeader;
                            }

                        }
                    }

                    for (count = 0; count <= conCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == conCountries[count].ToString())
                        {
                            if (possibleLeader.totalIC > possibleLeaderAuxC.totalIC)
                            {
                                possibleLeaderAuxC = possibleLeader;
                            }

                        }
                    }

                }

            }



            if (radioLFMorePR.Checked)
            {

                for (count2 = 0; count2 <= countriesGenerated.Count - 1; count2++)
                {
                    possibleLeader = (country)countriesGenerated[count2];

                    for (count = 0; count <= axisCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == axisCountries[count].ToString())
                        {
                            if (possibleLeader.numberProvinces > possibleLeaderAuxAx.numberProvinces)
                            {
                                possibleLeaderAuxAx = possibleLeader;
                            }

                        }

                    }

                    for (count = 0; count <= alliesCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == alliesCountries[count].ToString())
                        {
                            if (possibleLeader.numberProvinces > possibleLeaderAuxA.numberProvinces)
                            {
                                possibleLeaderAuxA = possibleLeader;
                            }

                        }
                    }

                    for (count = 0; count <= conCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == conCountries[count].ToString())
                        {
                            if (possibleLeader.numberProvinces > possibleLeaderAuxC.numberProvinces)
                            {
                                possibleLeaderAuxC = possibleLeader;
                            }

                        }
                    }

                }

            }


            if (radioLFMoreVP.Checked)
            {

                for (count2 = 0; count2 <= countriesGenerated.Count - 1; count2++)
                {
                    possibleLeader = (country)countriesGenerated[count2];

                    for (count = 0; count <= axisCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == axisCountries[count].ToString())
                        {
                            if (possibleLeader.totalPoints > possibleLeaderAuxAx.totalPoints)
                            {
                                possibleLeaderAuxAx = possibleLeader;
                            }

                        }

                    }

                    for (count = 0; count <= alliesCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == alliesCountries[count].ToString())
                        {
                            if (possibleLeader.totalPoints > possibleLeaderAuxA.totalPoints)
                            {
                                possibleLeaderAuxA = possibleLeader;
                            }

                        }
                    }

                    for (count = 0; count <= conCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == conCountries[count].ToString())
                        {
                            if (possibleLeader.totalPoints > possibleLeaderAuxC.totalPoints)
                            {
                                possibleLeaderAuxC = possibleLeader;
                            }

                        }
                    }

                }

            }

            if (radioLFMoreMP.Checked)
            {

                for (count2 = 0; count2 <= countriesGenerated.Count - 1; count2++)
                {
                    possibleLeader = (country)countriesGenerated[count2];

                    for (count = 0; count <= axisCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == axisCountries[count].ToString())
                        {
                            if (possibleLeader.totalManpower > possibleLeaderAuxAx.totalManpower)
                            {
                                possibleLeaderAuxAx = possibleLeader;
                            }

                        }

                    }

                    for (count = 0; count <= alliesCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == alliesCountries[count].ToString())
                        {
                            if (possibleLeader.totalManpower > possibleLeaderAuxA.totalManpower)
                            {
                                possibleLeaderAuxA = possibleLeader;
                            }

                        }
                    }

                    for (count = 0; count <= conCountries.Count - 1; count++)
                    {
                        if (possibleLeader.tag == conCountries[count].ToString())
                        {
                            if (possibleLeader.totalManpower > possibleLeaderAuxC.totalManpower)
                            {
                                possibleLeaderAuxC = possibleLeader;
                            }

                        }
                    }

                }

            }

            //if (!radioLFRandom.Checked)
            // {
            axTag = possibleLeaderAuxAx.tag;
            aTag = possibleLeaderAuxA.tag;
            cTag = possibleLeaderAuxC.tag;

            capitalAxTag = possibleLeaderAuxAx.capitalID;
            capitalATag = possibleLeaderAuxA.capitalID;
            capitalCTag = possibleLeaderAuxC.capitalID;

            possibleLeaderAuxAx.faction = "axis";
            possibleLeaderAuxA.faction = "allies";
            possibleLeaderAuxC.faction = "comintern";

            if (checkLFNU.Checked)
            {
                possibleLeaderAuxAx.national_unity = possibleLeaderAuxAx.national_unity + 10;
                possibleLeaderAuxA.national_unity = possibleLeaderAuxA.national_unity + 10;
                possibleLeaderAuxC.national_unity = possibleLeaderAuxC.national_unity + 10;
            }

            if (checkLFNeu.Checked)
            {
                possibleLeaderAuxAx.neutrality = possibleLeaderAuxAx.neutrality - 10;
                possibleLeaderAuxA.neutrality = possibleLeaderAuxA.neutrality - 10;
                possibleLeaderAuxC.neutrality = possibleLeaderAuxC.neutrality - 10;
            }


            foreach (ListViewItem ls in listView1.Items)
            {
                string fac = "Neutral";
                /*refresh stats*/
                if (ls.Text == possibleLeaderAuxAx.tag)
                {
                    fac = "Axis";
                }

                if (ls.Text == possibleLeaderAuxA.tag)
                {
                    fac = "Allies";
                }

                if (ls.Text == possibleLeaderAuxC.tag)
                {
                    fac = "Commintern";
                }
                ls.SubItems.Add(fac);
            }

            possibleLeaderAuxAx.major = true;
            possibleLeaderAuxAx.alignmentX = 200;
            possibleLeaderAuxAx.alignmentY = 200;


            if (possibleLeaderAuxAx.national_unity > 100)
            {
                possibleLeaderAuxAx.national_unity = 100;
            }



            if (possibleLeaderAuxAx.neutrality < 0)
            {
                possibleLeaderAuxAx.neutrality = 0;
            }

            possibleLeaderAuxA.major = true;
            possibleLeaderAuxA.alignmentX = 0;
            possibleLeaderAuxA.alignmentY = -146;



            if (possibleLeaderAuxA.national_unity > 100)
            {
                possibleLeaderAuxA.national_unity = 100;
            }



            if (possibleLeaderAuxA.neutrality < 0)
            {
                possibleLeaderAuxA.neutrality = 0;
            }


            possibleLeaderAuxC.major = true;
            possibleLeaderAuxC.alignmentX = -200;
            possibleLeaderAuxC.alignmentY = 200;



            if (possibleLeaderAuxC.national_unity > 100)
            {
                possibleLeaderAuxC.national_unity = 100;
            }



            if (possibleLeaderAuxC.neutrality < 0)
            {
                possibleLeaderAuxC.neutrality = 0;
            }



            // }



        }


        //Here we read the original minsiter files, so we can get the vanilla units names for that country. The ministers for the countries will generate later.
        private void dummyMinsiterFiles()
        {


            FileStream file1 = new FileStream(Application.StartupPath + "/countriesNewNations.txt", FileMode.Open, FileAccess.Read);
            StreamReader sw1 = new StreamReader(file1);




            FileStream file;
            StreamWriter sw;

            //we create the files for every country. Yes, all countries, used or not.

            while (!sw1.EndOfStream)
            {
                file = new FileStream(Application.StartupPath + "/common/countries/" + sw1.ReadLine().Substring(0, 3) + ".txt", FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(file, Encoding.GetEncoding(1252)); //encoding is necesary or data will be lost (and the file will fail to create correctly)
                sw.Flush();
                sw.Close();

            }

            sw1.Close();
        }

        private void readResourcesValues()
        {

            if (radioRes6.Checked)
            {
                por_Res = 1.50;
            }
            if (radioRes0.Checked)
            {
                por_Res = 1.25;
            }
            if (radioRes1.Checked)
            {
                por_Res = 1;
            }
            if (radioRes2.Checked)
            {
                por_Res = 0.8;
            }
            if (radioRes3.Checked)
            {
                por_Res = 0.6;
            }
            if (radioRes4.Checked)
            {
                por_Res = 0.5;
            }
            if (radioRes5.Checked)
            {
                por_Res = 0.33;

            }

            if (radiolea00.Checked)
            {
                por_lea = 1.50;
            }

            if (radiolea0.Checked)
            {
                por_lea = 1.25;
            }
            if (radiolea1.Checked)
            {
                por_lea = 1;
            }
            if (radiolea2.Checked)
            {
                por_lea = 0.75;
            }

            if (radiolea3.Checked)
            {
                por_lea = 0.50;
            }

            if (radioICM0.Checked)
            {
                por_IC = 1.25;
            }
            if (radioICM1.Checked)
            {
                por_IC = 1;
            }
            if (radioICM2.Checked)
            {
                por_IC = 0.75;
            }

            if (radioICM3.Checked)
            {
                por_IC = 0.50;
            }

            if (radioRM0.Checked)
            {
                por_MP = 1.25;
            }
            if (radioRM1.Checked)
            {
                por_MP = 1;
            }
            if (radioRM2.Checked)
            {
                por_MP = 0.75;
            }

            if (radioRM3.Checked)
            {
                por_MP = 0.50;
            }

            if (radioOil0.Checked)
            {
                por_oil = 1;
            }
            if (radioOil1.Checked)
            {
                por_oil = 0.75;
            }

            if (radioOil2.Checked)
            {
                por_oil = 0.50;
            }

            if (checkOilWar.Checked)
            {
                por_oil = por_oil * 2;
                probOil = 30;
                probOil2 = 100;
            }

        }

        public bool checkCoastal(String provinciaAct)
        {
            int count;
            for (count = 0; count <= navalProvinces.Count - 1; count++)
            {
                if (navalProvinces[count].ToString() == provinciaAct)
                {
                    return true;
                }
            }
            return false;
        }

        public bool checkArtic(String provinciaAct)
        {
            int count;
            for (count = 0; count <= articProvinces.Count - 1; count++)
            {
                if (articProvinces[count].ToString() == provinciaAct)
                {
                    return true;
                }
            }
            return false;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void textSRMIN_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textSRMAX.Text) < Convert.ToInt32(textSRMIN.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun SR  unity must be equal or less than Maximun SR");
                    textSRMIN.Text = textSRMAX.Text;
                }

                if (Convert.ToInt32(textSRMIN.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun SR must be between MAX SR  and 0");
                    textSRMIN.Text = textSRMAX.Text;
                }
            }
            catch (Exception ex)
            {
                textSRMIN.Text = textSRMAX.Text;
            }

        }

        private void textSRMAX_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textSRMAX.Text) < Convert.ToInt32(textSRMIN.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun SR  must be equal or higher than Minimun SR");
                    textSRMAX.Text = textSRMIN.Text;
                }

                if (Convert.ToInt32(textSRMAX.Text) > 5)
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun SR  must be between min SR  and 5");
                    textSRMAX.Text = "5";
                }
            }
            catch (Exception ex)
            {
                textSRMAX.Text = textSRMIN.Text;
            }
        }

        private void label63_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {

            try
            {

                Convert.ToInt32(this.textBoxInfraColon.Text);

            }
            catch (Exception ex)
            {
                this.textBoxInfraColon.Text = "2";
            }


        }

        private void textLevelClaims_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textLevelClaims.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun level must be 0 or higher");
                    textLevelClaims.Text = "5";
                }

                if (Convert.ToInt32(textLevelClaims.Text) > 7)
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun level  must be lower than 8");
                    textLevelClaims.Text = "5";
                }

                if (Convert.ToInt32(textLevelClaims.Text) > 5 && Convert.ToInt32(textLevelClaims.Text) < 8)
                {
                    MessageBox.Show("Warning! Any level above 5 (and high chance) can slow down a lot the generator (from 1 minute to 10 or more!!!)", "Watch out!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            catch (Exception ex)
            {
                textLevelClaims.Text = "5";
            }
        }

        private void textChanceLevelClaims_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textChanceLevelClaims.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun chance must be 0 or higher");
                    textChanceLevelClaims.Text = "60";
                }

                if (Convert.ToInt32(textChanceLevelClaims.Text) > 100)
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun chance  must be lower than 101");
                    textChanceLevelClaims.Text = "60";
                }
            }
            catch (Exception ex)
            {
                textChanceLevelClaims.Text = "60";
            }
        }

        private void textChanceFortsBorders_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textChanceFortsBorders.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun chance for forts must be 0 or higher");
                    textChanceFortsBorders.Text = "80";
                }

                if (Convert.ToInt32(textChanceFortsBorders.Text) > 100)
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun chance for forts must be lower than 101");
                    textChanceFortsBorders.Text = "80";
                }
            }
            catch (Exception ex)
            {
                textChanceFortsBorders.Text = "80";
            }
        }

        private void textMinFortBorder_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textMaxFortBorder.Text) < Convert.ToInt32(textMinFortBorder.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun fort border must be equal or less than Maximun fort border");
                    textMinFortBorder.Text = textMaxFortBorder.Text;
                }

                if (Convert.ToInt32(textMinFortBorder.Text) < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Minimun for border must be between maximun fort border and 0");
                    textMinFortBorder.Text = textMaxFortBorder.Text;
                }
            }
            catch (Exception ex)
            {
                textMinFortBorder.Text = textMaxFortBorder.Text;
            }
        }

        private void textMaxFortBorder_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(textMaxFortBorder.Text) < Convert.ToInt32(textMinFortBorder.Text))
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun fort border must be equal or higher than Minimun for border");
                    textMaxFortBorder.Text = textMinFortBorder.Text;
                }

                if (Convert.ToInt32(textMaxFortBorder.Text) > 10)
                {
                    e.Cancel = true;
                    MessageBox.Show("Maximun fort border must be between Minimun for border  and 10");
                    textMaxFortBorder.Text = "10";
                }
            }
            catch (Exception ex)
            {
                textMaxFortBorder.Text = textMinFortBorder.Text;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > 199)
            {
                MessageBox.Show("WARNING! It is not recomended to use more that 200 countries. The game can be very slow. You can try it of course, but for a nice Random Hoi Experiencie use between 70 and 120 countries.", "Whoa!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


    }
}

