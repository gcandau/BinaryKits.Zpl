using BinaryKits.Zpl.Label;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Models;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class FieldBlockZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public FieldBlockZplCommandAnalyzer() : base("^FB")        { }

        ///<inheritdoc/>
        public override ZplElementBase Analyze(string zplCommand, VirtualPrinter virtualPrinter, IPrinterStorage printerStorage)
        {
            string[] zplDataParts = this.SplitCommand(zplCommand);

            int tmpint;
            int widthOfTextBlockLine = 0;
            int maximumNumberOfLinesInTextBlock = 1;
            int addOrDeleteSpaceBetweenLines = 0;
            TextJustification textJustification = TextJustification.Left;
            int hangingIndentOfTheSecondAndRemainingLines = 0;

            if (zplDataParts.Length > 0 && TryParseInt(zplDataParts[0], out tmpint))
            {
                widthOfTextBlockLine = tmpint;
            }

            if (zplDataParts.Length > 1 && TryParseInt(zplDataParts[1], out tmpint))
            {
                maximumNumberOfLinesInTextBlock = tmpint;
            }

            if (zplDataParts.Length > 2 && TryParseInt(zplDataParts[2], out tmpint))
            {
                addOrDeleteSpaceBetweenLines = tmpint;
            }

            if (zplDataParts.Length > 3)
            {
                switch (zplDataParts[3])
                {
                    case "C":
                        textJustification = TextJustification.Center;
                        break;
                    case "R":
                        textJustification = TextJustification.Right;
                        break;
                    case "J":
                        textJustification = TextJustification.Justified;
                        break;
                }
            }

            if (zplDataParts.Length > 4 && TryParseInt(zplDataParts[4], out tmpint))
            {
                hangingIndentOfTheSecondAndRemainingLines = tmpint;
            }

            virtualPrinter.SetNextElementFieldBlock(new FieldBlock
            {
                WidthOfTextBlockLine = widthOfTextBlockLine,
                MaximumNumberOfLinesInTextBlock = maximumNumberOfLinesInTextBlock,
                AddOrDeleteSpaceBetweenLines = addOrDeleteSpaceBetweenLines,
                TextJustification = textJustification,
                HangingIndentOfTheSecondAndRemainingLines = hangingIndentOfTheSecondAndRemainingLines
            });

            return null;
        }
    }
}
