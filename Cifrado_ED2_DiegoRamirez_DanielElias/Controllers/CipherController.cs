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
            byte[] bytes;
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

        //public int[] P10;
        //public int[] P8;
        //public int[] P8;

        void ObtenerPermutaciones (int[] P10, int[] P8, int[] P4, int[] EP, int[] IP, int[] IP_1)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\Permutations.txt");
            var reader = new StreamReader(path);
            string linea = reader.ReadLine();
            string[]PACTUAL = linea.Split(',');

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
        
        public static int[] P10;
        public static int[] P8;
        public static int[] P4;
        public static int[] EP;
        public static int[] IP;
        public static int[] IP_1;







        [HttpPost("sdes/cipher/{name}")]
        
          public async Task<FileResult> CipherSDES([FromRoute] string name, [FromForm] IFormFile File, [FromForm] string Key)
        {

            //LEER PERMUTACIONES
            int[] P10 = new int[10];
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
            //fin leer permutaciones


            byte[] bytes;
           

            using (var memory = new MemoryStream())
            {
                await File.CopyToAsync(memory);

            
                bytes = memory.ToArray();
                List<byte> aux = bytes.OfType<byte>().ToList();
             
             }
            var SDES = new SDES();

           List<byte> final= SDES.Cypher(Key ,bytes, null, null, null, null);


            return base.File(final.ToArray(), "text / plain", name + ".txt");

          }

        [HttpPost("sdes/decipher/{name}")]

        public async Task<FileResult> DecipherSDES([FromRoute] string name, [FromForm] IFormFile File, [FromForm] string Key)
        {
            //LEER PERMUTACIONES
            int[] P10 = new int[10];
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
            //fin leer permutaciones
            byte[] bytes;


            using (var memory = new MemoryStream())
            {
                await File.CopyToAsync(memory);


                bytes = memory.ToArray();
                List<byte> aux = bytes.OfType<byte>().ToList();

            }
            var SDES = new SDES();

            List<byte> final = SDES.Decypher(Key, bytes, null, null, null, null);

     
            return base.File(final.ToArray(), "text / plain", name + ".txt");

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
