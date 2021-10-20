using System;
using System.Collections.Generic;
namespace LibreriaRDCifrado
{
    public class CifradoCesar : IEncriptador
    {
        public string Cipher(string mensaje, string clave)
        {

            Dictionary<char, int> diccionarioOriginal = new Dictionary<char, int>();
            Dictionary<int, char> diccionarioclave = new Dictionary<int, char>();
            char Current;

            string mensajecifrado = "";
            for (int i = 0; i < 256; i++)
            {
                diccionarioOriginal.Add((char)i, i);
            }

            foreach (char c in clave)
            {
                if (!diccionarioclave.ContainsValue(c))
                {
                    diccionarioclave.Add(diccionarioclave.Count, c);
                }

            }

            for (int i = 0; i < 256; i++)
            {
                if (!diccionarioclave.ContainsValue((char)i))
                {
                    diccionarioclave.Add(diccionarioclave.Count, (char)i);
                }
            }

            foreach (char x in mensaje)
            {
                Current = (char)diccionarioOriginal[x];

                mensajecifrado += diccionarioclave[Current];


            }

            return mensajecifrado;
        }
        public string Decipher(string mensaje, string clave)
        {
            Dictionary<int, char> diccionarioOriginal = new Dictionary<int, char>();
            Dictionary<char, int> diccionarioclave = new Dictionary<char, int>();
            int current;
            string mensajeDescifrado = "";
            for (int i = 0; i < 256; i++)
            {
                diccionarioOriginal.Add(i, (char)i);
            }

            foreach (char c in clave)
            {
                if (!diccionarioclave.ContainsKey(c))
                {
                    diccionarioclave.Add(c, diccionarioclave.Count);
                }

            }

            for (int i = 0; i < 256; i++)
            {
                if (!diccionarioclave.ContainsKey((char)i))
                {
                    diccionarioclave.Add((char)i, diccionarioclave.Count);
                }
            }

            foreach (char x in mensaje)
            {
                current = diccionarioclave[x];

                mensajeDescifrado += diccionarioOriginal[current];


            }

            return mensajeDescifrado;
        }

        public string[] GenerarLlave(string llave, int[] P10, int[] P8)
        {
            throw new NotImplementedException();
        }
    }
}
