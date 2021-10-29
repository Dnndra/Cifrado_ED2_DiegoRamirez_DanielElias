using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using LibreriaRDCifrado;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Json;
using System.Web;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cifrado_ED2_DiegoRamirez_DanielElias.Controllers
{
    [Route("api")]
    [ApiController]
    public class CipherController : ControllerBase
    {
        public static string NombreOriginalActual;
        // GET: api/<CipherController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CipherController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CipherController>
        [HttpPost("cipher/{method}")]
        public async Task<FileResult> Cipher([FromRoute] string method, [FromForm] IFormFile File, [FromForm] string Key)
        {
         
            if (method == "cesar")
            {
                var cesar = new CifradoCesar();
                var clave = Key;
                var nombreArchivo = File.FileName.Split('.');
                var reader = new StreamReader(File.OpenReadStream());
                string texto = reader.ReadToEnd();
                reader.Close();
                string codificado = cesar.Cipher(texto, clave);
                byte[] bytearray = Encoding.UTF8.GetBytes(codificado);
                return base.File(bytearray, "compressedFile / csr", nombreArchivo[0] + ".csr");

            }
            else if (method == "zigzag")
            {
                var zigzag = new CifradoZigZag();
                var clave = Key;
                var nombreArchivo = File.FileName.Split('.');
                var reader = new StreamReader(File.OpenReadStream());
                string texto = reader.ReadToEnd();
                reader.Close();
                string codificado = zigzag.Cipher(texto, clave);
                byte[] bytearray = Encoding.UTF8.GetBytes(codificado);
                return base.File(bytearray, "compressedFile / zz", nombreArchivo[0] + ".zz");
            }
           
            return null;
        }

        

        [HttpPost("sdes/cipher/{name}")]
        
          public async Task<FileResult> CipherSDES([FromRoute] string name, [FromForm] IFormFile File, [FromForm] string Key)
        {
            try
            {
                if (Convert.ToInt32(Key) > 1023)
                {
                    return default;
                }
            }
            catch
            {

            }
         
            //LEER PERMUTACIONES
            int[] P10 = new int[10];
            int[] P8 = new int[8];
            int[] P4 = new int[4];
            int[] EP = new int[8];
            int[] IP = new int[8];
            int[] IP_1 = new int[8];
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\Permutations.txt");
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
            //fin leer permutaciones


            byte[] bytes;
; 
         
            using (var memory = new MemoryStream())
            {
                await File.CopyToAsync(memory);
             
            
                bytes = memory.ToArray();
              
             
             }
            var SDES = new SDES();
            NombreOriginalActual = File.FileName;

           List<byte> final= SDES.Cypher(Key ,bytes, P4,EP,IP,IP_1,P10,P8);


            return base.File(final.ToArray(), "compressedFile    / sdes", name + ".sdes");

          }

        [HttpPost("sdes/decipher")]

        public async Task<FileResult> DecipherSDES([FromForm] IFormFile File, [FromForm] string Key)
        {
            try
            {
                if (Convert.ToInt32(Key) > 1023)
                {
                    return default;
                }
            }
            catch
            {

            }
            //LEER PERMUTACIONES
            int[] P10 = new int[10];
            int[] P8 = new int[8];
            int[] P4 = new int[4];
            int[] EP = new int[8];
            int[] IP = new int[8];
            int[] IP_1 = new int[8];
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\Permutations.txt");
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
            //fin leer permutaciones
            byte[] bytes;


            using (var memory = new MemoryStream())
            {
                await File.CopyToAsync(memory);


                bytes = memory.ToArray();
              

            }
            var SDES = new SDES();

            List<byte> final = SDES.Decypher(Key, bytes, P4, EP, IP, IP_1,P10,P8);
            string[] nombreArchivo = NombreOriginalActual.Split('.');
     
            return base.File(final.ToArray(), "text / plain", nombreArchivo[0]+".txt" );

        }


        [HttpPost("rsa/{name}/{p}/{q}")]
        public async Task<FileResult> RSA([FromForm] IFormFile File, [FromRoute] string name, [FromRoute] string p, [FromRoute] string q)
        {
            byte[] bytes;
            var RSA = new RSA();

            using (var memory = new MemoryStream())
            {
                await File.CopyToAsync(memory);


                bytes = memory.ToArray();


            }
            List<int> llaves = RSA.GenerarLlaves(Convert.ToInt32(p), Convert.ToInt32(q));
            byte[] mensaje = RSA.RSA_ALGORITHM(bytes, llaves);
            return base.File(mensaje, "text/ plain", name + ".txt");

        }




        [HttpPost("decipher")]
        public async Task<FileResult> DecipherAsync([FromForm] IFormFile File, [FromForm] string Key)
        {
          

            byte[] bytes;
            var cesar = new CifradoCesar();
            var zigzag = new CifradoZigZag();
            var clave = Key;
            var nombreArchivo = File.FileName.Split('.');
            if (nombreArchivo[1] == "csr")
            {
                using (var memory = new MemoryStream())
                {
                    await File.CopyToAsync(memory);


                    bytes = memory.ToArray();
                    List<byte> aux = bytes.OfType<byte>().ToList();

                }
                string codificado = Encoding.UTF8.GetString(bytes);
                string mensaje = cesar.Decipher(codificado, Key);
                byte[] bytearray = Encoding.UTF8.GetBytes(mensaje);

                return base.File(bytearray, "text/plain", nombreArchivo[0] + ".txt");
            }
            else if (nombreArchivo[1] == "zz")
            {
                using (var memory = new MemoryStream())
                {
                    await File.CopyToAsync(memory);


                    bytes = memory.ToArray();
                    List<byte> aux = bytes.OfType<byte>().ToList();

                }
                string codificado = Encoding.UTF8.GetString(bytes);
                string mensaje = zigzag.Decipher(codificado, Key);
                byte[] bytearray = Encoding.UTF8.GetBytes(mensaje);
                return base.File(bytearray, "text/plain", nombreArchivo[0] + ".txt");
            }

            return null;
        }


    }
}
