using System;
using System.Collections.Generic;
using System.Text;

namespace RandomHoiV2
{
    class minister
    {
        public int id;

        public string name;

        public string ideology;

        public string startDate = "1914.1.1";

        public string type1; //type 1 and type2 are always related, but if in a future we dont want this will be easy to change
        public string type2;

        public double loyalty = 0;

        public string idPicture = "0";


        public minister(int idM)
        {
            id = idM;
        
        }

        public void generateStats(Form1 form)
        {

            
            string aux1;
            string aux2;


            //we take a random name and lastname for the minister
            aux1 = form.names[form.ran.Next(0, form.names.Count)].ToString();
            aux2 = form.lastnames[form.ran.Next(0, form.lastnames.Count)].ToString();

            //we dont want only lowercase names
            aux1 = aux1.ToLower();
            aux2 = aux2.ToLower();

            //the first letter uppercase
            aux1 = aux1.Substring(0, 1).ToUpper() + aux1.Substring(1);
            aux2 = aux2.Substring(0, 1).ToUpper() + aux2.Substring(1);

            name = aux1 + " " + aux2;

            loyalty = form.ran.Next(0, 3);


            switch (form.ran.Next(1, 11))
            {
                case 1:
                    ideology = "national_socialist";
                    break;
                case 2:
                    ideology = "fascistic";
                    break;
                case 3:
                    ideology = "social_conservative";
                    break;
                case 4:
                    ideology = "paternal_autocrat";
                    break;
                case 5:
                    ideology = "stalinist";
                    break;
                case 6:
                    ideology = "leninist";
                    break;
                case 7:
                    ideology = "market_liberal";
                    break;
                case 8:
                    ideology = "social_democrat";
                    break;
                case 9:
                    ideology = "social_liberal";
                    break;
                case 10:
                    ideology = "left_wing_radical";
                    break;

                    

            }

            idPicture = form.ministerPics[form.ran.Next(0, form.ministerPics.Count)].ToString();

            //every minsiter created will have two related postitions

            switch (form.ran.Next(1, 6))
            {

                case 1:
                    type1 = "head_of_state =" + form.mhTraits[form.ran.Next(0, form.mhTraits.Count)];
                    type2 = "head_of_government =" + form.mhTraits[form.ran.Next(0, form.mhTraits.Count)];
                    break;
                case 2:
                    type1 = "foreign_minister =" + form.mForeTraits[form.ran.Next(0, form.mForeTraits.Count)];
                    type2 = "minister_of_intelligence =" + form.mIntelTraits[form.ran.Next(0, form.mIntelTraits.Count)];
                    break;
                case 3:
                    type1 = "armament_minister =" + form.mArmamentTraits[form.ran.Next(0, form.mArmamentTraits.Count)];
                    type2 = "minister_of_security =" + form.mSecurityTraits[form.ran.Next(0, form.mSecurityTraits.Count)];
                    break;
                case 4:
                    type1 = "chief_of_staff =" + form.mStaffTraits[form.ran.Next(0, form.mStaffTraits.Count)];
                    type2 = "chief_of_army =" + form.mArmyTraits[form.ran.Next(0, form.mArmyTraits.Count)];

                    break;
                case 5:
                    type1 = "chief_of_navy =" + form.mNavalTraits[form.ran.Next(0, form.mNavalTraits.Count)];
                    type2 = "chief_of_air =" + form.mAirTraits[form.ran.Next(0, form.mAirTraits.Count)];
                    break;
            }



            if (form.ran.Next(1, 11) >= 9)
            {
                switch (form.ran.Next(1, 10))
                {

                    case 1:
                    case 2:
                    case 9:
                        startDate = "1937.6.1";
                        break;
                    case 3:
                    case 4:
                        startDate = "1939.1.1";
                        break;
                    case 5:
                        startDate = "1940.6.1";
                        break;
                    case 6:
                        startDate = "1942.1.1";
                        break;
                    case 7:
                        startDate = "1943.6.1";
                        break;
                    case 8:
                        startDate = "1945.1.1";
                        break;



                }

            }
        }
    }
}
