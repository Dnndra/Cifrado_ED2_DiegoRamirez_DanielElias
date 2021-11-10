using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaRDCifrado
{
    interface IEncriptador
    {
        public string Cipher(string mensaje, string clave);

        public string Decipher(string mensaje, string clave);

        public string[] GenerarLlave(string llave, int[] P10, int[] P8);

        public byte[] RSA_CYPHER(byte[] mensaje, int n, int k);

        public byte[] RSA_DECYPHER(byte[] mensaje, int n, int k);
    }
}
