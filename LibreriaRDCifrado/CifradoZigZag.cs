using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaRDCifrado
{
    public class CifradoZigZag : IEncriptador
    {
        public string Cipher(string mensaje, string clave)
        {
            //int rail, string plaintext
            string plainText = mensaje;
            int rail;
            try
            {
               rail = Convert.ToInt32(clave);
            }
            catch
            {
                return null;
            }
            
            List<string> railFence = new List<string>();
            for (int i = 0; i < rail; i++)
            {
                railFence.Add("");
            }

            int number = 0;
            int x = 1;
            foreach (char c in plainText)
            {
                if (number + x == rail)
                {
                    x = -1;
                }
                else if (number + x == -1)
                {
                    x = 1;
                }
                railFence[number] += c;
                number += x;
            }

            string buffer = "";
            foreach (string s in railFence)
            {
                buffer += s;
            }
            return buffer;
        }

        public string Decipher(string mensaje, string clave)
        {
            string Codificado = mensaje;
            int nClave;
            try
            {
                nClave = Convert.ToInt32(clave);
            }
            catch
            {
                return null;
            }
            int longitud = Codificado.Length;
            //"matriz dinámica"
            List<List<int>> LineasZigZag = new List<List<int>>();
            for (int i = 0; i < nClave; i++)
            {
                LineasZigZag.Add(new List<int>());
            }

            int n = 0;
            int y = 1;
            for (int i = 0; i < longitud; i++)
            {
                if (n + y == nClave)
                {
                    y = -1;
                }
                else if (n + y == -1)
                {
                    y = 1;
                }
                LineasZigZag[n].Add(i);
                n += y;
            }

            int res = 0;
            char[] decodificado = new char[longitud];
            for (int i = 0; i < nClave; i++)
            {
                for (int j = 0; j < LineasZigZag[i].Count; j++)
                {
                    decodificado[LineasZigZag[i][j]] = Codificado[res];
                    res++;
                }
            }
            string mensajeFinal = new string(decodificado);

            return mensajeFinal;
        }

        public string[] GenerarLlave(string llave, int[] P10, int[] P8)
        {
            throw new NotImplementedException();
        }
    }
}
