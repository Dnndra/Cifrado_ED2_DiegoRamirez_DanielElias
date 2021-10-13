using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LibreriaRDCifrado;

namespace LibreriaRDCifrado
{
   public  class SDES
    {

        public string LeftShift1(string cadena)
        {
            string shifted = "";
            for (int i = 0; i < cadena.Length; i++)
            {
                while (i > 3)
                {
                    shifted += cadena[0];
                    i++;
                    if (i >= 4)
                    {
                        return shifted;
                    }
                }
                shifted += cadena[i + 1];
            }
            return shifted;
        }

        public string Permutacion8(int[] P8, string cadena)
        {
            string salida = "";
            int posicionP8 = 0;
            for (int i = 0; i < 8; i++)
            {
                posicionP8 = P8[i];
                salida += cadena[posicionP8 - 1].ToString();
            }
            return salida;

        }
        public string[] GenerarLlave(string llave, int[] P10, int[] P8)
        {
            if (Convert.ToInt32(llave) > 1023)
            {
                //llave incorrecta
                return null;
            }
            else
            {

                string llaveEnBinario = Convert.ToString(Convert.ToInt32(llave, 10), 2);
                string key = "";
                string[] llaves = new string[2];

                if (llaveEnBinario.Length < 10)
                {
                    string aux = "";
                    for (int m = 0; m < 10 - llaveEnBinario.Length; m++)
                    {

                        aux += "0";

                    }

                    key += aux + llaveEnBinario;
                    aux = "";
                }
                else
                {
                    key = llaveEnBinario;
                }
                //Hacer P10
                int[] p10 = { 3, 5, 2, 7, 4, 10, 1, 9, 8, 6 };
                string llaveConP10 = "";
                int posicionP10 = 0;
                for (int i = 0; i < key.Length; i++)
                {
                    posicionP10 = p10[i];
                    llaveConP10 += key[posicionP10-1].ToString();
                }
                //Hacer split y LS-1
                string LSizquierda = llaveConP10.Substring(0,5);
                string LSderecha = llaveConP10.Substring(5, 5);
                LSizquierda = LeftShift1(LSizquierda);
                LSderecha = LeftShift1(LSderecha);
                //HACER P8 Y GENERAR K1
                string llaveConP8 = LSizquierda + LSderecha;
                int[] p8 = { 6, 3, 7, 4, 8, 5, 10, 9 };
                llaveConP8 =Permutacion8(p8, llaveConP8);
                //ALMACENAR K1
                llaves[0] = llaveConP8;
                //HACER LEFTSHIFT 2
                LSizquierda = LeftShift1(LSizquierda);
                LSderecha = LeftShift1(LSderecha);
                LSizquierda = LeftShift1(LSizquierda);
                LSderecha = LeftShift1(LSderecha);
                llaveConP8 = LSizquierda + LSderecha;
                //HACER P8 Y ALMACENAR K2
                llaves[1] = Permutacion8(p8, llaveConP8);




                return llaves;
            }
        }

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
