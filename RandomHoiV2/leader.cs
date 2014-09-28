using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace RandomHoiV2
{
    class leader
    {

        public int id;

        public string name;


        public int skill = 0;
        public int maxskill = 0;
        public int rank = 0;

        public string type;

        public double loyalty = 0;

        public ArrayList traits = new ArrayList();

        public string idPicture = "0";


        public leader(int idL)
        {
            id = idL;
        
        }





        public void generateStats(Form1 form,string leaderType,int maxSk)
        {
            type = leaderType;
            int maxSetSkill;
            int conta2;


            skill = 0;
            maxskill = 0;
            loyalty = 0;
            maxSetSkill = 0;

            string aux1;
            string aux2;


                //we take a random name and lastname for the leader
            aux1 = form.names[form.ran.Next(0, form.names.Count)].ToString();
            aux2 = form.lastnames[form.ran.Next(0, form.lastnames.Count)].ToString();

                //we dont want only uppercase names
            aux1 = aux1.ToLower();
            aux2 = aux2.ToLower();

                //the first letter uppercase
            aux1 = aux1.Substring(0, 1).ToUpper() + aux1.Substring(1);
            aux2 = aux2.Substring(0, 1).ToUpper() + aux2.Substring(1);

            name = aux1 + " " + aux2;


                switch (type)
                {
                    case "land":
                        maxSetSkill = (Convert.ToInt32(form.textMxSLL.Text));
                        break;
                    case "air":
                        maxSetSkill = (Convert.ToInt32(form.textMxSLL.Text)) ;
                        break;
                    case "sea":
                        maxSetSkill = (Convert.ToInt32(form.textMxSLL.Text));
                        break;
                
                }

                switch (maxSetSkill)
                {

                    case 0:
                    case 1:
                    case 2:
                    case 3:

                        skill = form.ran.Next(0, maxSetSkill + 1);

                        break;

                    case 4:
                    case 5:
                    case 6:
                        /*this way its more probable to have leaders whit less skill*/
                        if (form.ran.Next(1, 11) <= 6)
                        {
                            /*max skill 3 for this leader)*/
                            skill = form.ran.Next(0, 4);
                        }
                        else
                        {
                            /*ok, max skill possible at maximum setting*/
                            skill = form.ran.Next(0, maxSetSkill + 1);
                        }


                        break;

                    case 7:
                    case 8:

                        if (form.ran.Next(1, 11) <= 7)
                        {
                            skill = form.ran.Next(0, 7);
                        }
                        else
                        {
                            skill = form.ran.Next(0, maxSetSkill + 1);
                        }

                        break;

                    case 9:
                    case 10:

                        if (form.ran.Next(1, 11) <= 8)
                        {
                            skill = form.ran.Next(0, 9);
                        }
                        else
                        {
                            skill = form.ran.Next(0, maxSetSkill + 1);
                        }

                        break;

                }


                maxskill = form.ran.Next(skill, 11); //maximun of a maximun skill for a leader is always 10,and the minimun his skill

                loyalty = form.ran.Next(0, 3);


                idPicture = form.leadersPics[form.ran.Next(0, form.leadersPics.Count)].ToString();

                if (form.ran.Next(1, 4) > 1) /*if 1 the leader will not have any traits*/
                {
                    switch (type)
                    {
                        case "land":
                            for (conta2 = 0; conta2 <= form.landTraits.Count - 1; conta2++)
                            {
                                if (form.ran.NextDouble() >= 0.9)
                                {
                                    traits.Add(form.landTraits[conta2]);
                                }
                            }
                            break;
                        case "air":
                            for (conta2 = 0; conta2 <= form.airTraits.Count - 1; conta2++)
                            {
                                if (form.ran.NextDouble() >= 0.9)
                                {
                                    traits.Add(form.airTraits[conta2]);
                                }
                            }
                            break;
                        case "sea":
                            for (conta2 = 0; conta2 <= form.navalTraits.Count - 1; conta2++)
                            {
                                if (form.ran.NextDouble() >= 0.9)
                                {
                                    traits.Add(form.navalTraits[conta2]);
                                }
                            }
                            break;

                    }


                }

                /*rank for the leader*/
                switch (form.ran.Next(0, 13))
                {
                    case 0:
                    case 1:
                    case 2:
                        rank = 0;
                        break;

                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        rank = 1;
                        break;

                    case 7:
                    case 8:
                    case 9:
                        rank = 2;
                        break;

                    case 10:
                    case 11:
                        rank = 3;
                        break;

                    case 12:
                        rank = 4;
                        break;

                }



            
        }

    }
}
