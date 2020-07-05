namespace QRCodeReaderAPI.Models
{
    public class QRCodeInterpretationResponse
    {

        public string Type { get; set; }

        public QRCodeSymbol[] Symbol { get; set; }
    }
}
