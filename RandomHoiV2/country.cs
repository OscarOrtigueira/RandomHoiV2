using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace RandomHoiV2
{
    class country
    {
        int id;
        public string tag;

        public string capitalID;

        public bool major; //if major will be the leader of the faction
        public string faction = "neutral"; //close to a faction != start in that faction. Only leaders will start in the faction, but its used to elect a leader later.

        public double neutrality;
        public double national_unity;
        public string eliteunit = "";
        public string government;
        public string ideology;
        public int intGovernment;
        public double alignmentX;
        public double alignmentY;

        public int techFocus = 4; /* 0 tanks, 1 inf, 2 air, 3 naval, 4 minimun, 5 ger36, 6 uk36, 7 ger39*/

        public int totalIC = 0;
        public double totalLeadership = 0;
        public double totalManpower = 0;
        public int numberProvinces = 0;
        public int totalPoints = 0;

        public ArrayList importantProvinces = new ArrayList();    //its only use its for capital//units assignemt, province cores is saved in the province. Maybe in a future its more useful anyway. NO CLAIMS
        public ArrayList provincesNotImportant = new ArrayList(); //its only use its for capital//units assignemt, province cores is saved in the province. Maybe in a future its more useful anyway. NO CLAIMS
        public ArrayList provincesAir = new ArrayList(); //air unit placement
        public ArrayList provincesNaval = new ArrayList(); //naval unit placement
        public ArrayList provincesCoastal = new ArrayList();

        public ArrayList provincesALL = new ArrayList(); // new generator
        public ArrayList provinceswithadj = new ArrayList(); // new generator
        public int growRate = 1; // new generator

        public ArrayList countryLeaders = new ArrayList();  //leaders
        public ArrayList countryMinisters = new ArrayList();  //minister

        public ArrayList partyOrganization = new ArrayList();

        public bool isGrowing = true; //new generator

        public bool hasNavalBases = false;
        bool coastal = false;

        public ArrayList countryArmy = new ArrayList(); // land army 
        public ArrayList countryAir = new ArrayList();  //air army
        public ArrayList countryNaval = new ArrayList(); //naval army

      //  public ArrayList countryArmyComp = new ArrayList(); // land army 

        /*No real use.. for now. But maybe will be useful in the future*/
        public int numberOfBrigades = 0;

        public int numberOfFightersUnits = 0;
        public int numberOfBombers = 0;

        public int numberOfScreens = 0;
        public int numberOfCapitalShips = 0;
        public int numberOfSubShips = 0;
        public int numberOfTransShips = 0;


        //public struct composicionArmy
        //{
        //    public string brigada1="";
        //    public string brigada2 = "";
        //    public string brigada3 = "";
        //    public string brigada4 = "";
        //}


        public country(int idCountry, string countryTag)
        {
            id = idCountry;
            tag = countryTag;
        }

        public country(int idCountry, string countryTag, int rate)
        {
            id = idCountry;
            tag = countryTag;
            growRate = rate;
        }


        public void assingProvinces(int firstProvince, int lastProvince, Form1 form, bool useNextProv)
        {
            int count = 0;
            int claims = 0;
            province prov;


            //first make claims!!!

            //claims for provinces before the country
            claims = form.ran.Next(Convert.ToInt32(form.textMinClaims.Text), Convert.ToInt32(form.textMaxClaims.Text));



            if (firstProvince - claims < 0)
            {
                //if the country is at the start of the array
                for (count = 0; count <= firstProvince - 1; count++)
                {
                    prov = (province)form.provincesGenerated[count];
                    prov.addCountryCore(tag);
                }
                //if the country is at the start of the array we need to continue their claims at the end of the array
                for (count = form.provincesGenerated.Count - 1 + (firstProvince - claims); count <= form.provincesGenerated.Count - 1; count++)
                {
                    prov = (province)form.provincesGenerated[count];
                    prov.addCountryCore(tag);
                }

            }
            else
            {

                if (claims > 0)
                {
                    for (count = (firstProvince - claims); count <= firstProvince - 1; count++)
                    {
                        prov = (province)form.provincesGenerated[count];
                        prov.addCountryCore(tag);
                    }
                }

            }




            //claims for provinces after the country
            claims = form.ran.Next(Convert.ToInt32(form.textMinClaims.Text), Convert.ToInt32(form.textMaxClaims.Text));

            if (lastProvince + claims > form.provincesGenerated.Count - 1)
            {
                //if the country is at the end of the array
                for (count = lastProvince; count <= form.provincesGenerated.Count - 1; count++)
                {
                    prov = (province)form.provincesGenerated[count];
                    prov.addCountryCore(tag);
                }
                //if the country is at the end of the array we need to continue their claims at start
                for (count = 0; count <= ((form.provincesGenerated.Count - 1) - lastProvince) + claims; count++)
                {
                    prov = (province)form.provincesGenerated[count];
                    prov.addCountryCore(tag);
                }

            }
            else
            {


                if (claims > 0)
                {

                    for (count = (lastProvince + 1); count <= lastProvince + claims; count++)
                    {
                        prov = (province)form.provincesGenerated[count];
                        prov.addCountryCore(tag);
                    }

                }
            }



            for (count = (firstProvince); count <= lastProvince; count++)
            {
                if (useNextProv)
                {

                    prov = (province)form.provincesGenerated[count];

                    prov.addCountryCore(tag);

                    prov.selectOwner(tag);

                    prov.selectController(tag);

                    totalIC = totalIC + prov.industry;
                    totalLeadership = totalLeadership + prov.leadership;
                    totalManpower = totalManpower + prov.manpower;
                    totalPoints = totalPoints + prov.points;


                    if (prov.navalBase > 0)
                    {
                        hasNavalBases = true;
                        provincesNaval.Add(prov.id);

                    }

                    if (prov.coastal)
                    {
                        coastal = true;
                        provincesCoastal.Add(count);//we dont want the id, we want the position on the array
                    }

                    if (prov.airBase > 0)
                    {
                        provincesAir.Add(prov.id);
                    }




                    if (prov.important)
                    {
                        importantProvinces.Add(count);//we dont want the id, we want the position on the array
                    }
                    else
                    {
                        provincesNotImportant.Add(count);//we dont want the id, we want the position on the array
                    }

                    numberProvinces++;
                }
                else
                {
                    useNextProv = true; // The first province of every coutnry is the last of the last country, so we dont want to collapse the owners. But the first generated country needs to generate his first province!
                }
            }

            //at least 1 port if coastal

            if (coastal && !hasNavalBases)
            {

                prov = (province)form.provincesGenerated[Convert.ToInt32(provincesCoastal[form.ran.Next(0, provincesCoastal.Count)].ToString())];
                prov.navalBase = 4;
                provincesNaval.Add(prov.id);
                hasNavalBases = true;
            }




            //now we are going to assign the capital

            //has the country important provinces?

            if (importantProvinces.Count == 0) //we need to create at least one important province
            {
                if (provincesNotImportant.Count == 1)
                {
                    prov = (province)form.provincesGenerated[Convert.ToInt32(provincesNotImportant[0])];

                }
                else if (provincesNotImportant.Count == 3)
                {
                    prov = (province)form.provincesGenerated[Convert.ToInt32(provincesNotImportant[1])];

                }
                else
                {
                    prov = (province)form.provincesGenerated[Convert.ToInt32(provincesNotImportant[form.ran.Next(1, provincesNotImportant.Count)])]; //random.next will never be the max value

                }

                //one province is going to change! 
                totalIC = totalIC - prov.industry;
                totalLeadership = totalLeadership - prov.leadership;
                totalManpower = totalManpower - prov.manpower;
                totalPoints = totalPoints - prov.points;

                capitalID = prov.id;

                provincesAir.Add(capitalID);
                prov.createResources(form, true);  //now its a ubberprovince
                prov.importantSG = true;
                prov.infra = 10;


                totalIC = totalIC + prov.industry;
                totalLeadership = totalLeadership + prov.leadership;
                totalManpower = totalManpower + prov.manpower;
                totalPoints = totalPoints + prov.points;

                if (prov.navalBase > 0) //capital will have always naval base if coastal, so no need to check if we destroyed some naval bases.
                {
                    provincesNaval.Add(capitalID);
                    hasNavalBases = true;
                }


            }
            else //just select one randomly, but try to stay away from borders (not the first, not the last one.. if possible)
            {
                if (importantProvinces.Count == 1)
                {
                    prov = (province)form.provincesGenerated[Convert.ToInt32(importantProvinces[0])];

                }
                else if (importantProvinces.Count == 3)
                {
                    prov = (province)form.provincesGenerated[Convert.ToInt32(importantProvinces[1])];

                }
                else
                {
                    prov = (province)form.provincesGenerated[Convert.ToInt32(importantProvinces[form.ran.Next(1, importantProvinces.Count)])]; //random.next will never be the max value

                }
                capitalID = prov.id;
                provincesAir.Add(capitalID);
                prov.infra = 10;

            }
            if (totalIC >= Convert.ToInt32(form.textICForRocket.Text))
            {
                prov.rocket = 1;
            }

        }


        public void assingProvincesPuppetable(int firstProvince, int lastProvince, Form1 form, bool littleCountry)
        {

            int count = 0;
            int claims = 0;
            province prov;


            //first make claims!!!

            //claims for provinces before the country
            if (littleCountry)
            {
                claims = form.ran.Next(Convert.ToInt32(Convert.ToInt32(form.textMinClaims.Text) / 8), Convert.ToInt32(Convert.ToInt32(form.textMaxClaims.Text) / 8));
            }
            else
            {
                claims = form.ran.Next(Convert.ToInt32(Convert.ToInt32(form.textMinClaims.Text) / 4), Convert.ToInt32(Convert.ToInt32(form.textMaxClaims.Text) / 4));
            }




            if (firstProvince - claims < 0)
            {
                //if the country is at the start of the array
                for (count = 0; count <= firstProvince - 1; count++)
                {
                    prov = (province)form.provincesGenerated[count];
                    prov.addCountryCore(tag);
                    if (prov.important)
                    {
                        importantProvinces.Add(count);
                    }
                    else
                    {
                        provincesNotImportant.Add(count);
                    }

                }
                //if the country are at the start of the array we need to continue their claims at the end of the array
                for (count = form.provincesGenerated.Count - 1 + (firstProvince - claims); count <= form.provincesGenerated.Count - 1; count++)
                {
                    prov = (province)form.provincesGenerated[count];
                    prov.addCountryCore(tag);
                    if (prov.important)
                    {
                        importantProvinces.Add(count);
                    }
                    else
                    {
                        provincesNotImportant.Add(count);
                    }
                }

            }
            else
            {

                if (claims > 0)
                {
                    for (count = (firstProvince - claims); count <= firstProvince - 1; count++)
                    {
                        prov = (province)form.provincesGenerated[count];
                        prov.addCountryCore(tag);
                        if (prov.important)
                        {
                            importantProvinces.Add(count);
                        }
                        else
                        {
                            provincesNotImportant.Add(count);
                        }
                    }
                }

            }




            //claims for provinces after the country
            if (littleCountry)
            {
                claims = form.ran.Next(Convert.ToInt32(Convert.ToInt32(form.textMinClaims.Text) / 4), Convert.ToInt32(Convert.ToInt32(form.textMaxClaims.Text) / 4));
            }
            else
            {
                claims = form.ran.Next(Convert.ToInt32(form.textMinClaims.Text), Convert.ToInt32(form.textMaxClaims.Text));
            }

            if (lastProvince + claims > form.provincesGenerated.Count - 1)
            {
                //if the country are at the end of the array
                for (count = lastProvince; count <= form.provincesGenerated.Count - 1; count++)
                {
                    prov = (province)form.provincesGenerated[count];
                    prov.addCountryCore(tag);
                    if (prov.important)
                    {
                        importantProvinces.Add(count);
                    }
                    else
                    {
                        provincesNotImportant.Add(count);
                    }
                }
                //if the country are at the end of the array we need to continue their claims at start
                for (count = 0; count <= ((form.provincesGenerated.Count - 1) - lastProvince) + claims; count++)
                {
                    prov = (province)form.provincesGenerated[count];
                    prov.addCountryCore(tag);
                    if (prov.important)
                    {
                        importantProvinces.Add(count);
                    }
                    else
                    {
                        provincesNotImportant.Add(count);
                    }
                }

            }
            else
            {


                if (claims > 0)
                {

                    for (count = (lastProvince + 1); count <= lastProvince + claims; count++)
                    {
                        prov = (province)form.provincesGenerated[count];
                        prov.addCountryCore(tag);
                        if (prov.important)
                        {
                            importantProvinces.Add(count);
                        }
                        else
                        {
                            provincesNotImportant.Add(count);
                        }
                    }

                }
            }



            //now we are going to assign the capital

            //has the country important provinces?

            if (importantProvinces.Count == 0)
            {

                capitalID = provincesNotImportant[form.ran.Next(0, provincesNotImportant.Count)].ToString();


            }
            else //just select one randomly
            {
                capitalID = importantProvinces[form.ran.Next(0, importantProvinces.Count)].ToString();
            }



        }

        public void generatePolitics(Form1 form)
        {
            int aux = 0;




            neutrality = form.ran.Next((Convert.ToInt32(form.MinNe.Text)), (Convert.ToInt32(form.MaxNe.Text) + 1));


            national_unity = form.ran.Next((Convert.ToInt32(form.MinNU.Text)), (Convert.ToInt32(form.MaxNU.Text) + 1));

            aux = form.ran.Next(1, 13); /*there are 12 possible alignaments*//*remember, max random next will never selected. Thats why its 13 and not 12.*/

            #region checkCountriesFaction


            if (form.radioFactAll.Checked)
            /*the first 3 generated countries will be 1 axis,1 allied and 1 con, to be sure all factions will have at least 1 possible faction leaders*/
            {
                if (form.axisCountries.Count == 0 && form.alliesCountries.Count == 0 && form.conCountries.Count == 0)
                {
                    switch (form.ran.Next(0, 3))
                    {
                        case 0:
                            aux = 1;
                            break;
                        case 1:
                            aux = 8;
                            break;
                        case 2:
                            aux = 12;
                            break;

                    }
                }
                else
                {
                    if (form.axisCountries.Count == 0)
                    {
                        aux = 1;

                    }
                    else
                    {

                        if (form.alliesCountries.Count == 0)
                        {
                            aux = 8;

                        }
                        else
                        {
                            if (form.conCountries.Count == 0)
                            {

                                {
                                    aux = 12;
                                }

                            }
                        }

                    }


                }
            }

            if (form.radioFactTwo.Checked)
            {
                if (form.axisCountries.Count == 0 && form.alliesCountries.Count == 0 && form.conCountries.Count == 0)
                {
                    switch (form.ran.Next(0, 3))
                    {
                        case 0:
                            aux = 1;
                            break;
                        case 1:
                            aux = 8;
                            break;
                        case 2:
                            aux = 12;
                            break;

                    }





                }
                else
                {
                    if ((form.axisCountries.Count == 0 && form.alliesCountries.Count == 0) && form.conCountries.Count > 0)
                    {

                        if (form.ran.Next(0, 2) == 0)
                        {
                            aux = 8;
                        }
                        else
                        {
                            aux = 1;
                        }
                    }

                    if ((form.conCountries.Count == 0 && form.alliesCountries.Count == 0) && form.axisCountries.Count > 0)
                    {

                        if (form.ran.Next(0, 2) == 0)
                        {
                            aux = 8;
                        }
                        else
                        {
                            aux = 12;
                        }
                    }

                    if ((form.axisCountries.Count == 0 && form.conCountries.Count == 0) && form.alliesCountries.Count > 0)
                    {

                        if (form.ran.Next(0, 2) == 0)
                        {
                            aux = 12;
                        }
                        else
                        {
                            aux = 1;
                        }
                    }

                }

            }

            #endregion

            int neutralityDemo = 0;

            if (form.raNeDemo1.Checked)
            {
                neutralityDemo = 5;
            }
            if (form.raNeDemo2.Checked)
            {
                neutralityDemo = 10;
            }
            if (form.raNeDemo3.Checked)
            {
                neutralityDemo = 15;
            }
            if (form.raNeDemo4.Checked)
            {
                neutralityDemo = 20;
            }
            if (form.raNeDemo5.Checked)
            {
                neutralityDemo = 25;
            }

            intGovernment = aux;
            switch (aux)
            {
                case 1:
                    alignmentX = form.ran.Next(125, 165);
                    alignmentY = form.ran.Next(125, 165);
                    government = "national_socialism";
                    ideology = "national_socialist";

                    if (form.radioButtonRuler.Checked)
                    {
                        partyOrganization.Add(form.ran.Next(40, 61)); //national_socialist
                        partyOrganization.Add(form.ran.Next(25, 41)); //fascistic
                        partyOrganization.Add(form.ran.Next(20, 31)); //paternal_autocrat
                        partyOrganization.Add(form.ran.Next(15, 36)); //social_conservative
                        partyOrganization.Add(form.ran.Next(0, 6)); //market_liberal
                        partyOrganization.Add(form.ran.Next(0, 6)); //social_liberal
                        partyOrganization.Add(form.ran.Next(10, 16)); //social_democrat
                        partyOrganization.Add(form.ran.Next(0, 6)); //left_wing_radical
                        partyOrganization.Add(form.ran.Next(0, 3)); //leninist
                        partyOrganization.Add(form.ran.Next(0, 3)); //stalinist
                    }


                    if (form.radioFactAll.Checked)
                    {
                        form.axisCountries.Add(tag);
                    }

                    if (form.radioFactTwo.Checked)
                    {
                        if (form.alliesCountries.Count == 0 || form.conCountries.Count == 0)
                        {
                            form.axisCountries.Add(tag);
                            faction = "axis";
                        }

                    }





                    break;
                case 2:
                    alignmentX = form.ran.Next(75, 150);
                    alignmentY = form.ran.Next(75, 150);
                    government = "fascist_republic";
                    ideology = "fascistic";
                    if (form.radioButtonRuler.Checked)
                    {
                        partyOrganization.Add(form.ran.Next(25, 46)); //national_socialist
                        partyOrganization.Add(form.ran.Next(45, 61)); //fascistic
                        partyOrganization.Add(form.ran.Next(30, 41)); //paternal_autocrat
                        partyOrganization.Add(form.ran.Next(20, 35)); //social_conservative
                        partyOrganization.Add(form.ran.Next(0, 11)); //market_liberal
                        partyOrganization.Add(form.ran.Next(0, 11)); //social_liberal
                        partyOrganization.Add(form.ran.Next(0, 11)); //social_democrat
                        partyOrganization.Add(form.ran.Next(0, 11)); //left_wing_radical
                        partyOrganization.Add(form.ran.Next(0, 6)); //leninist
                        partyOrganization.Add(form.ran.Next(0, 6)); //stalinist
                    }
                    break;
                case 3:
                    alignmentX = form.ran.Next(0, 25);
                    alignmentY = form.ran.Next(0, 25);
                    government = "right_wing_republic";
                    ideology = "social_conservative";
                    if (form.radioButtonRuler.Checked)
                    {
                        partyOrganization.Add(form.ran.Next(10, 21)); //national_socialist
                        partyOrganization.Add(form.ran.Next(35, 45)); //fascistic
                        partyOrganization.Add(form.ran.Next(25, 41)); //paternal_autocrat
                        partyOrganization.Add(form.ran.Next(30, 40)); //social_conservative
                        partyOrganization.Add(form.ran.Next(5, 16)); //market_liberal
                        partyOrganization.Add(form.ran.Next(0, 11)); //social_liberal
                        partyOrganization.Add(form.ran.Next(5, 21)); //social_democrat
                        partyOrganization.Add(form.ran.Next(5, 11)); //left_wing_radical
                        partyOrganization.Add(form.ran.Next(5, 11)); //leninist
                        partyOrganization.Add(form.ran.Next(5, 11)); //stalinist
                    }
                    break;
                case 4:
                    alignmentX = form.ran.Next(50, 100);
                    alignmentY = form.ran.Next(50, 125);
                    government = "right_wing_autocrat";
                    ideology = "paternal_autocrat";
                    if (form.radioButtonRuler.Checked)
                    {
                        partyOrganization.Add(form.ran.Next(5, 16)); //national_socialist
                        partyOrganization.Add(form.ran.Next(5, 21)); //fascistic
                        partyOrganization.Add(form.ran.Next(45, 61)); //paternal_autocrat
                        partyOrganization.Add(form.ran.Next(20, 41)); //social_conservative
                        partyOrganization.Add(form.ran.Next(5, 11)); //market_liberal
                        partyOrganization.Add(form.ran.Next(5, 11)); //social_liberal
                        partyOrganization.Add(form.ran.Next(10, 16)); //social_democrat
                        partyOrganization.Add(form.ran.Next(0, 6)); //left_wing_radical
                        partyOrganization.Add(form.ran.Next(0, 6)); //leninist
                        partyOrganization.Add(form.ran.Next(0, 6)); //stalinist
                    }


                    break;

                case 5:
                    alignmentX = form.ran.Next(25, 50);
                    alignmentY = form.ran.Next(0, 50);
                    government = "absolute_monarchy";
                    ideology = "paternal_autocrat";
                    if (form.radioButtonRuler.Checked)
                    {
                        partyOrganization.Add(form.ran.Next(10, 16)); //national_socialist
                        partyOrganization.Add(form.ran.Next(5, 16)); //fascistic
                        partyOrganization.Add(form.ran.Next(60, 81)); //paternal_autocrat
                        partyOrganization.Add(form.ran.Next(30, 51)); //social_conservative
                        partyOrganization.Add(form.ran.Next(5, 11)); //market_liberal
                        partyOrganization.Add(form.ran.Next(5, 11)); //social_liberal
                        partyOrganization.Add(form.ran.Next(10, 21)); //social_democrat
                        partyOrganization.Add(form.ran.Next(0, 11)); //left_wing_radical
                        partyOrganization.Add(form.ran.Next(0, 6)); //leninist
                        partyOrganization.Add(form.ran.Next(0, 6)); //stalinist
                    }
                    break;
                case 6:
                    alignmentX = form.ran.Next(-25, 25);
                    alignmentY = form.ran.Next(-75, 25);
                    government = "social_conservatism";
                    ideology = "social_conservative";
                    if (form.radioButtonRuler.Checked)
                    {
                        partyOrganization.Add(form.ran.Next(10, 16)); //national_socialist
                        partyOrganization.Add(form.ran.Next(10, 16)); //fascistic
                        partyOrganization.Add(form.ran.Next(20, 31)); //paternal_autocrat
                        partyOrganization.Add(form.ran.Next(40, 56)); //social_conservative
                        partyOrganization.Add(form.ran.Next(15, 31)); //market_liberal
                        partyOrganization.Add(form.ran.Next(10, 26)); //social_liberal
                        partyOrganization.Add(form.ran.Next(20, 31)); //social_democrat
                        partyOrganization.Add(form.ran.Next(15, 26)); //left_wing_radical
                        partyOrganization.Add(form.ran.Next(10, 16)); //leninist
                        partyOrganization.Add(form.ran.Next(5, 11)); //stalinist
                    }
                    neutrality = neutrality + neutralityDemo;
                    break;
                case 7:
                    alignmentX = form.ran.Next(-50, 50);
                    alignmentY = form.ran.Next(-100, -50);
                    government = "market_liberalism";
                    ideology = "market_liberal";
                    if (form.radioButtonRuler.Checked)
                    {
                        partyOrganization.Add(form.ran.Next(0, 6)); //national_socialist
                        partyOrganization.Add(form.ran.Next(0, 16)); //fascistic
                        partyOrganization.Add(form.ran.Next(10, 21)); //paternal_autocrat
                        partyOrganization.Add(form.ran.Next(15, 26)); //social_conservative
                        partyOrganization.Add(form.ran.Next(40, 56)); //market_liberal
                        partyOrganization.Add(form.ran.Next(25, 41)); //social_liberal
                        partyOrganization.Add(form.ran.Next(20, 36)); //social_democrat
                        partyOrganization.Add(form.ran.Next(15, 21)); //left_wing_radical
                        partyOrganization.Add(form.ran.Next(10, 16)); //leninist
                        partyOrganization.Add(form.ran.Next(5, 11)); //stalinist
                    }
                    neutrality = neutrality + neutralityDemo;
                    break;

                case 8:
                    alignmentX = form.ran.Next(-50, 50);
                    alignmentY = form.ran.Next(-50, 0);
                    government = "social_democracy";
                    ideology = "social_democrat";

                    if (form.radioFactAll.Checked)
                    {
                        form.alliesCountries.Add(tag);
                    }


                    if (form.radioFactTwo.Checked)
                    {
                        if (form.conCountries.Count == 0 || form.axisCountries.Count == 0)
                        {
                            form.alliesCountries.Add(tag);
                            faction = "allies";
                        }

                    }
                    if (form.radioButtonRuler.Checked)
                    {
                        partyOrganization.Add(form.ran.Next(0, 6)); //national_socialist
                        partyOrganization.Add(form.ran.Next(0, 6)); //fascistic
                        partyOrganization.Add(form.ran.Next(5, 16)); //paternal_autocrat
                        partyOrganization.Add(form.ran.Next(15, 36)); //social_conservative
                        partyOrganization.Add(form.ran.Next(20, 41)); //market_liberal
                        partyOrganization.Add(form.ran.Next(20, 36)); //social_liberal
                        partyOrganization.Add(form.ran.Next(40, 61)); //social_democrat
                        partyOrganization.Add(form.ran.Next(15, 26)); //left_wing_radical
                        partyOrganization.Add(form.ran.Next(10, 16)); //leninist
                        partyOrganization.Add(form.ran.Next(5, 11)); //stalinist
                    }
                    neutrality = neutrality + neutralityDemo;
                    break;
                case 9:
                    alignmentX = form.ran.Next(-75, 25);
                    alignmentY = form.ran.Next(0, 75);
                    government = "social_liberalism";
                    ideology = "social_liberal";
                    if (form.radioButtonRuler.Checked)
                    {
                        partyOrganization.Add(form.ran.Next(0, 6)); //national_socialist
                        partyOrganization.Add(form.ran.Next(0, 11)); //fascistic
                        partyOrganization.Add(form.ran.Next(10, 16)); //paternal_autocrat
                        partyOrganization.Add(form.ran.Next(15, 26)); //social_conservative
                        partyOrganization.Add(form.ran.Next(20, 36)); //market_liberal
                        partyOrganization.Add(form.ran.Next(40, 56)); //social_liberal
                        partyOrganization.Add(form.ran.Next(25, 36)); //social_democrat
                        partyOrganization.Add(form.ran.Next(20, 31)); //left_wing_radical
                        partyOrganization.Add(form.ran.Next(15, 26)); //leninist
                        partyOrganization.Add(form.ran.Next(10, 21)); //stalinist
                    }
                    neutrality = neutrality + neutralityDemo;
                    break;

                case 10:
                    alignmentX = form.ran.Next(-100, -25);
                    alignmentY = form.ran.Next(50, 100);
                    government = "left_wing_radicals";
                    ideology = "left_wing_radical";
                    if (form.radioButtonRuler.Checked)
                    {
                        partyOrganization.Add(form.ran.Next(0, 6)); //national_socialist
                        partyOrganization.Add(form.ran.Next(0, 11)); //fascistic
                        partyOrganization.Add(form.ran.Next(5, 11)); //paternal_autocrat
                        partyOrganization.Add(form.ran.Next(10, 16)); //social_conservative
                        partyOrganization.Add(form.ran.Next(15, 21)); //market_liberal
                        partyOrganization.Add(form.ran.Next(20, 31)); //social_liberal
                        partyOrganization.Add(form.ran.Next(15, 26)); //social_democrat
                        partyOrganization.Add(form.ran.Next(40, 56)); //left_wing_radical
                        partyOrganization.Add(form.ran.Next(30, 46)); //leninist
                        partyOrganization.Add(form.ran.Next(30, 41)); //stalinist
                    }
                    break;

                case 11:
                    alignmentX = form.ran.Next(-125, -75);
                    alignmentY = form.ran.Next(75, 125);
                    government = "socialist_republic";
                    ideology = "leninist";
                    if (form.radioButtonRuler.Checked)
                    {
                        partyOrganization.Add(form.ran.Next(0, 6)); //national_socialist
                        partyOrganization.Add(form.ran.Next(0, 6)); //fascistic
                        partyOrganization.Add(form.ran.Next(0, 11)); //paternal_autocrat
                        partyOrganization.Add(form.ran.Next(5, 11)); //social_conservative
                        partyOrganization.Add(form.ran.Next(5, 11)); //market_liberal
                        partyOrganization.Add(form.ran.Next(10, 21)); //social_liberal
                        partyOrganization.Add(form.ran.Next(5, 16)); //social_democrat
                        partyOrganization.Add(form.ran.Next(20, 36)); //left_wing_radical
                        partyOrganization.Add(form.ran.Next(40, 61)); //leninist
                        partyOrganization.Add(form.ran.Next(30, 41)); //stalinist
                    }
                    break;
                case 12:
                    alignmentX = form.ran.Next(-165, -125);
                    alignmentY = form.ran.Next(125, 165);
                    government = "federal_socialist_republic";
                    ideology = "stalinist";


                    if (form.radioFactAll.Checked)
                    {

                        form.conCountries.Add(tag);
                    }

                    if (form.radioFactTwo.Checked)
                    {
                        if (form.alliesCountries.Count == 0 || form.axisCountries.Count == 0)
                        {
                            form.conCountries.Add(tag);
                            faction = "con";
                        }

                    }
                    if (form.radioButtonRuler.Checked)
                    {
                        partyOrganization.Add(form.ran.Next(0, 3)); //national_socialist
                        partyOrganization.Add(form.ran.Next(0, 3)); //fascistic
                        partyOrganization.Add(form.ran.Next(0, 6)); //paternal_autocrat
                        partyOrganization.Add(form.ran.Next(0, 11)); //social_conservative
                        partyOrganization.Add(form.ran.Next(0, 6)); //market_liberal
                        partyOrganization.Add(form.ran.Next(10, 16)); //social_liberal
                        partyOrganization.Add(form.ran.Next(0, 6)); //social_democrat
                        partyOrganization.Add(form.ran.Next(21, 31)); //left_wing_radical
                        partyOrganization.Add(form.ran.Next(30, 46)); //leninist
                        partyOrganization.Add(form.ran.Next(50, 71)); //stalinist
                    }
                    break;

            }

            if (form.radioButtonBalancedParty.Checked)
            {
                partyOrganization.Add(form.ran.Next(30, 41)); //national_socialist
                partyOrganization.Add(form.ran.Next(30, 41)); //fascistic
                partyOrganization.Add(form.ran.Next(30, 41)); //paternal_autocrat
                partyOrganization.Add(form.ran.Next(30, 41)); //social_conservative
                partyOrganization.Add(form.ran.Next(30, 41)); //market_liberal
                partyOrganization.Add(form.ran.Next(30, 41)); //social_liberal
                partyOrganization.Add(form.ran.Next(30, 41));//social_democrat
                partyOrganization.Add(form.ran.Next(30, 41)); //left_wing_radical
                partyOrganization.Add(form.ran.Next(30, 41)); //leninist
                partyOrganization.Add(form.ran.Next(30, 41)); //stalinist
            }
            else
            {
                partyOrganization.Add(form.ran.Next(5, 61)); //national_socialist
                partyOrganization.Add(form.ran.Next(5, 61));  //fascistic
                partyOrganization.Add(form.ran.Next(5, 61));  //paternal_autocrat
                partyOrganization.Add(form.ran.Next(5, 61));  //social_conservative
                partyOrganization.Add(form.ran.Next(5, 61));  //market_liberal
                partyOrganization.Add(form.ran.Next(5, 61));  //social_liberal
                partyOrganization.Add(form.ran.Next(5, 61)); //social_democrat
                partyOrganization.Add(form.ran.Next(5, 61));  //left_wing_radical
                partyOrganization.Add(form.ran.Next(5, 61));  //leninist
                partyOrganization.Add(form.ran.Next(5, 61));  //stalinist
            }

        }

        public void generateTechs(Form1 form)
        {



            if (form.radioMinimun.Checked)
            {
                techFocus = 4;
            }

            //if (form.radioGermany.Checked)
            //{
            //    techFocus = 5;
            //}

            //if (form.radioUK.Checked)
            //{
            //    techFocus = 6;
            //}

            //if (form.radioGer39.Checked)
            //{
            //    techFocus = 7;
            //}


            if (form.radioMinimun.Checked && form.checkTechFocus.Checked)
            {
                if (!hasNavalBases)
                {
                    techFocus = form.ran.Next(0, 3);
                }
                else
                {
                    if (form.provincePacific.Contains(capitalID))
                    {
                        techFocus = 3;
                    }
                    else
                    {
                        techFocus = form.ran.Next(0, 4);
                    }

                }


            }

        }

        public void createLeaders(Form1 form)
        {
            int conta;
            leader newLeader;

            for (conta = 0; conta <= Convert.ToInt32(form.textBox1.Text.ToString()); conta++)
            {
                newLeader = new leader(form.leadersID);

                newLeader.generateStats(form, "land", Convert.ToInt32(form.textMxSLL.Text));

                countryLeaders.Add(newLeader);
                form.leadersID++;
            }

            for (conta = 0; conta <= Convert.ToInt32(form.textBox2.Text.ToString()); conta++)
            {
                newLeader = new leader(form.leadersID);

                newLeader.generateStats(form, "air", Convert.ToInt32(form.textMxSAL.Text));

                countryLeaders.Add(newLeader);
                form.leadersID++;
            }

            for (conta = 0; conta <= Convert.ToInt32(form.textBox3.Text.ToString()); conta++)
            {
                newLeader = new leader(form.leadersID);

                newLeader.generateStats(form, "sea", Convert.ToInt32(form.textMxSNL.Text));

                countryLeaders.Add(newLeader);
                form.leadersID++;
            }



        }

        public void createMinisters(Form1 form)
        {

            int conta;
            minister newMinister;

            for (conta = 0; conta <= Convert.ToInt32(form.textMinisters.Text.ToString()); conta++)
            {
                newMinister = new minister(form.ministersID);

                newMinister.generateStats(form);

                countryMinisters.Add(newMinister);
                form.ministersID++;

            }


        }


        public void createStartingArmy(Form1 form)
        {


            int conta = 0;
            int aux;
            int createNumber;
            bool firstShip = true;

            int max = 0; //addition to min and max of land brigades if hasnt got any navalbases and the country dont want ships

            int maxA = 0; //addition to min and max of air divisions if hasnt got any navalbases and the country dont want ships

            //we are starting with naval

            //has naval base?

            if (hasNavalBases || form.comboBox1.SelectedIndex == 0)
            {
                //minimun
                if (form.radioSUMN.Checked)
                {
                    countryNaval.Add("heavy_cruiser");
                    numberOfCapitalShips++;
                    countryNaval.Add("destroyer");
                    numberOfScreens++;
                    countryNaval.Add("transport_ship");
                    numberOfTransShips++;
                }
                else if (form.radioSUCN.Checked || form.radioSUPLN.Checked)
                {
                    if (form.radioSUCN.Checked)
                    {
                        createNumber = form.ran.Next(Convert.ToInt32(form.textSUMN.Text), Convert.ToInt32(form.textSUMxN.Text) + 1);

                    }
                    else
                    {

                        //every brigade is 2.5 ic worth
                        int rdivisor = Convert.ToInt32(Math.Round(Convert.ToDouble(totalIC / 8)));
                        if (rdivisor < 3)
                        {
                            rdivisor = 3; //at least 3 navy units
                        }
                        createNumber = Convert.ToInt32(rdivisor + max);
                    }

                    //custom
                    for (conta = 0; conta <= createNumber - 1; conta++)
                    {

                        aux = form.ran.Next(0, 14);

                        //first ship will be always transport ship
                        if (firstShip)
                        {
                            aux = 0;
                            firstShip = false;
                        }



                        switch (aux)
                        {
                            case 0:
                            case 9:
                                countryNaval.Add("transport_ship");
                                numberOfTransShips++;
                                break;
                            case 1:
                            case 10:
                                countryNaval.Add("destroyer");
                                numberOfScreens++;
                                break;
                            case 2:
                            case 11:
                                countryNaval.Add("light_cruiser");
                                numberOfScreens++;
                                break;
                            case 3:
                            case 12:
                                countryNaval.Add("submarine");
                                numberOfSubShips++;
                                break;
                            case 4:
                            case 13:
                                countryNaval.Add("heavy_cruiser");
                                numberOfCapitalShips++;
                                break;
                            case 5:
                                countryNaval.Add("battlecruiser");
                                numberOfCapitalShips++;
                                break;
                            case 6:
                                countryNaval.Add("battleship");
                                numberOfCapitalShips++;
                                break;
                            case 7:
                                //remember, techfocus its also the minimun, ger,ger39 and uk techs
                                if (techFocus == 0 || techFocus == 1)
                                {
                                    countryNaval.Add("battlecruiser");
                                    numberOfCapitalShips++;
                                }
                                else
                                {
                                    countryNaval.Add("escort_carrier");
                                    numberOfCapitalShips++;
                                }
                                break;
                            case 8:
                                //remember, techfocus its also the minimun, ger,ger39 and uk techs
                                if (techFocus == 0 || techFocus == 1 || techFocus == 2)
                                {
                                    countryNaval.Add("battleship");
                                    numberOfCapitalShips++;
                                }
                                else
                                {
                                    countryNaval.Add("carrier");
                                    numberOfCapitalShips++;
                                }

                                break;
                        }

                    }

                }

            }
            else
            {


                if (form.comboBox1.SelectedIndex == 1)
                {
                    /*we ll make more air*/
                    if (form.radioSUMN.Checked)
                    {

                        maxA = 3;

                    }

                    if (form.radioSUPLN.Checked)
                    {
                        maxA = Convert.ToInt32(Math.Round(Convert.ToDouble(totalIC / 8)));
                        if (maxA < 3)
                        {
                            maxA = 3;
                        }
                    }
                    if (form.radioSUCN.Checked)
                    {
                        maxA = form.ran.Next(Convert.ToInt32(form.textSUMN.Text), Convert.ToInt32(form.textSUMxN.Text) + 1);

                    }
                }

                if (form.comboBox1.SelectedIndex == 2)
                {
                    /*we ll make more land brigades*/
                    if (form.radioSUMN.Checked)
                    {

                        max = 9;

                    }


                    if (form.radioSUCN.Checked)
                    {
                        max = form.ran.Next((Convert.ToInt32(form.textSUMN.Text) * 3), (Convert.ToInt32(form.textSUMxN.Text) * 3) + 1);

                    }

                    if (form.radioSUPLN.Checked)
                    {
                        max = Convert.ToInt32(Math.Round(Convert.ToDouble(totalIC / 8)));
                        if (max < 3)
                        {
                            max = 3;
                        }
                    }

                }



            }



            //now the airforce


            if (form.radioSUMA.Checked)
            {

                countryAir.Add("cag");
                numberOfFightersUnits++;
                countryAir.Add("interceptor");
                numberOfFightersUnits++;
                countryAir.Add("interceptor");
                numberOfFightersUnits++;
                countryAir.Add("tactical_bomber");
                numberOfBombers++;

                if (maxA == 3)
                {
                    countryAir.Add("interceptor");
                    numberOfFightersUnits++;
                    countryAir.Add("interceptor");
                    numberOfFightersUnits++;
                    countryAir.Add("tactical_bomber");
                }
            }

            if (form.radioSUCA.Checked || form.radioSUPLA.Checked)
            {
                if (form.radioSUCA.Checked)
                {
                    createNumber = form.ran.Next(Convert.ToInt32(form.textSUMA.Text) + maxA, Convert.ToInt32(form.textSUMxA.Text) + maxA + 1);

                }
                else
                {
                    //every brigade is 2.5 ic worth
                    int rdivisor = Convert.ToInt32(Math.Round(Convert.ToDouble(totalIC / 8)));
                    if (rdivisor < 3)
                    {
                        rdivisor = 3; //at least 2 air units
                    }
                    createNumber = Convert.ToInt32(rdivisor + maxA);
                }



                for (conta = 0; conta <= createNumber - 1; conta++)
                {

                    switch (form.ran.Next(0, 8))
                    {
                        case 0:
                        case 1:
                        case 4:
                        case 7:
                            //only air techFocus can start with cas or strategic bombers.
                            if (techFocus == 2 && form.ran2.Next(0, 4) < 2)
                            {
                                switch (form.ran2.Next(0, 2))
                                {
                                    case 0:
                                        countryAir.Add("cas");
                                        numberOfBombers++;
                                        break;
                                    case 1:
                                        countryAir.Add("strategic_bomber");
                                        numberOfBombers++;
                                        break;

                                }

                            }
                            else
                            {
                                countryAir.Add("tactical_bomber");
                                numberOfBombers++;
                            }

                            break;
                        case 2:
                        case 3:
                        case 5:
                            countryAir.Add("interceptor");
                            numberOfFightersUnits++;
                            break;
                        case 6:
                            //only naval and air techFocus can start with cags.
                            if (techFocus == 0 || techFocus == 1)
                            {
                                countryAir.Add("interceptor");
                                numberOfFightersUnits++;

                            }
                            else
                            {
                                countryAir.Add("cag");
                                numberOfFightersUnits++;

                            }
                            break;
                    }


                }



            }


            //finally, the land forces

            //minimal, 5 inf, 1 tnk, 3 support
            //if (form.radioSUML.Checked)
            //{
            //    for (conta = 0; conta <= 5; conta++)
            //    {
            //        countryArmy.Add("infantry_brigade");
            //        numberOfBrigades++;

            //    }

            //    countryArmy.Add("light_armor_brigade");
            //    numberOfBrigades++;

            //    if (form.checkNOBR.Checked)
            //    {
            //        for (conta = 0; conta <= 2; conta++)
            //        {
            //            countryArmy.Add("infantry_brigade");
            //            numberOfBrigades++;

            //        }
            //    }
            //    else
            //    {
            //        countryArmy.Add("anti_air_brigade");
            //        numberOfBrigades++;

            //        countryArmy.Add("artillery_brigade");
            //        numberOfBrigades++;

            //        countryArmy.Add("anti_tank_brigade");
            //        numberOfBrigades++;
            //    }


            //    if (max == 9)
            //    {
            //        for (conta = 0; conta <= 5; conta++)
            //        {
            //            countryArmy.Add("infantry_brigade");
            //            numberOfBrigades++;

            //        }

            //        if (form.checkNOBR.Checked)
            //        {
            //            for (conta = 0; conta <= 2; conta++)
            //            {
            //                countryArmy.Add("infantry_brigade");
            //                numberOfBrigades++;

            //            }
            //        }
            //        else
            //        {
            //            countryArmy.Add("anti_air_brigade");
            //            numberOfBrigades++;

            //            countryArmy.Add("artillery_brigade");
            //            numberOfBrigades++;

            //            countryArmy.Add("anti_tank_brigade");
            //            numberOfBrigades++;
            //        }
            //    }

            //}

            //custom or IC based




            if (form.radioSUCL.Checked || form.radioSUPL.Checked)
            {
                if (form.radioSUCL.Checked)
                {
                    createNumber = form.ran.Next(Convert.ToInt32(form.textSUML.Text) + max, Convert.ToInt32(form.textSUMxL.Text) + max + 1);

                }
                else
                {
                    //every brigade is 2.5 ic worth

                    createNumber = Convert.ToInt32(Math.Round(totalIC / 2.5) + max);
                }


                int tocaBrigada = 0;
                string tipoBrigada = "";
                //countryArmyComp
               // composicionArmy comp = new composicionArmy();
                for (conta = 0; conta <= createNumber - 1; conta++)
                {
                    if (tocaBrigada < 3 || form.checkNOBR.Checked)
                    {
                        if (tocaBrigada == 3)
                        {
                            tocaBrigada = 0;
                            tipoBrigada="";
                        }

                        if (tipoBrigada != "")
                        {
                            switch (tipoBrigada)
                            {
                                case "infantry_brigade":
                                    countryArmy.Add("infantry_brigade");
                                    break;
                                case "bergsjaeger_brigade":
                                    countryArmy.Add("bergsjaeger_brigade");
                                    break;
                                case "armor_brigade":
                                    countryArmy.Add("armor_brigade");
                                    tocaBrigada = 3;
                                    break;
                                case "paratrooper_brigade":
                                    countryArmy.Add("paratrooper_brigade");
                                    break;
                                case "marine_brigade":
                                    countryArmy.Add("marine_brigade");
                                    break;
                                case "light_armor_brigade":
                                    countryArmy.Add("light_armor_brigade");
                                    break;

                            }
                            tocaBrigada++;
                            continue;
                        }

                        switch (form.ran.Next(0, 25))
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 15:
                            case 16:
                            case 17:
                            case 18:
                            case 19:
                            case 20:
                            case 21:
                            case 22:
                                countryArmy.Add("infantry_brigade");
                                tipoBrigada = "infantry_brigade";
                                break;
                            case 3:
                            case 4:
                            case 23:
                            case 24:
                                if ((techFocus == 1 || techFocus == 2 || techFocus == 3) && form.ran2.Next(0, 10) < 4) //40%chance
                                {
                                    switch (techFocus)
                                    {
                                        case 1:
                                            countryArmy.Add("bergsjaeger_brigade");
                                            tipoBrigada = "bergsjaeger_brigade";
                                            break;
                                        case 2:
                                            countryArmy.Add("paratrooper_brigade");
                                            tipoBrigada = "paratrooper_brigade";
                                            break;
                                        case 3:
                                            countryArmy.Add("marine_brigade");
                                            tipoBrigada = "marine_brigade";
                                            break;
                                    }

                                }
                                else
                                {
                                    countryArmy.Add("infantry_brigade");
                                    tipoBrigada = "infantry_brigade";
                                }
                                break;
                            case 5:
                            case 6:

                                    countryArmy.Add("infantry_brigade");
                                    tipoBrigada = "infantry_brigade";

                                break;
                            case 7:
                            case 14:
                                //only tank focus can have med armors
                                if (techFocus == 0 && form.ran2.Next(0, 4) < 2)
                                {
                                    countryArmy.Add("armor_brigade");
                                    tipoBrigada = "armor_brigade";
                                }
                                else
                                {
                                    countryArmy.Add("light_armor_brigade");
                                    tipoBrigada = "light_armor_brigade";
                                }
                                break;
                            case 8:
                            case 9:         
                                    countryArmy.Add("light_armor_brigade");
                                    tipoBrigada = "light_armor_brigade";
                                break;
                            case 10:
                            case 11:             
                                    countryArmy.Add("infantry_brigade");
                                    tipoBrigada = "infantry_brigade";
                                break;
                            case 12:
                            case 13:
                                    countryArmy.Add("infantry_brigade");
                                    tipoBrigada = "infantry_brigade";
                                break;
                        }
                        tocaBrigada++;
                    }
                    else
                    {
                        tocaBrigada = 0;


                        switch (tipoBrigada)
                        {
                            case "infantry_brigade":
                                switch (form.ran.Next(0, 4))
                                {
                                    case 0:
                                        countryArmy.Add("anti_air_brigade");
                                        break;
                                    case 1:
                                        countryArmy.Add("anti_tank_brigade");
                                        break;
                                    case 2:
                                        countryArmy.Add("artillery_brigade");
                                        break;
                                    case 3:
                                        countryArmy.Add("engineer_brigade");
                                        break;
                                }
                                break;
                            case "bergsjaeger_brigade":
                                createNumber--;
                                break;
                            case "armor_brigade":
                                countryArmy.Add("motorized_brigade");
                                break;
                            case "paratrooper_brigade":
                                createNumber--; // only 3
                                break;
                            case "marine_brigade":
                                createNumber--;
                                break;
                            case "light_armor_brigade":
                                countryArmy.Add("engineer_brigade");
                                break;

                        }


                       tipoBrigada="";
                    
                    }
                    numberOfBrigades++;

                }

            }



        }


        public void setEliteUnit(Form1 form)
        {

            switch (techFocus)
            {
                case 1:
                    switch (form.ran.Next(0, 4))
                    {
                        case 0:
                            eliteunit = "Alpini_brigade";
                            break;
                        case 1:
                            eliteunit = "Alpins_brigade";
                            break;
                        case 2:
                            eliteunit = "Guards_brigade";
                            break;
                        case 3:
                            eliteunit = "legion_brigade";
                            break;
                    }
                    break;
                case 0:
                    switch (form.ran.Next(0, 2))
                    {
                        case 0:
                            eliteunit = "legion_brigade";
                            break;
                        case 1:
                            eliteunit = "waffenSS_brigade";
                            break;
                    }

                    break;
                case 2:
                    switch (form.ran.Next(0, 2))
                    {
                        case 0:
                            eliteunit = "Gurkha_brigade";
                            break;
                        case 1:
                            eliteunit = "ranger_brigade";
                            break;
                    }

                    break;
                case 3:
                    switch (form.ran.Next(0, 2))
                    {
                        case 0:
                            eliteunit = "Imperial_brigade";
                            break;
                        case 1:
                            eliteunit = "legion_brigade";
                            break;
                    }
                    break;

                case 4:
                    break;

            }



        }







        public void assingProvincesNewGenerator(Form1 form, bool setCapital)
        {
            province prov;
            totalIC = 0;
            totalLeadership = 0;
            totalManpower = 0;
            totalPoints = 0;
            numberProvinces = 0;
            hasNavalBases = false;

            for (int count = 0; count <= this.provincesALL.Count - 1; count++)
            {

                prov = (province)provincesALL[count];

                //prov.addCountryCore(tag);

                //prov.selectOwner(tag);

                //prov.selectController(tag);

                totalIC = totalIC + prov.industry;
                totalLeadership = totalLeadership + prov.leadership;
                totalManpower = totalManpower + prov.manpower;
                totalPoints = totalPoints + prov.points;


                if (prov.navalBase > 0)
                {
                    hasNavalBases = true;
                    provincesNaval.Add(prov.id);

                }

                if (prov.coastal)
                {
                    coastal = true;
                    provincesCoastal.Add(prov);//we dont want the id, we want the position on the array
                }

                if (prov.airBase > 0)
                {
                    provincesAir.Add(prov.id);
                }




                if (prov.important)
                {
                    importantProvinces.Add(prov);//we dont want the id, we want the position on the array
                }
                else
                {
                    provincesNotImportant.Add(prov);//we dont want the id, we want the position on the array
                }

                numberProvinces++;
            }



            //at least 1 port if coastal

            if (coastal && !hasNavalBases)
            {

                prov = (province)provincesCoastal[form.ran.Next(0, provincesCoastal.Count)];
                prov.navalBase = 4;
                provincesNaval.Add(prov.id);
                hasNavalBases = true;
            }



            if (setCapital)
            {
                //now we are going to assign the capital

                //has the country important provinces?

                if (importantProvinces.Count == 0) //we need to create at least one important province
                {
                    if (provincesNotImportant.Count == 1)
                    {
                        prov = (province)provincesNotImportant[0];

                    }
                    else if (provincesNotImportant.Count == 3)
                    {
                        prov = (province)provincesNotImportant[1];

                    }
                    else
                    {
                        bool isvalid = true;
                        int intentos = 0;
                        do
                        {

                            prov = (province)provincesNotImportant[form.ran.Next(1, provincesNotImportant.Count)]; //random.next will never be the max value

                            if (prov.isColony)
                            {
                                isvalid = false;
                            }
                            else
                            {
                                isvalid = true;
                            }
                            intentos++;
                        } while (isvalid == false && intentos < 100);
                    }

                    //one province is going to change! 
                    totalIC = totalIC - prov.industry;
                    totalLeadership = totalLeadership - prov.leadership;
                    totalManpower = totalManpower - prov.manpower;
                    totalPoints = totalPoints - prov.points;

                    capitalID = prov.id;

                    provincesAir.Add(capitalID);
                    prov.createResources(form, true);  //now its a ubberprovince
                    prov.importantSG = true;
                    prov.infra = 10;


                    totalIC = totalIC + prov.industry;
                    totalLeadership = totalLeadership + prov.leadership;
                    totalManpower = totalManpower + prov.manpower;
                    totalPoints = totalPoints + prov.points;

                    if (prov.navalBase > 0) //capital will have always naval base if coastal, so no need to check if we destroyed some naval bases.
                    {
                        provincesNaval.Add(capitalID);
                        hasNavalBases = true;
                    }


                }
                else //just select one randomly, but try to stay away from borders (not the first, not the last one.. if possible)
                {
                    if (importantProvinces.Count == 1)
                    {
                        prov = (province)importantProvinces[0];

                    }
                    else if (importantProvinces.Count == 3)
                    {
                        prov = (province)importantProvinces[1];

                    }
                    else
                    {
                        prov = (province)importantProvinces[form.ran.Next(1, importantProvinces.Count)]; //random.next will never be the max value

                    }
                    capitalID = prov.id;
                    provincesAir.Add(capitalID);
                    prov.infra = 10;

                }
                if (totalIC >= Convert.ToInt32(form.textICForRocket.Text))
                {
                    prov.rocket = 1;
                }
            }

        }








    }


}
