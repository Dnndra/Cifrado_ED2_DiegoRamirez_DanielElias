using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Buffers.Binary;

namespace LibreriaRDCifrado
{
  public  class RSA
    {
       public string public_key;
        public string private_key;
        public void GenerarLlaves(int p, int q)
        {

            int n = p * q;
            int Fi_n = (p - 1) * (q - 1);
            int i;
            for (i = 2; i < Fi_n; i++)
            {
                if (coprime(Fi_n, i) == true && coprime(n, i) == true) 
                {
                    break;
                }

            }
            int e = i;
            int d = calcularD(e, Fi_n);

             public_key = Convert.ToString(e) + "," + Convert.ToString(n);
             private_key = Convert.ToString(d) + "," + Convert.ToString(n);

        }



        public byte[] RSA_CYPHER(byte[] mensaje, int n, int k)
        {      
            var numeros = new List<int>();
          
            foreach (int b in mensaje)
            {             
                
                 int cifrado = (int)BigInteger.ModPow(b, n, k);
                numeros.Add(cifrado);
             
            }
            byte[] mensajefinal = numeros.SelectMany(BitConverter.GetBytes).ToArray();
            byte[] identificar = { default };
            return Combine(identificar,mensajefinal);

        }
        public byte[] RSA_DECYPHER(byte[] mensaje, int n, int k)
        {

            mensaje = mensaje.Skip(1).ToArray();

            var numerosmensaje = new int[mensaje.Length / 4];
            Buffer.BlockCopy(mensaje, 0, numerosmensaje, 0, mensaje.Length);

            List<byte> mensajedescifrado = new List<byte>();

            foreach (int b in numerosmensaje)
            {

                int cifrado = (int)BigInteger.ModPow(b, n, k);

                mensajedescifrado.Add((byte)cifrado);
            }

            return mensajedescifrado.ToArray();
        }





        static int calcularD(int a, int m)
        {

            for (int x = 1; x < m; x++)
                if (((a % m) * (x % m)) % m == 1)
                    return x;
            return 1;
        }
       public bool EsPrimo(int numero)
        {
            for (int i = 2; i < numero; i++)
            {
                if ((numero % i) == 0)
                {
                   //no primo 
                    return false;
                }
            }

            //primo
            return true;
        }

        static bool coprime(long u, long v)
        {
            if (((u | v) & 1) == 0) return false;

            while ((u & 1) == 0) u >>= 1;
            if (u == 1) return true;

            do
            {
                while ((v & 1) == 0) v >>= 1;
                if (v == 1) return true;

                if (u > v) { long t = v; v = u; u = t; }
                v -= u;
            } while (v != 0);

            return false;
        }

        static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }




    }
}
