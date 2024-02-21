using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using signatureProcess.Entities;
using signatureProcess.utils;

namespace signatureProcess.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SignatureController : ControllerBase
  {
      private readonly ILogger<SignatureController> _logger;

      public SignatureController(ILogger<SignatureController> logger)
      {
          _logger = logger;
      }

      [HttpPost(Name = "Signature")]
      public IActionResult Post([FromBody] SignatureRequest request)
      {
        try
        {
          if(request?.lstFirmantes != null && request.lstFirmantes.Count > 0)
          {
            foreach(var firmante in request.lstFirmantes)
            {
              if(firmante != null && firmante.Firma != null)
              {
                firmante.Firma = ImagesProcessing.ConvertToJpegBase64(firmante.Firma);
              }
            }
            return Ok(request);
          }
        }
        catch(JsonException e)
        {
          return BadRequest($"Error al decodificar el JSON: {e.Message}");
        }
      return BadRequest("Error al decodificar el JSON");
    }
  }
}