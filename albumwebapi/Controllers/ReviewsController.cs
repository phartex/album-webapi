using albumwebapi.Interfacers;
using albumwebapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;

namespace albumwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly RSA _rsa;
     

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
            _rsa = RSA.Create(2048);
            string privateKeyBase64 = "MIIEpAIBAAKCAQEAwmuVRFpaFshvmRTCrMtmKYFjEcOshexEe1URIEqN7O0fcdBeNeAoXd2uIvb9Y9nMoy7HR9Iqx4CBcTJozzMlKB2PYMG4/YW0xV5RgsywQQABG6/c0E0J2sW1VLGtmXaMDQln+bnhxpNmJDpXPoNwSC0hWxF6Diory63/49eVHX/phC35uKmjBEGcvL66VuhXe+QSdzRdgEB1HdTDaqNCsFV8NBKIV9jipUgacI3MmwFg2DBV3iPl436girQoToCds4oWPLEc1Yrq1vkGOrj+RU4EBh0xp3lUmZdtQclTjm3zy9aNvofpjuS0Sy7IYa/KYxD9gafjEsMDQaHBCG1xlQIDAQABAoIBAG9S1teavLcK8gu/fXEfzlG5ypZjaqgMj3HnVbUdu4KMjFLJMwi6X6LPymQu9Qx1q91MbcNMLj6p8HR2ntj1ujLsKisHAfv40Xep+BZjShEOBZRq8I89bYUdbCUgz/xtiuf0GNs/em4P/I6F8WrULDjHzq+spCz0LLul2D4INVP76KMwgPo2rvOWtIr5fI1+E/fXpXj8kYwt/z8UKPNtkfRApFkygw8pVdro0sdb1qPPzv9RzIslBJaZXD/4mJ+qTTrE7Be8e0IVteG+HDYMFmYqZaT4zF4jexFeZ3yWIoLn+8cfPYpCWci29J3MvT6kpLrzKJ48sowZnjZEVh92AUkCgYEA4YM9OkrSGZ3d1SvCFxMwPheAn/uXgKV4Rdq6GQj2+BBrvZA1zKvWdr9+5nBy6dAj4nUjj/sW9km92QkUVPMTg4ngf9miOFLokqsJ1lIbFVvvkxH+qULQ1wccudh/LoGx3m25QfTEcm9F0GifDc0ikoBtwnBpRsjhggxzMDcqnY8CgYEA3LREiJ3iY6i+BOzPV+Obfd29XXCPmOOuTlzhqUd5JRPzPJn27XXsJdIs0bVtil5vLW4cEZcgGACySKLzVfDceJWuEgXyzoaxhEI6fO1gYvROTUP5TZvnOv94XKSkn5eHy6RqFEnMY1wjzLnoN0MgMcMRAyUu1TSU5+ftMfrxNJsCgYBZ1Yj/fqzBbaTf9WWFAazXH0+q63OH2OLXmPFHZc3UNd5ljlwQI2f0uAYaUDNGtxaEcLtw8MpN5ERPgBFsYcSlQrh+1biMjWJ+gsoRaXXGhXAjeyiiTq6Y6glkczz+zsWHYNhK0PGs0GzUP8kUm2IpUCXpLhSSERXcU49TZraAOwKBgQCpDgxQ+V8oJ2EmNJ6+G8JHL0qWAFp+KiiiLdNM70qEdDuk9+qFCgbREaTzHZ3Cl4NhsS4e6zvSJvZpXSg3dD/svfqQI65RNCtVu8VBXDKwmaJA3QhhwkHklzuC+zp+ZHvIKTB4Se8dTL6/WsfdTLWkaF7nodu1xIkq+iReXHHWcQKBgQCnZ8ad+zLUWA7xzNzbbzOaSXLaGyrziXTX0O8Sa0xMSIbYHDBXKPx/RA4ngH6a/EFUAnyu6T4Yeouo+JLJePFgn31uG4lTREBoYWLAIFxXiqdNoVL5VNL/fnu8FybHPBS1DSz0/JcitJtt8N8qFLVsvurPFnLyWYWzsRVy8ytbMg==";
            byte[] privateKeyBytes = Convert.FromBase64String(privateKeyBase64);
            _rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

        }

        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _reviewService.GetReviewsAsync();
            return Ok(reviews);
        }

        //[HttpPost]
        //public async Task<IActionResult> AddReview([FromBody] Review review)
        //{
        //    var createdReview = await _reviewService.AddReviewAsync(review);
        //    return CreatedAtAction(nameof(GetReviews), createdReview);
        //}


        [HttpGet("public-key")]
        public IActionResult GetPublicKey()
        {
            using var rsa = RSA.Create(2048);

            // Export keys
            var privateKey = rsa.ExportRSAPrivateKey();
            var publicKey = rsa.ExportRSAPublicKey();

            return Ok(new { publicKey = publicKey, privatekey = privateKey });
        }



        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] CreateReviewDto encryptedDto)
        {
            try
            {
                // Encrypt a string using RSA public key
                string plaintext = "Hello, World!";
                byte[] encryptedData = _rsa.Encrypt(Encoding.UTF8.GetBytes(plaintext), RSAEncryptionPadding.Pkcs1);
                string encryptedBase64 = Convert.ToBase64String(encryptedData);

                // Now decrypt using the private key (the method you're using)
                byte[] decryptedData = _rsa.Decrypt(Convert.FromBase64String(encryptedBase64), RSAEncryptionPadding.Pkcs1);
                string decryptedText = Encoding.UTF8.GetString(decryptedData);

                Console.WriteLine("Original: " + plaintext);
                Console.WriteLine("Encrypted (Base64): " + encryptedBase64);
                Console.WriteLine("Decrypted: " + decryptedText);

             

                // Decrypt the encrypted fields

                string decryptedName = DecryptField(encryptedDto.Name);
                string decryptedContent = DecryptField(encryptedDto.Content);
               

                // Map to the review model
                var review = new Review
                {
                    Name = decryptedName,
                    Content = decryptedContent,
                    CreatedAt = DateTime.UtcNow
                };

                // Add the review to the service
                var createdReview = await _reviewService.AddReviewAsync(review);

                // Return the created review with a 201 status code
                return CreatedAtAction(nameof(GetReviews), new { id = createdReview.Id }, createdReview);
            }
            catch (CryptographicException ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var result = await _reviewService.DeleteReviewAsync(id);
            if (result == 0)
            {
                // If no rows were affected, return NotFound
                var notFoundResponse = new DeleteMethodResponse(404, "Review not found");
                return NotFound(notFoundResponse);
            }

            var successResponse = new DeleteMethodResponse(204, "Review deleted successfully");

            // Return a status of 204 (No Content) to indicate successful deletion
            return Ok(successResponse);
        }

        private string DecryptField2(string DecryptField)
        {
            byte[] encryptedBytes = Convert.FromBase64String("bV1NVwRHRYodeXUCN0bMnoTfbLfD6QedTJJD7CTuY1GQiLmZfCYJ93r8S5O9DivZeV+rSelgiFoTSDHsuCMjzcTc0P4jfjxzKG6gl3c9usYeiUFKqzBy+2vfudt5dIo1rQqirlVHikIwdrfGrFQlcUuqwsXWUUUxi9GaFA3C9bDE346Qjm4QoOnczGNVp4W8FX2SvFio1IS8yLS30kY/z9Ow5ATAuor9ApGKIUVRVpfHq/KLBhlUrZKypZ5csE0OFai+rZszCnedqv8c/JxRKDEXwZGGUqBBSAMOxMXB935SA3040Ki8YJpy0MWtScnhJLPEmcXBgejPFtLa6mur1Q==");
            byte[] decryptedBytes = _rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public static string DecryptField(string cypherText)
        {
            string plainTextData = string.Empty;

            string privKeyBase64 = "MIIEpAIBAAKCAQEAudg3E4nJO4Q6ZNTfEn85+WwqhsorBwmDX3FVvAy+BG26AXvq+3LSo3XLuYPYJY9R7usSRT/HfiK3tBx93xidauQfQTY4T2PIX4NyUss389pTPq4QTRzDySyGCM3cRMltm02tiW0BlLfvXNXOzDWz/mZY2k5c5bxlP0scBUt25qtg5jQBHHw9ZPt1dX3DdRrenVFoQjoLT2eUI//PYytg7ICgCs8ZbkHQvrRc5S/MVMdg1um5MalsZtGVdKlhZ3t7Fi+4cbSWsdImcZhHh009b5gPL/va9RHbwdvCLWs2stbLxgqBxag4e6vuUmwDsXtQBFkSlEWSKp4bxkZla51yDQIDAQABAoIBAQCCjcCLKjVCaS54XQ5kORop/PGXBPjbFbTDXKPE2hK2m7qahknZ4JzWlc7LATLbl+YP9/U6zoY/NvZkQcuu6OTVDXJjZkqCgu9lIbszLG2fEdV+59qtwTJM5ck28B7396TW/+eT7g65QcmWJuEClgtfANkUaw5ZmVJq90G0+oGJotFkfzjwhDDpoGtkPSMCr4Ebg0FdImaSipaXukrb5qps6+tkRuzVn1mYkvZe+b1YvAohCxFtCoYTrQJh6Uy/k600g735gptcOytLvsu9gUy8P+fEJN5lbKCCHupfvB7PynWO6XJfazlP9FYp93AfFI0RzwGscLnrDN0cKeMQtNfFAoGBAO5IA07BTkBlyjjkipTsn2doKBkJN9W3jE2onmFeCc+ujvICWE/lJgjMiCz6HS0UoG09q/OjqukARKFhOUr9Px0fZOCIVCe2w1d9g8fkhXDxSAyOFNV9MNioLSjLpaslqW1oV7QG/n990B55Ebb5Foie25dITH11JVIENz8+sasjAoGBAMeqALwk5EgK2z5JS1pwmkWhT6ZahelXoyy2e9sG7gfiFpPb0pF7j+ZZ6tiUbD0pIpTuOhf18UjAuvjJPu3Vyk4/var4YZoZBTxhCaFyll1V/WGRDl66uilHtw86RGCNdwEhiKXfBCeShrUvSuKvJfu5blwk8+QVYW7ghQZjahkPAoGAU8T9ZLKQ0BL5BQdTsQd/AtubPTIAbyIYMTUWeSSWTm0P1AT/BxeIR4+gUC+eEjsuKzmDOiRP8ZomhpWnjrV5esOOs2E6NLwL8LYvnha40vwAWGma6XMZuRFmzvDJTWHwLoaATsomrquMfyoREEKBqYYvrP+95F1KwYq54YIo9AECgYEAwt2engF7cvx1gdLS4k+noXGQZRFQRK6tqMPpGZkn0zXLRz9xwV9Q0EbNt9cT1JVDFQt0U1JLzO+dC5aN/l17ducq26RSzazBuW4TkihdrHZyNzj1R2sCqas+dHQvq/QlQ0tRLGH+kgilxEuF0LuUXJtbpD7EccSUNTyXV+bK8+0CgYB8o+17mgoeRgbFUfqXRoKbvNc30v7hWNzzkQbTcKJHXWWb4Bf+Y1cwLlWisTmnSFA/m04u0lecOB+Zxo+k8y2Yy2PJhrQS6bb3+X2mXkVMfdkwUTlQVxtMSAIqvQH8pUQXja/LfKHuI7otcyi7syXWklpgpdSy7RraAcnswGgk6w==; //get value from session/temp storage";

            string privKeyString = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(privKeyBase64));

            var sr = new System.IO.StringReader(privKeyString);
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            var privKey = (RSAParameters)xs.Deserialize(sr);

            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(privKey);

            var bytesCypherText = Convert.FromBase64String(cypherText);

            //Problematic line
            var bytesPlainTextData = csp.Decrypt(bytesCypherText, RSAEncryptionPadding.Pkcs1);

            plainTextData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);

            return plainTextData;
        }

        private const string PrivateKeyBase64 = "MIIEpAIBAAKCAQEAwmuVRFpaFshvmRTCrMtmKYFjEcOshexEe1URIEqN7O0fcdBeNeAoXd2uIvb9Y9nMoy7HR9Iqx4CBcTJozzMlKB2PYMG4/YW0xV5RgsywQQABG6/c0E0J2sW1VLGtmXaMDQln+bnhxpNmJDpXPoNwSC0hWxF6Diory63/49eVHX/phC35uKmjBEGcvL66VuhXe+QSdzRdgEB1HdTDaqNCsFV8NBKIV9jipUgacI3MmwFg2DBV3iPl436girQoToCds4oWPLEc1Yrq1vkGOrj+RU4EBh0xp3lUmZdtQclTjm3zy9aNvofpjuS0Sy7IYa/KYxD9gafjEsMDQaHBCG1xlQIDAQABAoIBAG9S1teavLcK8gu/fXEfzlG5ypZjaqgMj3HnVbUdu4KMjFLJMwi6X6LPymQu9Qx1q91MbcNMLj6p8HR2ntj1ujLsKisHAfv40Xep+BZjShEOBZRq8I89bYUdbCUgz/xtiuf0GNs/em4P/I6F8WrULDjHzq+spCz0LLul2D4INVP76KMwgPo2rvOWtIr5fI1+E/fXpXj8kYwt/z8UKPNtkfRApFkygw8pVdro0sdb1qPPzv9RzIslBJaZXD/4mJ+qTTrE7Be8e0IVteG+HDYMFmYqZaT4zF4jexFeZ3yWIoLn+8cfPYpCWci29J3MvT6kpLrzKJ48sowZnjZEVh92AUkCgYEA4YM9OkrSGZ3d1SvCFxMwPheAn/uXgKV4Rdq6GQj2+BBrvZA1zKvWdr9+5nBy6dAj4nUjj/sW9km92QkUVPMTg4ngf9miOFLokqsJ1lIbFVvvkxH+qULQ1wccudh/LoGx3m25QfTEcm9F0GifDc0ikoBtwnBpRsjhggxzMDcqnY8CgYEA3LREiJ3iY6i+BOzPV+Obfd29XXCPmOOuTlzhqUd5JRPzPJn27XXsJdIs0bVtil5vLW4cEZcgGACySKLzVfDceJWuEgXyzoaxhEI6fO1gYvROTUP5TZvnOv94XKSkn5eHy6RqFEnMY1wjzLnoN0MgMcMRAyUu1TSU5+ftMfrxNJsCgYBZ1Yj/fqzBbaTf9WWFAazXH0+q63OH2OLXmPFHZc3UNd5ljlwQI2f0uAYaUDNGtxaEcLtw8MpN5ERPgBFsYcSlQrh+1biMjWJ+gsoRaXXGhXAjeyiiTq6Y6glkczz+zsWHYNhK0PGs0GzUP8kUm2IpUCXpLhSSERXcU49TZraAOwKBgQCpDgxQ+V8oJ2EmNJ6+G8JHL0qWAFp+KiiiLdNM70qEdDuk9+qFCgbREaTzHZ3Cl4NhsS4e6zvSJvZpXSg3dD/svfqQI65RNCtVu8VBXDKwmaJA3QhhwkHklzuC+zp+ZHvIKTB4Se8dTL6/WsfdTLWkaF7nodu1xIkq+iReXHHWcQKBgQCnZ8ad+zLUWA7xzNzbbzOaSXLaGyrziXTX0O8Sa0xMSIbYHDBXKPx/RA4ngH6a/EFUAnyu6T4Yeouo+JLJePFgn31uG4lTREBoYWLAIFxXiqdNoVL5VNL/fnu8FybHPBS1DSz0/JcitJtt8N8qFLVsvurPFnLyWYWzsRVy8ytbMg==";
    }


}