using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CryptoQuoteApp20.Services;

namespace CryptoQuoteApp20.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CryptoController : ControllerBase
    {
        private readonly CryptoService _cryptoService;

        public CryptoController(CryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        [HttpGet("{cryptoCode}")]
        public async Task<IActionResult> GetCryptoQuote(string cryptoCode)
        {
            var quotes = await _cryptoService.GetCryptoQuote(cryptoCode.ToUpper());
            return Ok(quotes);
        }


        //[HttpGet("{GetCryptoPriceAsync}")]
        //public async Task<IActionResult> GetCryptoPriceAsync(string cryptoCode)
        //{
        //    var quotes = await _cryptoService.GetCryptoPriceAsync(cryptoCode.ToUpper());
        //    return Ok(quotes);
        //}
    }
}
