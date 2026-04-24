using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class AztecBarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        // The official ZPL II Aztec command is ^B0 (digit zero); some sources/printers
        // also use ^BO (letter O). Accept both spellings.
        public AztecBarcodeZplCommandAnalyzer() : base("^B0") { }

        ///<inheritdoc/>
        public override bool CanAnalyze(string zplLine)
        {
            return zplLine.StartsWith("^B0") || zplLine.StartsWith("^BO");
        }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            int tmpint;
            FieldOrientation fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0], virtualPrinter);
            int magnificationFactor = 2;
            bool extendedChannel = false;
            int errorControl = 0;
            bool menuSymbol = false;
            int symbolCount = 1;
            string idField = null;

            if (zplDataParts.Length > 1 && TryParseInt(zplDataParts[1], out tmpint))
            {
                magnificationFactor = tmpint;
            }

            if (zplDataParts.Length > 2)
            {
                extendedChannel = this.ConvertBoolean(zplDataParts[2]);
            }

            if (zplDataParts.Length > 3 && TryParseInt(zplDataParts[3], out tmpint))
            {
                errorControl = tmpint;
            }

            if (zplDataParts.Length > 4)
            {
                menuSymbol = this.ConvertBoolean(zplDataParts[4]);
            }

            if (zplDataParts.Length > 5 && TryParseInt(zplDataParts[5], out tmpint))
            {
                symbolCount = tmpint;
            }

            if (zplDataParts.Length > 6)
            {
                idField = zplDataParts[6];
            }

            virtualPrinter.SetNextElementFieldData(new AztecBarcodeFieldData
            {
                FieldOrientation = fieldOrientation,
                MagnificationFactor = magnificationFactor,
                ExtendedChannel = extendedChannel,
                ErrorControl = errorControl,
                MenuSymbol = menuSymbol,
                SymbolCount = symbolCount,
                IdField = idField
            });

            return null;
        }

    }
}
