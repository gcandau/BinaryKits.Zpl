using BinaryKits.Zpl.Label.Elements;

using System.Globalization;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class BarCodeFieldDefaultZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public BarCodeFieldDefaultZplCommandAnalyzer() : base("^BY") { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            int tmpint;
            double tmpdbl;

            if (zplDataParts.Length > 0 && int.TryParse(zplDataParts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out tmpint))
            {
                // TODO: add validation message: between 1 and 10
                if (tmpint < 1)
                {
                    tmpint = 1;
                }
                else if (tmpint > 10)
                {
                    tmpint = 10;
                }

                virtualPrinter.SetBarcodeModuleWidth(tmpint);
            }

            if (zplDataParts.Length > 1)
            {
                // Be tolerant of non-standard suffixes such as "2.4:1" emitted by some label
                // generators. Only the leading numeric portion is meaningful for the ratio.
                string ratioRaw = zplDataParts[1];
                int colonIndex = ratioRaw.IndexOf(':');
                if (colonIndex >= 0)
                {
                    ratioRaw = ratioRaw.Substring(0, colonIndex);
                }

                if (double.TryParse(ratioRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out tmpdbl))
                {
                    // TODO: add validation message: between 2.0 and 3.0 in 0.1 increments
                    if (tmpdbl < 2.0)
                    {
                        tmpdbl = 2.0;
                    }
                    else if (tmpdbl > 3.0)
                    {
                        tmpdbl = 3.0;
                    }
                    else
                    {
                        tmpdbl = System.Math.Round(tmpdbl, 1);
                    }

                    virtualPrinter.SetBarcodeWideBarToNarrowBarWidthRatio(tmpdbl);
                }
            }

            if (zplDataParts.Length > 2 && int.TryParse(zplDataParts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out tmpint))
            {
                // TODO: add validation message: greater or equal than 1
                if (tmpint < 1)
                {
                    tmpint = 1;
                }

                virtualPrinter.SetBarcodeHeight(tmpint);
            }

            return null;
        }
    }
}
