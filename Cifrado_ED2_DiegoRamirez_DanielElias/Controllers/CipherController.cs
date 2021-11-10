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
using Ionic.Zip;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Net.Http;
using ZipFile = Ionic.Zip.ZipFile;
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

        [HttpGet("rsa/keys/{p}/{q}")]
        public ActionResult Llaves([FromRoute] string p, [FromRoute] string q)
        {
            var RSA = new RSA();

            string workingDirectory = Environment.CurrentDirectory;
            string pathFolderActual = Directory.GetParent(workingDirectory).FullName;
            string pathDirectoryKeys = pathFolderActual + "\\Keys\\";
            string rutaKeyPublica = "";
            string rutaKeyPrivada = "";

            rutaKeyPublica = pathDirectoryKeys + "public.key";

            if (Directory.Exists(pathDirectoryKeys))
            {
                Directory.Delete(pathDirectoryKeys, true);
            }
            Directory.CreateDirectory(pathDirectoryKeys);
            if (RSA.EsPrimo(Convert.ToInt32(p)) == true && RSA.EsPrimo(Convert.ToInt32(q)) == true && Convert.ToInt32(p) * Convert.ToInt32(q) > 256 && Convert.ToInt32(p) * Convert.ToInt32(q) < Int32.MaxValue)
            {


                RSA.GenerarLlaves(Convert.ToInt32(p), Convert.ToInt32(q));
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                rutaKeyPublica = pathDirectoryKeys + "public.key";
                System.IO.File.WriteAllBytes(rutaKeyPublica, System.Text.Encoding.UTF8.GetBytes(RSA.public_key));
                rutaKeyPrivada = pathDirectoryKeys + "private.key";
                System.IO.File.WriteAllBytes(rutaKeyPrivada, System.Text.Encoding.UTF8.GetBytes(RSA.private_key));

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddEntry("public.key", System.IO.File.ReadAllBytes(rutaKeyPublica));
                    zip.AddEntry("private.key", System.IO.File.ReadAllBytes(rutaKeyPrivada));

                    using (MemoryStream output = new MemoryStream())
                    {
                        zip.Save(output);
                        //dar send and download para escoger la ruta o esta en la carpeta del proyecto
                        return File(output.ToArray(), "application/ zip", "keys.zip");
                    }
                }
            }
            Response.Clear();
            Response.StatusCode = 500; return Content("Error!" + "\n" + "P y Q tienen que ser numeros primos y el producto de estos dos mayor a 256");

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
  
       
   
       
       
        [HttpPost("rsa/{name}")]
        public async Task<FileResult> RSA([FromForm] IFormFile File, [FromForm] IFormFile File2, [FromRoute] string name)
        {
            byte[] bytes;
            var reader = new StreamReader(File2.OpenReadStream());
            var RSA= new RSA();
            string linea = reader.ReadLine();
            string[] llave = linea.Split(',');
            string ORIGINALFILENAME = File2.FileName;

            using (var memory = new MemoryStream())
            {
                await File.CopyToAsync(memory);
                
                bytes = memory.ToArray();


            }
           
            if (bytes[0] == 0)
            {
              
                byte[] mensaje = RSA.RSA_DECYPHER(bytes, Convert.ToInt32(llave[0]), Convert.ToInt32(llave[1]));

                return base.File(mensaje, "text/ plain", name + ".txt");
            }
            else 
            {
                byte[] mensaje = RSA.RSA_CYPHER(bytes, Convert.ToInt32(llave[0]), Convert.ToInt32(llave[1]));

                return base.File(mensaje, "text/ plain", name + ".txt");
            }


            return null;

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
