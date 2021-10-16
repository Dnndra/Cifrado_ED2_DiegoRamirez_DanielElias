using System;
using System.Collections.Generic;

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
        public string IP(int[] IP, string cadena)
        {
            string salida = "";
            int posicionIP = 0;
            for (int i = 0; i < 8; i++)
            {
                posicionIP = IP[i];
                salida += cadena[posicionIP - 1].ToString();
            }
            return salida;
        }
        public string EP(int[] EP, string cadena)
        {
            string salida = "";
            int posicionEP = 0;
            for (int i = 0; i < 8; i++)
            {
                posicionEP = EP[i];
                salida += cadena[posicionEP - 1].ToString();
            }
            return salida;

        }


        public string IP1(int[] IP1,string cadena)
        {
            string salida = "";
            int posicionIP1 = 0;
            for (int i = 0; i < 8; i++)
            {
                posicionIP1 = IP1[i];
                salida += cadena[posicionIP1 - 1].ToString();
            }
            return salida;
        }
        public string P4(int[] P4, string cadena)
        {
            string salida = "";
            int posicionP4 = 0;
            for (int i = 0; i < 4; i++)
            {
                posicionP4 = P4[i];
                salida += cadena[posicionP4 - 1].ToString();
            }
            return salida;
        }
        public string Suma(string cadena1, string cadena2)
        {
            string resultado = "";
            for (int  i =0; i<cadena1.Length; i++)
            {

                if (cadena1[i] == cadena2[i])
                {
                    resultado += "0";
                }
                else
                {
                    resultado += "1";
                }
            }
            return resultado;
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
              
                string llaveConP10 = "";
                int posicionP10 = 0;
                for (int i = 0; i < key.Length; i++)
                {
                    posicionP10 = P10[i];
                    llaveConP10 += key[posicionP10-1].ToString();
                }
                //Hacer split y LS-1
                string LSizquierda = llaveConP10.Substring(0,5);
                string LSderecha = llaveConP10.Substring(5, 5);
                LSizquierda = LeftShift1(LSizquierda);
                LSderecha = LeftShift1(LSderecha);
                //HACER P8 Y GENERAR K1
                string llaveConP8 = LSizquierda + LSderecha;
        
                llaveConP8 =Permutacion8(P8, llaveConP8);
                //ALMACENAR K1
                llaves[0] = llaveConP8;
                //HACER LEFTSHIFT 2
                LSizquierda = LeftShift1(LSizquierda);
                LSderecha = LeftShift1(LSderecha);
                LSizquierda = LeftShift1(LSizquierda);
                LSderecha = LeftShift1(LSderecha);
                llaveConP8 = LSizquierda + LSderecha;
                //HACER P8 Y ALMACENAR K2
                llaves[1] = Permutacion8(P8, llaveConP8);




                return llaves;
            }
        }

        public List<byte> Cypher(string llave, byte[] mensaje, int[] P4Q, int[] EPQ , int[] IPQ, int[] IP1Q, int[] P10, int[] P8)
        {

            string[,] So = { { "01","00","11","10"},
                             { "11","10","01","00"},
                             { "00","10","01","11"},
                             { "11","01","11","10"} };

            string[,] S1 = { { "00","01","10","11"},
                             { "10","00","01","11"},
                             { "11","00","01","00"},
                             { "10","01","00","11"} };

            List<byte> Mensajecifrado = new List<byte>();

            string[] keys = GenerarLlave(llave, P10, P8);

            for (int i = 0; i < mensaje.Length; i++)
            {



                string Charenbinario = Convert.ToString(mensaje[i], 2);

                string caracterenbinario = "";
                if (Charenbinario.Length < 8)
                {
                    string aux = "";
                    for (int m = 0; m < 8 - Charenbinario.Length; m++)
                    {

                        aux += "0";

                    }

                    caracterenbinario += aux + Charenbinario;
                    aux = "";
                }
                else
                {
                    caracterenbinario = Charenbinario;
                }

                //primer permutacion
                string MensajeIP = IP(IPQ, caracterenbinario);
              
                //separacion izquierda 1
                string izquierda = MensajeIP.Substring(0, 4);
                //separacion derecha 1
                string derecha = MensajeIP.Substring(4, 4);

                //EXPANDIR Y PERMUTAR
                string MensajeEp = EP(EPQ, derecha);


                //suma
                string suma1 = Suma(MensajeEp, keys[0]);
                //suma izquierda
                string suma1Izquierda = suma1.Substring(0, 4);
                //suma derecha
                string suma1derecha = suma1.Substring(4, 4);

                //FILAS Y COLUMNAS
                string  combinacion = suma1Izquierda.Substring(0, 1) + suma1Izquierda.Substring(3, 1);

                int fila = Convert.ToInt32(combinacion, 2);
               
                combinacion = suma1Izquierda.Substring(1, 1) + suma1Izquierda.Substring(2, 1);
                int col = Convert.ToInt32(combinacion, 2);
            
                combinacion = suma1derecha.Substring(0, 1) + suma1derecha.Substring(3, 1);
                int fila1 = Convert.ToInt32(combinacion, 2);
              
                combinacion = suma1derecha.Substring(1, 1) + suma1derecha.Substring(2, 1);
                int col2 = Convert.ToInt32(combinacion, 2);
                string Scombinada = So[fila, col] + S1[fila1, col2];

                //PERMTUACION 4
                string mensajeP4 = P4(P4Q, Scombinada);
                //SUMA
                suma1 = Suma(mensajeP4, izquierda);
               
                //SWAP MENSAJE
                string mensajesw = derecha + suma1;

                //SEPARACION IZQUIERDA, DERCHA
                 izquierda = mensajesw.Substring(0, 4);
                 derecha = mensajesw.Substring(4, 4);


                string MensaEP2 = EP(EPQ, derecha);

                //SUMA
                suma1 = Suma(MensaEP2, keys[1]);

                // SEPARADOR DE SUMA
                suma1Izquierda = suma1.Substring(0, 4);
                suma1derecha = suma1.Substring(4, 4);

                //FILAS Y COLUMNAS
                combinacion = suma1Izquierda.Substring(0, 1) + suma1Izquierda.Substring(3, 1);
                fila = Convert.ToInt32(combinacion,  2);
                combinacion = suma1Izquierda.Substring(1, 1) + suma1Izquierda.Substring(2, 1);
                col = Convert.ToInt32(combinacion,  2);
                combinacion = suma1derecha.Substring(0, 1) + suma1derecha.Substring(3, 1);

                fila1 = Convert.ToInt32(combinacion,  2);
                combinacion = suma1derecha.Substring(1, 1) + suma1derecha.Substring(2, 1);
                col2 = Convert.ToInt32(combinacion, 2);
               
                Scombinada = So[fila, col] + S1[fila1, col2];

                string mensajeP42 = P4(P4Q, Scombinada);

                // SUMA1 
                suma1 = Suma(izquierda, mensajeP42);

                //PERMUTACION IP-1
                string mensajepi = IP1(IP1Q, suma1 + derecha);

                int mensajefinal = Convert.ToInt32(mensajepi, 2);
                Mensajecifrado.Add((byte)mensajefinal);
            }


            return Mensajecifrado;
        }
        public List<byte> Decypher(string llave, byte[] mensaje, int[] P4Q, int[] EPQ, int[] IPQ, int[] IP1Q, int[] P10, int[] P8)
        {
            string[,] So = { { "01","00","11","10"},
                             { "11","10","01","00"},
                             { "00","10","01","11"},
                             { "11","01","11","10"} };

            string[,] S1 = { { "00","01","10","11"},
                             { "10","00","01","11"},
                             { "11","00","01","00"},
                             { "10","01","00","11"} };

            List<byte> Mensajecifrado = new List<byte>();

            string[] keys = GenerarLlave(llave, P10, P8);

            for (int i = 0; i < mensaje.Length; i++)
            {



                string Charenbinario = Convert.ToString(mensaje[i], 2);

                string caracterenbinario = "";
                if (Charenbinario.Length < 8)
                {
                    string aux = "";
                    for (int m = 0; m < 8 - Charenbinario.Length; m++)
                    {

                        aux += "0";

                    }

                    caracterenbinario += aux + Charenbinario;
                    aux = "";
                }
                else
                {
                    caracterenbinario = Charenbinario;
                }

                //primer permutacion
                string MensajeIP = IP(IPQ, caracterenbinario);

                //separacion izquierda 1
                string izquierda = MensajeIP.Substring(0, 4);
                //separacion derecha 1
                string derecha = MensajeIP.Substring(4, 4);

                //EXPANDIR Y PERMUTAR
                string MensajeEp = EP(EPQ, derecha);


                //suma
                string suma1 = Suma(MensajeEp, keys[1]);
                //suma izquierda
                string suma1Izquierda = suma1.Substring(0, 4);
                //suma derecha
                string suma1derecha = suma1.Substring(4, 4);

                //FILAS Y COLUMNAS
                string combinacion = suma1Izquierda.Substring(0, 1) + suma1Izquierda.Substring(3, 1);

                int fila = Convert.ToInt32(combinacion, 2);

                combinacion = suma1Izquierda.Substring(1, 1) + suma1Izquierda.Substring(2, 1);
                int col = Convert.ToInt32(combinacion, 2);

                combinacion = suma1derecha.Substring(0, 1) + suma1derecha.Substring(3, 1);
                int fila1 = Convert.ToInt32(combinacion, 2);

                combinacion = suma1derecha.Substring(1, 1) + suma1derecha.Substring(2, 1);
                int col2 = Convert.ToInt32(combinacion, 2);
                string Scombinada = So[fila, col] + S1[fila1, col2];

                //PERMTUACION 4
                string mensajeP4 = P4(P4Q, Scombinada);
                //SUMA
                suma1 = Suma(mensajeP4, izquierda);

                //SWAP MENSAJE
                string mensajesw = derecha + suma1;

                //SEPARACION IZQUIERDA, DERCHA
                izquierda = mensajesw.Substring(0, 4);
                derecha = mensajesw.Substring(4, 4);


                string MensaEP2 = EP(EPQ, derecha);

                //SUMA
                suma1 = Suma(MensaEP2, keys[0]);

                // SEPARADOR DE SUMA
                suma1Izquierda = suma1.Substring(0, 4);
                suma1derecha = suma1.Substring(4, 4);

                //FILAS Y COLUMNAS
                combinacion = suma1Izquierda.Substring(0, 1) + suma1Izquierda.Substring(3, 1);
                fila = Convert.ToInt32(combinacion, 2);
                combinacion = suma1Izquierda.Substring(1, 1) + suma1Izquierda.Substring(2, 1);
                col = Convert.ToInt32(combinacion, 2);
                combinacion = suma1derecha.Substring(0, 1) + suma1derecha.Substring(3, 1);

                fila1 = Convert.ToInt32(combinacion, 2);
                combinacion = suma1derecha.Substring(1, 1) + suma1derecha.Substring(2, 1);
                col2 = Convert.ToInt32(combinacion, 2);

                Scombinada = So[fila, col] + S1[fila1, col2];

                string mensajeP42 = P4(P4Q, Scombinada);

                // SUMA1 
                suma1 = Suma(izquierda, mensajeP42);

                //PERMUTACION IP-1
                string mensajepi = IP1(IP1Q, suma1 + derecha);

                int mensajefinal = Convert.ToInt32(mensajepi, 2);
                Mensajecifrado.Add((byte)mensajefinal);
            }


            return Mensajecifrado;
        }
    }
}
