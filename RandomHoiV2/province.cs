using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace RandomHoiV2
{
    public class province
    {
        public string id;                  //province ID



        public bool coastal;             //is coastal
        public bool artic;             //is artic
        public bool important;         //is an important province
        public int points;             //points
        public ArrayList countriesCore = new ArrayList();  //countries with core here

        public string ownerTag;        //country owner
        public string controllerTag;   //actual country controller


        //resources
        public double energy = 0;
        public double metal = 0;
        public double rare = 0;
        public double oil = 0;
        public double manpower = 0;
        public double leadership = 0;
        public String resource = "";

        public bool isColony = true;  //new generator

        public ArrayList adje = new ArrayList(); //new generator
        public ArrayList adjeNoDel = new ArrayList(); //new generator
        public ArrayList adjeProvinceGenerated = new ArrayList();

        //buildings
        //yay! too many public, but whatever, this program dont need protection of any kind.(lazy!)
        public int industry = 0;

        public int AAA = 0;
        public int navalBase = 0;
        public int airBase = 0;
        public int infra = 0;

        public int nuclear = 0;
        public int rocket = 0;

        public int landFort = 0;
        public int navalFort = 0;


        public province(string idProvince)
        {
            id = idProvince;
            important = false;
            artic = false;
            coastal = false;
            points = 0;


        }

        public province(string idProvince, bool newGenerator, Form1 form,bool colony)
        {
        
            id = idProvince;
            important = false;
            artic = false;
            coastal = false;
            points = 0;
            isColony = colony;

            for (int count =0;count <= form.provinceAdj.Count-1;count++){

                if (form.provinceAdj[count].ToString().Substring(0, form.provinceAdj[count].ToString().IndexOf(":")) == idProvince)
                {
                    string[] adj;
                    adj = form.provinceAdj[count].ToString().Substring(form.provinceAdj[count].ToString().IndexOf(":")+1).Split(';');
                    for (int count1 = 0; count1 <= adj.Length - 1; count1++)
                    {
                        adje.Add(adj[count1]);
                        adjeNoDel.Add(adj[count1]);
                    }
                    
                    break;
                }
                
            }
           


        }


        public bool importantSG
        {
            get { return important; }
            set { important = value; }
        }

        public bool coastalSG //wops, now its public and this is not ... bah, whatever :F
        {
            get { return coastal; }
            set { coastal = value; }
        }

        public bool articSG
        {
            get { return artic; }
            set { artic = value; }
        }


        public void createResources(Form1 form, bool capital) //capital is to recalculate later a province if a country needs a capital
        {
            bool VPimportant = false;

            if (form.checkVPImportant.Checked)
            {
                if (form.provinceVP.Contains(id))
                {
                    VPimportant = true;
                }
            }

            double addRes = 1; //for new generator
            double addIC = 1; //for new generator
            double addLea = 1; //for new generator
            double addManpo = 1; //for new generator
            int addinfra = 0;

            if (form.radioButtonMapNew.Checked && form.checkBoxColinialResources.Checked)
            {
                if (this.isColony)
                {
                     addRes = 2; //for new generator
                     addIC = 0.5; //for new generator
                     addLea = 0.5; //for new generator
                     addManpo = 0.5; //for new generator
                }
            }

            if (form.radioButtonMapNew.Checked )
            {
                if (this.isColony)
                {
                     addinfra = 0 - Convert.ToInt32(form.textBoxInfraColon.Text); //for new generator

                }
            }

            if (important || capital || VPimportant)
            {
                points = 3;
                metal = (form.ran.Next(3, 6) * (2.25 * form.por_Res * addRes));
                energy = (form.ran.Next(4, 7) * 3 * form.por_Res * addRes);
                rare = (form.ran.Next(2, 4) * (1.75 * form.por_Res * addRes));
                industry = Convert.ToInt32(form.ran.Next(3, 11) * form.por_IC * addIC);

                points = points + industry;


                if (VPimportant)
                {
                    landFort = 10;
                }
                else
                {

                    if (form.ran.Next(1, 10) >= ((100 - Convert.ToInt32(form.textchanceICLand.Text)) / 10) && (industry >= Convert.ToInt32(form.textICLand.Text)))
                    {
                        landFort = form.ran.Next(Convert.ToInt32(form.textMinICLand.Text), Convert.ToInt32(form.textMaxICLand.Text));

                    }
                }


                if (VPimportant)
                {
                    AAA = 10;
                }
                else
                {
                    AAA = (form.ran.Next(2, 8));
                }
                manpower = (form.ran.Next(1, 6) * (1.35 * form.por_MP) * addManpo);

                points = points + Convert.ToInt32(Convert.ToInt32(Math.Round(manpower)));

                if (VPimportant)
                {
                    points = points + 30;
                }

                if (form.ran.Next(1, 31) >= form.probOil)
                {
                    oil = (form.ran.Next(1, 5) * (0.90 * form.por_oil) * addRes);
                    points = points + Convert.ToInt32(Math.Round(oil / 2));
                }

                if (VPimportant)
                {
                    airBase = 10;
                }
                else
                {
                    if (form.ran.Next(1, 10) >= 5 || capital)
                    {
                        airBase = (form.ran.Next(4, 10));

                    }
                }

                if (coastal)
                {
                    if (VPimportant)
                    {
                        navalBase = 10;
                        points = points + Convert.ToInt32(Math.Round(Convert.ToDouble(navalBase / 4)));
                        navalFort = 10;
                    }
                    else
                    {
                        navalBase = (form.ran.Next(5, 11));
                        points = points + Convert.ToInt32(Math.Round(Convert.ToDouble(navalBase / 4)));

                        if ((form.ran.Next(1, 10) >= ((100 - Convert.ToInt32(form.textchanceNVNaval.Text)) / 10)) && (navalBase >= Convert.ToInt32(form.textNVNaval.Text)))
                        {
                            navalFort = form.ran.Next(Convert.ToInt32(form.textMinNVNaval.Text), Convert.ToInt32(form.textMaxNVNaval.Text));

                        }
                    }

                }



                switch (form.ran.Next(0, 8))
                {
                    case 0:
                        leadership = 0.20;
                        break;

                    case 1:
                        leadership = 0.40;
                        break;

                    case 2:
                        leadership = 0.60;
                        break;

                    case 3:
                        leadership = 0.80;
                        break;

                    case 4:
                        leadership = 1.00;
                        break;

                    case 5:
                        leadership = 1.20;
                        break;

                    case 6:
                        leadership = 1.40;
                        break;

                    case 7:
                        leadership = 1.60;
                        break;

                }

                points = points + Convert.ToInt32(Math.Round(leadership * addLea));


            }
            else
            {
                points = 0;
                if (form.ran.NextDouble() >= 0.8)
                {
                    if (form.ran.NextDouble() >= 0.7)
                    {
                        metal = (form.ran.Next(1, 6) * form.por_Res * addRes);
                    }
                    if (form.ran.NextDouble() >= 0.5)
                    {
                        energy = (form.ran.Next(1, 7) * 1.5 * form.por_Res * addRes);

                    }
                    if (form.ran.NextDouble() >= 0.8)
                    {
                        rare = (form.ran.Next(1, 4) * form.por_Res * addRes);

                    }
                    if (form.ran.Next(1, 40) >= 38)
                    {

                        industry = Convert.ToInt32(form.ran.Next(1, 4) * form.por_IC * addIC);


                    }
                    if (form.ran.NextDouble() >= 0.7)
                    {
                        AAA = (form.ran.Next(1, 3));
                    }
                    if (form.ran.NextDouble() >= 0.9)
                    {
                        manpower = (form.ran.Next(1, 5) * 0.65 * form.por_MP * addManpo);

                    }
                    if (form.ran.Next(1, 100) >= form.probOil2)
                    {
                        oil = (form.ran.Next(1, 7) * 0.25 * form.por_oil * addRes);

                    }


                    if (form.ran.Next(1, 16) >= 15)
                    {
                        airBase = (form.ran.Next(1, 3));
                    }

                    if (coastal)
                    {
                        if (form.ran.Next(1, 15) >= 9)
                        {
                            navalBase = (form.ran.Next(5, 7));
                        }
                    }

                    if (form.ran.Next(1, 30) >= 28)
                    {
                        switch (form.ran.Next(0, 4))
                        {
                            case 0:
                                leadership = 0.20 * addLea;
                                break;

                            case 1:
                                leadership = 0.40 * addLea;
                                break;

                            case 2:
                                leadership = 0.60 * addLea;
                                break;

                            case 3:
                                leadership = 0.80 * addLea;
                                break;
                        }
                    }

                }

            }
            if (VPimportant)
            {
                infra = 10;
            }
            else
            {

                if (industry >= Convert.ToInt32(form.textICLand.Text))
                {
                    if (Convert.ToInt32(form.textchanceICLand.Text) >= form.ran.Next(0, 101))
                    {
                        landFort = form.ran.Next(Convert.ToInt32(form.textMinICLand.Text), Convert.ToInt32(form.textMaxICLand.Text));
                    }

                }

                if (navalBase >= Convert.ToInt32(form.textNVNaval.Text))
                {
                    if (Convert.ToInt32(form.textchanceNVNaval.Text) >= form.ran.Next(0, 101))
                    {
                        navalFort = form.ran.Next(Convert.ToInt32(form.textMinNVNaval.Text), Convert.ToInt32(form.textMaxNVNaval.Text));
                    }

                }





                infra = form.ran.Next(Convert.ToInt32(form.textInfra.Text), Convert.ToInt32(form.textmaxInfra.Text) + 1);

                infra = infra + addinfra;

                if (navalBase > 0)
                {
                    infra = infra + 1;

                }

                if (infra > 10)
                {
                    infra = 10;
                }
                else if (infra < 0)
                {
                    infra = 0;
                }


            }


        }

        public void addCountryCore(string tag)
        {

            countriesCore.Add(tag);

        }

        public void selectOwner(string tag)
        {

            ownerTag = tag;

        }

        public void selectController(string tag)
        {

            controllerTag = tag;

        }


        public void addInfraToMajors(int bonus, string major1, string major2, string major3)
        {
            if ((ownerTag == major1 || ownerTag == major2 || ownerTag == major3) && infra < 10)
            {
                infra = infra + bonus;

                if (infra > 10)
                {
                    infra = 10;
                }
            }

        }

        public void addNuclearToMajors(int bonus, string capital1, string capital2, string capital3)
        {
            if ((id == capital1 || id == capital2 || id == capital3) && nuclear < 10)
            {
                nuclear = nuclear + bonus;

                if (nuclear > 10)
                {
                    nuclear = 10;
                }
            }


        }

    }

}
