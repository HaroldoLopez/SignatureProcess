namespace signatureProcess.Entities
{
  public class SignatureRequest
  {
      public List<Firmante>? lstFirmantes { get; set; }
  }

  public class Firmante
  {
      public string? CIF { get; set; }
      public string? NombreFirmante { get; set; }
      public string? Relacion { get; set; }
      public string? TipoFirma { get; set; }
      public string? Firma { get; set; }
  }
}