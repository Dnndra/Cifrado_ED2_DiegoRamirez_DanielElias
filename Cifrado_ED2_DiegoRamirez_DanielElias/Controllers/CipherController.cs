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


        [HttpPost("sdes/cipher/{name}")]
        
          public async Task<FileResult> CipherSDES([FromRoute] string name, [FromForm] IFormFile File)
        {
            byte[] bytes;
           

            using (var memory = new MemoryStream())
            {
                await File.CopyToAsync(memory);

            
                bytes = memory.ToArray();
                List<byte> aux = bytes.OfType<byte>().ToList();
             
             }
            var SDES = new SDES();

           List<byte> final= SDES.Cypher("50",bytes, null, null, null, null);


            return base.File(final.ToArray(), "text / plain", name + ".txt");

          }

        [HttpPost("sdes/decipher/{name}")]

        public async Task<FileResult> DecipherSDES([FromRoute] string name, [FromForm] IFormFile File)
        {
            byte[] bytes;


            using (var memory = new MemoryStream())
            {
                await File.CopyToAsync(memory);


                bytes = memory.ToArray();
                List<byte> aux = bytes.OfType<byte>().ToList();

            }
            var SDES = new SDES();

            List<byte> final = SDES.Decypher("50", bytes, null, null, null, null);

     
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
