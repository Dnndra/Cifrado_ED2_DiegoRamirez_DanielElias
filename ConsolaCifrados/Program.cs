using System;
using LibreriaRDCifrado;

namespace ConsolaCifrados
{
    class Program
    {
        static void Main(string[] args)
        {
            var cesar = new CifradoCesar();
            var zigzag = new CifradoZigZag();
            string mensajeOriginal = "Marco Antonio Solis";
            string claveCesar = "pinula";
            int claveZigzag = 5;
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
