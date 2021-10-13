using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaRDCifrado
{
   public  class SDES
    {

        public string Cypher(string clave, List<byte> mensaje )
        {
            string result= "";
            string bebe = "";
            string aux = "";
            string finalresult = "";
            result = Convert.ToString(Convert.ToInt32(clave, 10), 2);

            for (int m = 0; m < (10-result.Length); m++)
            {

                aux += "0";

            }

            string P10 = "37512910486";
            finalresult = aux + result;

            foreach(char z  in P10)
            {
                bebe += finalresult.Substring(3, 1);

            }





            return bebe;

        }

    }
}
