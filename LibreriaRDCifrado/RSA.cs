using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Text;
namespace LibreriaRDCifrado
{
  public  class RSA
    {
        public List<int> GenerarLlaves(int p, int q)
        {

            List<int> Claves = new List<int>();
            int n = p * q;
            int Fi_n = (p - 1) * (q - 1);
            int i;
            for (i = 2; i < Fi_n; i++)
            {
                if (gccd(Fi_n, i) == 1 && gccd(n, i) == 1)
                {
                    break;
                }

            }
            int e = i;
            int d = modInverse(e, Fi_n);
            Claves.Add(n);
            Claves.Add(e);
            Claves.Add(d);
            return Claves;


        }



        public byte[] RSA_ALGORITHM(byte[] mensaje, List<int> llaves)
        {
            var numeros = new List<byte>();
            foreach(byte b in mensaje)
            {

                int C = power(b, llaves[1], llaves[0]);
                numeros.Add((byte)C);
            }

            byte[] mensajeBytes = numeros.ToArray();

            return mensajeBytes;

        }


        static int power(int x, int y, int p)
        {
            int res = 1;

            x = x % p; 

            if (x == 0)
                return 0; 

            while (y > 0)
            {

              
                if ((y & 1) != 0)
                    res = (res * x) % p;

              
                y = y >> 1; 
                x = (x * x) % p;
            }
            return res;
        }
        static int modInverse(int a, int m)
        {

            for (int x = 1; x < m; x++)
                if (((a % m) * (x % m)) % m == 1)
                    return x;
            return 1;
        }
        int gccd(int a, int b)
        {
            if (a == 0 || b == 0)
                return 0;
            if (a == b)
            {
                return a;
            }

            if (a > b)
            {
                return gccd(a - b, b);
            }

            return gccd(a, b - a);
        }

    }
}
