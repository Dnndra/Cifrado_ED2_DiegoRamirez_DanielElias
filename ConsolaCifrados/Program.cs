using System;
using System.IO;
using System.Linq;
using System.Reflection;
using LibreriaRDCifrado;

namespace ConsolaCifrados
{
    class Program
    {
        public void ObtenerPermutaciones(int[] P10, int[] P8, int[] P4, int[] EP, int[] IP, int[] IP_1)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\Permutations.txt");
            var reader = new StreamReader(path);
            string linea = reader.ReadLine();
            string[] PACTUAL = linea.Split(',');

            foreach (string s in PACTUAL)
            {
                int num = Convert.ToInt32(s);
                P10.Append(num);
            }

            linea = reader.ReadLine();
            PACTUAL = linea.Split(',');
            foreach (string s in PACTUAL)
            {
                int num = Convert.ToInt32(s);
                P8.Append(num);
            }

        }
        static void Main(string[] args)
        {
            var cesar = new CifradoCesar();
            var zigzag = new CifradoZigZag();
            var SDES = new SDES();
            string mensajeOriginal = "Marco Antonio Solis";
            string claveCesar = "pinula";
            int claveZigzag = 5;

            string llaveSDES = "50";

            int[] P10  = new int[10];
            int[] P8 = new int[8];
            int[] P4 = new int[4];
            int[] EP = new int[8];
            int[] IP = new int[8];
            int[] IP_1 = new int[8];
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\Permutations.txt");
            var reader = new StreamReader(path);

            string linea = reader.ReadLine();
            string[] PACTUAL = linea.Split(',');

            for (int i = 0; i < PACTUAL.Length; i++)
            {
                int num = Convert.ToInt32(PACTUAL[i]);
                P10[i] = num;
            }

            linea = reader.ReadLine();
            PACTUAL = linea.Split(',');
            for (int i = 0; i < PACTUAL.Length; i++)
            {
                int num = Convert.ToInt32(PACTUAL[i]);
                P8[i] = num;
            }
            linea = reader.ReadLine();
            PACTUAL = linea.Split(',');
            for (int i = 0; i < PACTUAL.Length; i++)
            {
                int num = Convert.ToInt32(PACTUAL[i]);
                P4[i] = num;
            }
            linea = reader.ReadLine();
            PACTUAL = linea.Split(',');
            for (int i = 0; i < PACTUAL.Length; i++)
            {
                int num = Convert.ToInt32(PACTUAL[i]);
                EP[i] = num;
            }
            linea = reader.ReadLine();
            PACTUAL = linea.Split(',');
            for (int i = 0; i < PACTUAL.Length; i++)
            {
                int num = Convert.ToInt32(PACTUAL[i]);
                IP[i] = num;
            }
            linea = reader.ReadLine();
            PACTUAL = linea.Split(',');
            for (int i = 0; i < PACTUAL.Length; i++)
            {
                int num = Convert.ToInt32(PACTUAL[i]);
                IP_1[i] = num;
            }



            Console.WriteLine(SDES.GenerarLlave(llaveSDES, null, null));




            Console.WriteLine("El mensaje original es: " + mensajeOriginal + ", la clave es: " + claveCesar);

            Console.WriteLine("El codificado por Cesar es: " + cesar.Cipher(mensajeOriginal, claveCesar));

            Console.WriteLine("El mensaje decodificado por Cesar es: " + cesar.Decipher(cesar.Cipher(mensajeOriginal, claveCesar), claveCesar));

            Console.WriteLine("La clave para zigzag es: " + claveZigzag);

            Console.WriteLine("El mensaje codificado por Zigzag es: " + zigzag.Cipher(mensajeOriginal, claveZigzag.ToString()));

            Console.WriteLine("El mensaje decodificado por Zigzag es: " + zigzag.Decipher(zigzag.Cipher(mensajeOriginal, claveZigzag.ToString()), claveZigzag.ToString()));

            Console.ReadKey();
        }

       
    }
}
