using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace RandomHoiV2
{

    class writing
    {
        Form1 form;

        public writing(Form1 oriForm)
        {

            form = oriForm;

        }

        public void writeAutoexec()
        {
            FileStream file1 = new FileStream(Application.StartupPath + "/luaTemplate/autoexecTEMPLATE.lua", FileMode.Open, FileAccess.Read);
            StreamReader sw1 = new StreamReader(file1);

            FileStream file = new FileStream(Application.StartupPath + "/script/autoexec.lua", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file, Encoding.GetEncoding(1252));

            while (sw1.EndOfStream == false)
            {
                Application.DoEvents();
                sw.WriteLine(sw1.ReadLine());
            }

            sw.Close();
            sw1.Close();

        }

        public void writeProvinces()
        {

            FileStream file;
            StreamWriter sw;

            province writingProvince;

            int count = 0;
            int count2 = 0;

            for (count = 0; count <= form.provincesGenerated.Count - 1; count++)
            {
                Application.DoEvents();
                writingProvince = (province)form.provincesGenerated[count];

                file = new FileStream(Application.StartupPath + "/history/provinces/z/" + writingProvince.id + " - PROV" + count + ".txt", FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(file);

                sw.WriteLine("owner = " + writingProvince.ownerTag);
                sw.WriteLine("controller = " + writingProvince.controllerTag);

                for (count2 = 0; count2 <= writingProvince.countriesCore.Count - 1; count2++)
                {
                    sw.WriteLine("add_core = " + (string)writingProvince.countriesCore[count2]);
                }

                sw.WriteLine("");

                if (writingProvince.artic)
                {
                    sw.WriteLine("terrain = arctic");
                    sw.WriteLine("");
                }

                if (writingProvince.points > 0)
                {
                    sw.WriteLine("points = " + writingProvince.points.ToString());
                }


                if (writingProvince.metal > 0)
                {
                    sw.WriteLine("metal = " + writingProvince.metal.ToString().Replace(",", "."));
                }
                if (writingProvince.energy > 0)
                {
                    sw.WriteLine("energy = " + writingProvince.energy.ToString().Replace(",", "."));
                }
                if (writingProvince.rare > 0)
                {
                    sw.WriteLine("rare_materials = " + writingProvince.rare.ToString().Replace(",", "."));
                }
                if (writingProvince.oil > 0)
                {
                    sw.WriteLine("crude_oil = " + writingProvince.oil.ToString().Replace(",", "."));
                }
                if (writingProvince.leadership > 0)
                {
                    sw.WriteLine("leadership = " + writingProvince.leadership.ToString().Replace(",", "."));

                }
                if (writingProvince.manpower > 0)
                {
                    sw.WriteLine("manpower = " + writingProvince.manpower.ToString().Replace(",", "."));
                }
                if (writingProvince.industry > 0)
                {
                    sw.WriteLine("industry = " + writingProvince.industry.ToString().Replace(",", "."));
                }
                if (writingProvince.AAA > 0)
                {
                    sw.WriteLine("anti_air = " + writingProvince.AAA.ToString().Replace(",", "."));
                }
                if (writingProvince.airBase > 0)
                {
                    sw.WriteLine("air_base = " + writingProvince.airBase.ToString().Replace(",", "."));
                }
                if (writingProvince.navalBase > 0)
                {
                    sw.WriteLine("naval_base = " + writingProvince.navalBase.ToString().Replace(",", "."));
                }
                if (writingProvince.navalFort > 0)
                {
                    sw.WriteLine("coastal_fort  = " + writingProvince.navalFort.ToString().Replace(",", "."));
                }
                if (writingProvince.landFort > 0)
                {
                    sw.WriteLine("land_fort = " + writingProvince.landFort.ToString().Replace(",", "."));
                }


                sw.WriteLine("");

                if (writingProvince.rocket > 0)
                {
                    sw.WriteLine("rocket_test = " + writingProvince.rocket.ToString().Replace(",", "."));
                }
                if (writingProvince.nuclear > 0)
                {
                    sw.WriteLine("nuclear_reactor = " + writingProvince.nuclear.ToString().Replace(",", "."));
                }

                sw.WriteLine("");

                sw.WriteLine("infra = " + writingProvince.infra.ToString().Replace(",", "."));

                sw.WriteLine("");

                if (writingProvince.resource != "")
                {
                    sw.WriteLine("strategic_resource = " + writingProvince.resource);
                    sw.WriteLine("");

                }

                sw.Flush();
                sw.Close();
            }



        }

        public void writeCountries()
        {
            FileStream file;
            StreamWriter sw;

            country writingCountry;

            int count = 0;



            //starting countries
            for (count = 0; count <= form.countriesGenerated.Count - 1; count++)
            {
                Application.DoEvents();
                writingCountry = (country)form.countriesGenerated[count];

                FileStream file1 = new FileStream(Application.StartupPath + "/script/country/" + writingCountry.tag + ".lua", FileMode.Create, FileAccess.Write);
                file1.Close();
                file = new FileStream(Application.StartupPath + "/history/countries/" + writingCountry.tag + ".txt", FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(file);
                sw.WriteLine("capital = " + writingCountry.capitalID);

                sw.WriteLine("neutrality = " + writingCountry.neutrality.ToString());
                sw.WriteLine("national_unity = " + writingCountry.national_unity.ToString());

                sw.WriteLine("government = " + writingCountry.government.ToString());
                sw.WriteLine("ideology = " + writingCountry.ideology.ToString());

                sw.WriteLine("alignment = { x = " + writingCountry.alignmentX.ToString() + " y = " + writingCountry.alignmentY.ToString() + " }");

                if (writingCountry.faction != "neutral")
                {
                    sw.WriteLine("join_faction = " + writingCountry.faction);
                }

                sw.WriteLine();
                sw.WriteLine("organization = { ");

                sw.WriteLine("national_socialist =" + writingCountry.partyOrganization[0]);
                sw.WriteLine("fascistic = " + writingCountry.partyOrganization[1]);
                sw.WriteLine("paternal_autocrat = " + writingCountry.partyOrganization[2]);
                sw.WriteLine("social_conservative = " + writingCountry.partyOrganization[3]);
                sw.WriteLine("market_liberal = " + writingCountry.partyOrganization[4]);
                sw.WriteLine("social_liberal = " + writingCountry.partyOrganization[5]);
                sw.WriteLine("social_democrat = " + writingCountry.partyOrganization[6]);
                sw.WriteLine("left_wing_radical = " + writingCountry.partyOrganization[7]);
                sw.WriteLine("leninist = " + writingCountry.partyOrganization[8]);
                sw.WriteLine("stalinist = " + writingCountry.partyOrganization[9]);

                sw.WriteLine("}");

                // popularity of the parties can be not set. Will be random at the start of the scen.

                //practical and officers

                sw.WriteLine();

                if (form.radioButPTTF.Checked)
                {
                    FileStream practicals;
                    StreamReader sr;
                    string fileN;

                    switch (writingCountry.techFocus)
                    {
                        case 0:
                            fileN = "PracticalTanks.txt";
                            break;
                        case 1:
                            fileN = "PracticalInf.txt";
                            break;
                        case 2:
                            fileN = "PracticalAir.txt";
                            break;
                        case 3:
                            fileN = "PracticalNav.txt";
                            break;
                        default: /*when not using tech focus*/
                            fileN = "PracticaDefault.txt";
                            break;

                    }

                    practicals = new FileStream(Application.StartupPath + "/" + fileN, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(practicals);

                    while (!sr.EndOfStream)
                    {
                        sw.WriteLine(sr.ReadLine());
                    }

                }

                if (form.radioButPTRA.Checked)
                {
                    sw.WriteLine("infantry_theory = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("militia_theory = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("mobile_theory = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("artillery_theory = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("naval_engineering = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("automotive_theory = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("aeronautic_engineering = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("single_engine_aircraft_practical = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("infantry_practical = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("militia_practical  = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("mobile_practical = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("artillery_practical = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("destroyer_practical  = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("armour_practical  = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("transport_practical  = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("fleet_in_being_doctrine   = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("capitalship_practical   = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("carrier_practical    = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("cruiser_practical     = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("twin_engine_aircraft_practical  = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("four_engine_aircraft_practical   = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("fighter_focus   = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("superior_firepower_theory  = " + form.ran.Next(0, 10).ToString());
                    sw.WriteLine("spearhead_theory   = " + form.ran.Next(0, 10).ToString());
                }

                if (!form.checkSmallBonus.Checked)
                {
                    sw.WriteLine("1935.3.1 = {  set_country_flag = noSmallBonus }");
                }

                if (!form.checkResources.Checked)
                {
                    sw.WriteLine("1935.2.1 = {  set_country_flag = resources  }");
                }


                double auxof;
                string off;
                auxof = (Convert.ToDouble(form.textOffRa.Text) / 100);
                off = auxof.ToString().Replace(",", ".");

                sw.WriteLine("officers_ratio =" + off);

                //TECHS

                //we read the tech file and write it

                FileStream fileR;
                StreamReader swR;
                String fileStr = "";


                switch (writingCountry.techFocus)
                {

                    case 0:
                        fileStr = Application.StartupPath + "/techTanks.txt";
                        break;

                    case 1:
                        fileStr = Application.StartupPath + "/techInf.txt";
                        break;

                    case 2:
                        fileStr = Application.StartupPath + "/techAir.txt";
                        break;

                    case 3:
                        fileStr = Application.StartupPath + "/techNaval.txt";
                        break;

                    case 4:
                        fileStr = Application.StartupPath + "/techMinimun.txt";
                        break;

                    case 5:
                        fileStr = Application.StartupPath + "/techGermany.txt";
                        break;

                    case 6:
                        fileStr = Application.StartupPath + "/techUK.txt";
                        break;

                    case 7:
                        fileStr = Application.StartupPath + "/techGer39.txt";
                        break;

                }





                fileR = new FileStream(fileStr, FileMode.Open, FileAccess.Read);
                swR = new StreamReader(fileR);
                while (!(swR.EndOfStream))
                {

                    sw.WriteLine(swR.ReadLine());


                }

                swR.Close();

                sw.WriteLine();

                switch (writingCountry.techFocus)
                {

                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        if (writingCountry.totalIC < 20)
                        {
                            fileR = new FileStream(Application.StartupPath + "/techbonusless20ic.txt", FileMode.Open, FileAccess.Read);
                            swR = new StreamReader(fileR);
                            while (!(swR.EndOfStream))
                            {

                                sw.WriteLine(swR.ReadLine());


                            }
                            swR.Close();

                        }
                        else
                            if (writingCountry.totalIC < 30)
                            {
                                fileR = new FileStream(Application.StartupPath + "/techbonusless30ic.txt", FileMode.Open, FileAccess.Read);
                                swR = new StreamReader(fileR);
                                while (!(swR.EndOfStream))
                                {

                                    sw.WriteLine(swR.ReadLine());


                                }
                                swR.Close();

                            }
                            else
                                if (writingCountry.totalIC < 40)
                                {
                                    fileR = new FileStream(Application.StartupPath + "/techbonusless40ic.txt", FileMode.Open, FileAccess.Read);
                                    swR = new StreamReader(fileR);
                                    while (!(swR.EndOfStream))
                                    {

                                        sw.WriteLine(swR.ReadLine());


                                    }
                                    swR.Close();

                                }
                        break;


                    default:
                        break;

                }

                sw.WriteLine();


                //laws

                sw.WriteLine("training_laws = advanced_training");
                sw.WriteLine("press_laws = free_press");
                sw.WriteLine("industrial_policy_laws = mixed_industry");
                sw.WriteLine("education_investment_law = medium_large_education_investment");
                sw.WriteLine("economic_law = basic_mobilisation");
                sw.WriteLine("conscription_law = volunteer_army");
                sw.WriteLine("civil_law = open_society");
                sw.WriteLine("exterior_laws = do_the_minimal_for_status_quo");


                sw.WriteLine();
                //name of the oob file
                sw.WriteLine("oob = \"" + writingCountry.tag + ".txt\"");

                sw.WriteLine();

                switch (form.comboNOWAR.SelectedIndex)
                {
                    case 0:
                        sw.WriteLine("1936.1.1 = { decision = non_agg3 }");
                        break;
                    case 1:
                        sw.WriteLine("1935.1.1 = { decision = non_agg }");
                        break;
                    case 2:
                        sw.WriteLine("1936.1.1 = { decision = non_agg2 }");
                        break;
                    default:
                        sw.WriteLine("1935.1.1 = { decision = non_agg }");
                        break;

                }



                sw.WriteLine();

                switch (writingCountry.eliteunit)
                {

                    case "Alpini_brigade":
                        sw.WriteLine("1935.4.1 = {  set_country_flag = eliteUnit }");
                        sw.WriteLine("activate_AiB = 1");
                        break;

                    case "Alpins_brigade":
                        sw.WriteLine("1936.1.1 = {  set_country_flag = eliteUnit }");
                        sw.WriteLine("activate_AfB = 1");
                        break;

                    case "Guards_brigade":
                        sw.WriteLine("1935.4.1 = {  set_country_flag = eliteUnit }");
                        sw.WriteLine("activate_GuB = 1");
                        break;

                    case "legion_brigade":
                        sw.WriteLine("1935.4.1 = {  set_country_flag = eliteUnit }");
                        sw.WriteLine("activate_LE = 1");
                        break;

                    case "waffenSS_brigade":
                        sw.WriteLine("1935.4.1 = {  set_country_flag = eliteUnit }");
                        sw.WriteLine("activate_WF = 1");
                        break;

                    case "Gurkha_brigade":
                        sw.WriteLine("1935.4.1 = {  set_country_flag = eliteUnit }");
                        sw.WriteLine("activate_GB = 1");
                        break;

                    case "ranger_brigade":
                        sw.WriteLine("1935.4.1 = {  set_country_flag = eliteUnit }");
                        sw.WriteLine("activate_RB = 1");
                        break;

                    case "Imperial_brigade":
                        sw.WriteLine("1935.4.1 = {  set_country_flag = eliteUnit }");
                        sw.WriteLine("activate_ImB = 1");
                        break;

                    default:
                        break;

                }
                sw.WriteLine();
                sw.Flush();
                sw.Close();

                writeLua(writingCountry);


                writeLeaders(writingCountry);
                writeMinisters(writingCountry);
                writeUnits(writingCountry);

            }


        }

        public void writeLua(country writingCountry)
        {
            if (form.radioMinimun.Checked && form.checkTechFocus.Checked)
            {
                FileStream file;
                StreamReader sr;
                FileStream file1 = new FileStream(Application.StartupPath + "/script/autoexec.lua", FileMode.Append, FileAccess.Write);
                StreamWriter sw1 = new StreamWriter(file1);

                sw1.WriteLine("require('" + writingCountry.tag + "')");

                sw1.Close();

                file1 = new FileStream(Application.StartupPath + "/script/country/" + writingCountry.tag + ".lua", FileMode.Create, FileAccess.Write);
                sw1 = new StreamWriter(file1);

                switch (writingCountry.techFocus)
                {
                    case 0:
                        file = new FileStream(Application.StartupPath + "/luaTemplate/tankTemplate.lua", FileMode.Open, FileAccess.Read);
                        sr = new StreamReader(file);
                        break;

                    case 1:
                        file = new FileStream(Application.StartupPath + "/luaTemplate/infatryTEMPLATE.lua", FileMode.Open, FileAccess.Read);
                        sr = new StreamReader(file);
                        break;
                    case 2:
                        file = new FileStream(Application.StartupPath + "/luaTemplate/airTEMPLATE.lua", FileMode.Open, FileAccess.Read);
                        sr = new StreamReader(file);
                        break;
                    case 3:
                        file = new FileStream(Application.StartupPath + "/luaTemplate/shipTemplate.lua", FileMode.Open, FileAccess.Read);
                        sr = new StreamReader(file);
                        break;
                    default:
                        file = new FileStream(Application.StartupPath + "/luaTemplate/infatryTEMPLATE.lua", FileMode.Open, FileAccess.Read);
                        sr = new StreamReader(file);
                        break;


                }

                sw1.WriteLine("local P = {}");
                sw1.WriteLine("AI_" + writingCountry.tag + " = P");


                while (sr.EndOfStream == false)
                {
                    sw1.WriteLine(sr.ReadLine());
                }

                sw1.WriteLine("return AI_" + writingCountry.tag);

                sw1.Close();
                sr.Close();

            }

        }

        public void writePuppetableCountries()
        {
            FileStream file;
            StreamWriter sw;

            country writingCountry;

            int count = 0;
            int count2 = 0;


            //puppetable countries
            for (count = 0; count <= form.countriesPuppetableGenerated.Count - 1; count++)
            {
                Application.DoEvents();
                writingCountry = (country)form.countriesPuppetableGenerated[count];

                file = new FileStream(Application.StartupPath + "/history/countries/" + writingCountry.tag + ".txt", FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(file);
                sw.WriteLine("capital = " + writingCountry.capitalID);


                sw.WriteLine();
                sw.WriteLine();
                sw.Flush();
                sw.Close();


                writeLeaders(writingCountry);
                writeMinisters(writingCountry);

                FileStream file1 = new FileStream(Application.StartupPath + "/script/country/" + writingCountry.tag + ".lua", FileMode.Create, FileAccess.Write);

                if (form.radioMinimun.Checked && form.checkTechFocus.Checked)
                {
                    FileStream file2 = new FileStream(Application.StartupPath + "/script/autoexec.lua", FileMode.Append, FileAccess.Write);
                    StreamWriter sw1 = new StreamWriter(file2);

                    sw1.WriteLine("require('" + writingCountry.tag + "')");

                    sw1.Close();


                }

            }



        }

        public void writeLeaders(country writingCountry)
        {
            FileStream file = new FileStream(Application.StartupPath + "/history/leaders/" + writingCountry.tag + ".txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);

            int count = 0;
            int count2 = 0;

            leader countryLeader;

            for (count = 0; count <= writingCountry.countryLeaders.Count - 1; count++)
            {
                Application.DoEvents();
                countryLeader = (leader)writingCountry.countryLeaders[count];

                sw.WriteLine(countryLeader.id + " = {");
                sw.WriteLine("name=\"" + countryLeader.name + "\"");
                sw.WriteLine("type =" + countryLeader.type);
                sw.WriteLine("country = " + writingCountry.tag);
                sw.WriteLine("skill = " + countryLeader.skill);
                sw.WriteLine("max_skill =" + countryLeader.maxskill);
                sw.WriteLine("loyalty = " + countryLeader.loyalty);
                sw.WriteLine("picture = " + countryLeader.idPicture);

                for (count2 = 0; count2 <= countryLeader.traits.Count - 1; count2++)
                {
                    sw.WriteLine("add_trait = " + countryLeader.traits[count2]);
                }

                sw.WriteLine("history = {");
                sw.WriteLine("1914.1.1 = { rank = " + countryLeader.rank + " }");
                sw.WriteLine("}");
                sw.WriteLine("}");

            }

            sw.Flush();
            sw.Close();




        }

        public void writeMinisters(country writingCountry)
        {


            //we read the dummy files and copy all the names of the units and that stuff.
            string ruta;

            bool encontrado;
            encontrado = false;

            String lineaArch;
            lineaArch = "";

            int count = 0;

            minister writingMinister;


            if (form.checkBox2.Checked)
            {
                ruta = "/countriesNewNations.txt";
            }
            else
            {
                ruta = "/Countries.txt";
            }


            FileStream file1 = new FileStream(Application.StartupPath + ruta, FileMode.Open, FileAccess.Read);
            StreamReader sw1 = new StreamReader(file1);

            int conta5;

            conta5 = 0;

            //we look for the name of the dummy file in the .txt
            while (!encontrado && sw1.EndOfStream == false)
            {
                lineaArch = sw1.ReadLine();
                if (lineaArch.Substring(0, 3) == writingCountry.tag)
                {
                    lineaArch = lineaArch.Substring(lineaArch.IndexOf("\"") + 1);
                    lineaArch = lineaArch.Substring(0, lineaArch.Length - 1);
                    encontrado = true;

                }


            }

            sw1.Close();

            file1 = new FileStream(Application.StartupPath + "/" + lineaArch, FileMode.Open, FileAccess.Read);
            sw1 = new StreamReader(file1, Encoding.GetEncoding(1252));


            FileStream file = new FileStream(Application.StartupPath + "/common/countries/" + writingCountry.tag + ".txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file, Encoding.GetEncoding(1252));

            lineaArch = sw1.ReadLine();
            //write the units names, but stop when reach the minister of the dummy file, so we can write ours.
            while (!(lineaArch.Contains("ministers")))
            {

                sw.WriteLine(lineaArch);
                lineaArch = sw1.ReadLine();

                if (conta5 == 2)
                {
                    if (writingCountry.faction != "neutral")
                    {
                        sw.WriteLine("major = yes");
                        sw.WriteLine();
                    }

                }

                conta5++;
            }

            sw1.Close();

            //now we can write the ministers

            sw.WriteLine("ministers = {");

            for (count = 0; count <= writingCountry.countryMinisters.Count - 1; count++)
            {
                writingMinister = (minister)writingCountry.countryMinisters[count];

                sw.WriteLine(writingMinister.id + " = {");
                sw.WriteLine("name=\"" + writingMinister.name + "\"");
                sw.WriteLine("ideology = " + writingMinister.ideology);
                sw.WriteLine("loyalty = " + writingMinister.loyalty);
                sw.WriteLine("picture = " + writingMinister.idPicture);
                sw.WriteLine(writingMinister.type1);
                sw.WriteLine(writingMinister.type2);
                sw.WriteLine("start_date = " + writingMinister.startDate);
                sw.WriteLine("}");
                sw.WriteLine();
            }




            sw.WriteLine("}");
            sw.Flush();
            sw.Close();



        }

        public void writeUnits(country writingCountry)
        {
            FileStream file = new FileStream(Application.StartupPath + "/history/units/" + writingCountry.tag + ".txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);

            int count = 0;



            //int numberOfDivision; //brigades / 3 - no use
            int numBrig = 0;

            //numberOfDivision = Math.Floor(writingCountry.numberOfBrigades/3);

            string model;
            province prov;


            for (count = 0; count <= writingCountry.countryArmy.Count - 1; count++)
            {
                Application.DoEvents();


                if (numBrig == 0)
                {
                    sw.WriteLine("division= {");
                    sw.WriteLine("name=\"" + form.unitID + "- Start unit\"");
                    sw.WriteLine("is_reserve = no");
                    if (writingCountry.importantProvinces.Count == 0)
                    {
                        sw.WriteLine("location = " + writingCountry.capitalID);
                    }
                    else
                    {
                        if (form.radioButtonMapOri.Checked)
                        {
                            prov = (province)form.provincesGenerated[Convert.ToInt32(writingCountry.importantProvinces[form.ran.Next(0, writingCountry.importantProvinces.Count)])];
                        }
                        else
                        {
                            prov = (province)writingCountry.importantProvinces[form.ran.Next(0, writingCountry.importantProvinces.Count)];
                        }

                        sw.WriteLine("location = " + prov.id);
                    }

                    form.unitID++;
                }


                if (writingCountry.countryArmy[count].ToString() == "light_armor_brigade" || writingCountry.countryArmy[count].ToString() == "armor_brigade")
                {

                    if (writingCountry.techFocus == 0) //tank focus
                    {
                        model = "1";
                    }
                    else
                    {
                        model = "0";
                    }


                }
                else
                {
                    if (writingCountry.techFocus == 1) //inf focus
                    {
                        model = "1";
                    }
                    else
                    {
                        model = "0";
                    }

                }
                sw.WriteLine("regiment = { type = " + writingCountry.countryArmy[count] + " historical_model = " + model + " }");

                numBrig++;

                if (numBrig == 3 && (writingCountry.countryArmy[count].ToString() == "marine_brigade" || writingCountry.countryArmy[count].ToString() == "paratrooper_brigade" || writingCountry.countryArmy[count].ToString() == "bergsjaeger_brigade" || writingCountry.countryArmy[count].ToString() == "motorized_brigade"))
                {
                    sw.WriteLine();
                    sw.WriteLine("}");
                    sw.WriteLine();
                    numBrig = 0;
                }
                else if (numBrig == 4)
                {
                    sw.WriteLine();
                    sw.WriteLine("}");
                    sw.WriteLine();
                    numBrig = 0;
                }


            }
            //we close the division if there are not more brigades
           // if (numBrig == 1 || numBrig == 2 || (numBrig == 3 && (writingCountry.countryArmy[count] != "marine_brigade" && writingCountry.countryArmy[count] != "paratrooper_brigade" && writingCountry.countryArmy[count] != "bergsjaeger_brigade")))
            if (numBrig > 0)
            {
                sw.WriteLine("}");
                numBrig = 0;
                form.unitID++;

            }
            sw.WriteLine();


            //air


            string airBase;


            if (writingCountry.techFocus == 2) //air focus
            {
                model = "1";
            }
            else
            {
                model = "0";
            }
            numBrig = 0;
            for (count = 0; count <= writingCountry.countryAir.Count - 1; count++)
            {
                if (numBrig == 2)
                {
                    numBrig = 0;


                }

                if (numBrig == 0)
                {

                    airBase = writingCountry.provincesAir[form.ran.Next(0, writingCountry.provincesAir.Count)].ToString();
                    sw.WriteLine("air = {");
                    sw.WriteLine("name = \"Air StartForce " + form.unitID + "\"");
                    sw.WriteLine("base = " + airBase);
                    sw.WriteLine("location = " + airBase);
                    form.unitID++;
                }


                sw.WriteLine("wing = { type = " + writingCountry.countryAir[count] + " name = \"Air Wing Start -" + (numBrig + 1).ToString() + " - " + form.unitID + "\" historical_model = " + model + " }");


                numBrig++;

                if (numBrig == 2)
                {
                    sw.WriteLine("}");
                    sw.WriteLine();
                }

            }

            //we close the division if there are not more brigades
            if (numBrig == 1)
            {
                sw.WriteLine();
                sw.WriteLine("}");
                numBrig = 0;
                form.unitID++;

            }
            sw.WriteLine();


            //naval


            if (writingCountry.techFocus == 3) //naval focus
            {
                model = "1";
            }
            else
            {
                model = "0";
            }

            string navalBase;

            if (writingCountry.hasNavalBases && writingCountry.countryNaval.Count > 0)
            {

                navalBase = writingCountry.provincesNaval[form.ran.Next(0, writingCountry.provincesNaval.Count)].ToString();
                sw.WriteLine("navy  = {");
                sw.WriteLine("name = \"Naval StartForce " + form.unitID + "\"");
                sw.WriteLine("base = " + navalBase);
                sw.WriteLine("location = " + navalBase);
                form.unitID++;



                for (count = 0; count <= writingCountry.countryNaval.Count - 1; count++)
                {
                    sw.WriteLine("ship  = { type = " + writingCountry.countryNaval[count] + " name = \"Start Ship -" + (numBrig + 1).ToString() + " - " + form.unitID + "\" historical_model = " + model + " }");

                    numBrig++;
                }

                sw.WriteLine("}");
                sw.WriteLine();

            }
            else
            { //production screen at 99%
                for (count = 0; count <= writingCountry.countryNaval.Count - 1; count++)
                {
                    sw.WriteLine("military_construction = {");
                    sw.WriteLine("country = " + writingCountry.tag);
                    sw.WriteLine(writingCountry.countryNaval[count].ToString() + " = {");
                    sw.WriteLine("name = \"Start Ship " + form.unitID + "\"");
                    sw.WriteLine("historical_model = " + model);
                    sw.WriteLine("}");
                    sw.WriteLine("cost = 1.00");
                    sw.WriteLine("progress=1.000");
                    sw.WriteLine("duration = 1");
                    sw.WriteLine("}");
                    sw.WriteLine();
                    form.unitID++;
                }

            }

            sw.Flush();
            sw.Close();


        }


        public void writeScenBookMark()
        {
            FileStream file;
            StreamWriter sw;

            //write scen bookmark

            FileStream file1 = new FileStream(Application.StartupPath + "/common/bookmarks.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw1 = new StreamWriter(file1);

            sw1.WriteLine("bookmark =");
            sw1.WriteLine("{");
            sw1.WriteLine("icon = \"GFX_bg_startdate_1936\"");
            sw1.WriteLine("name = \"Random Hoi\"");


            sw1.WriteLine("desc = \"RANDOM_HOI_DESC\"");
            sw1.WriteLine("date = 1936.1.1");

            ArrayList bookCountry = new ArrayList();

            if (form.alliesCountries.Count > 0)
            {
                sw1.WriteLine("country = " + form.aTag);
                bookCountry.Add(form.aTag);
            }

            if (form.axisCountries.Count > 0)
            {
                sw1.WriteLine("country = " + form.axTag);
                bookCountry.Add(form.axTag);
            }

            if (form.conCountries.Count > 0)
            {
                sw1.WriteLine("country = " + form.cTag);
                bookCountry.Add(form.cTag);
            }



            sw1.WriteLine("}");

            sw1.Flush();
            sw1.Close();


            //write localization file
            file = new FileStream(Application.StartupPath + "/localisation/randomhoi.csv", FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(file);

            string cad;
            cad = "RANDOM_HOI_DESC;A random scenario generated by SuiciSpai's random generator mod. Using new countries from NNM and Magrathea. Added some stuff of HPP mod(read forum for more info). Thx to all.;A random scenario generated by SuiciSpai's random generator mod. Using new countries from NNM and Magrathea.Added some stuff of HPP mod(read forum for more info). Thx to all.;A random scenario generated by SuiciSpai's random generator mod. Using new countries from NNM and Magrathea. Added some stuff of HPP mod(read forum for more info). Thx to all.;;A random scenario generated by SuiciSpai's random generator mod. Using new countries from NNM and Magrathea. Thx to all. Added some stuff of HPP mod(read forum for more info).;;;;;;;;;x";
            sw.WriteLine(cad);
            sw.Flush();
            sw.Close();

        }


        public void writeScenBookMarkCGM()
        {
            FileStream file;
            StreamWriter sw;

            //write scen bookmark

            FileStream file1 = new FileStream(Application.StartupPath + "/cgm/bookmarks.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw1 = new StreamWriter(file1);

            sw1.WriteLine("bookmark =");
            sw1.WriteLine("{");

            sw1.WriteLine("is_cgm = yes");
            sw1.WriteLine("cgm_folder = \"RoadToWar\"");

            sw1.WriteLine("icon = \"GFX_bg_startdate_1936\"");
            sw1.WriteLine("name = \"Random Hoi CGM\"");


            sw1.WriteLine("desc = \"RANDOM_HOI_CGM_DESC\"");
            sw1.WriteLine("date = 1936.1.1");



            ArrayList bookCountry = new ArrayList();

            if (form.alliesCountries.Count > 0)
            {
                sw1.WriteLine("country = " + form.aTag);
                bookCountry.Add(form.aTag);
            }

            if (form.axisCountries.Count > 0)
            {
                sw1.WriteLine("country = " + form.axTag);
                bookCountry.Add(form.axTag);
            }

            if (form.conCountries.Count > 0)
            {
                sw1.WriteLine("country = " + form.cTag);
                bookCountry.Add(form.cTag);
            }



            sw1.WriteLine("}");

            sw1.Flush();
            sw1.Close();


            //write localization file
            file = new FileStream(Application.StartupPath + "/localisation/randomhoi.csv", FileMode.Append, FileAccess.Write);
            sw = new StreamWriter(file);

            string cad;
            cad = "RANDOM_HOI_CGM_DESC;CGM - Random scenario generated by SuiciSpai's random generator mod. Using new countries from NNM and Magrathea. Added some stuff of HPP mod(read forum for more info). Thx to all.;CGM Random scenario generated by SuiciSpai's random generator mod. Using new countries from NNM and Magrathea.Added some stuff of HPP mod(read forum for more info). Thx to all.;CGM Random scenario generated by SuiciSpai's random generator mod. Using new countries from NNM and Magrathea. Added some stuff of HPP mod(read forum for more info). Thx to all.;;CGM Random scenario generated by SuiciSpai's random generator mod. Using new countries from NNM and Magrathea. Thx to all. Added some stuff of HPP mod(read forum for more info).;;;;;;;;;x";
            sw.WriteLine(cad);
            sw.Flush();
            sw.Close();

        }

    }
}
