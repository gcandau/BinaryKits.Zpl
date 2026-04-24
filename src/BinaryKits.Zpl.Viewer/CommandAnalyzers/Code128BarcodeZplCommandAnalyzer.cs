using System.Globalization;
using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class Code128BarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public Code128BarcodeZplCommandAnalyzer() : base("^BC") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            FieldOrientation fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0], virtualPrinter);

            int tmpint;
            int height = virtualPrinter.BarcodeInfo.Height;
            bool printInterpretationLine = true;
            bool printInterpretationLineAboveCode = false;
            bool uccCheckDigit = false;
            string mode = "N";

            if (zplDataParts.Length > 1 && TryParseInt(zplDataParts[1], out tmpint))
            {
                height = tmpint;
            }
            else if (zplDataParts.Length > 1
                && float.TryParse(zplDataParts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float tmpFloat))
            {
                // Tolerate decimal heights such as ^BCR,172.0,... (Labelary accepts these).
                height = (int)System.Math.Round(tmpFloat);
            }

            if (zplDataParts.Length > 2)
            {
                printInterpretationLine = this.ConvertBoolean(zplDataParts[2], "Y");
            }

            if (zplDataParts.Length > 3)
            {
                printInterpretationLineAboveCode = this.ConvertBoolean(zplDataParts[3]);
            }

            if (zplDataParts.Length > 4)
            {
                uccCheckDigit = this.ConvertBoolean(zplDataParts[4]);
            }

            if (zplDataParts.Length > 5)
            {
                mode = zplDataParts[5];
            }

            //The field data are processing in the FieldDataZplCommandAnalyzer
            virtualPrinter.SetNextElementFieldData(new Code128BarcodeFieldData
            {
                FieldOrientation = fieldOrientation,
                Height = height,
                PrintInterpretationLine = printInterpretationLine,
                PrintInterpretationLineAboveCode = printInterpretationLineAboveCode,
                UccCheckDigit = uccCheckDigit,
                Mode = mode
            });

            return null;
        }
    }
}
