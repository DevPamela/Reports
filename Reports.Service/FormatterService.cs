using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using Reports.Domain.Interfaces.Service;
using Reports.Service.Interfaces.Service;

namespace Reports.Service
{
    public class FormatterService : IFormatterService
    {
        public FormatterService()
        {
        }

        /// <inheritdoc />
        public async Task SetDefaultFormatForReport(XWPFParagraph paragraph, XWPFRun run, XWPFDocument doc)
        {
            await SetJustifyAlighment(paragraph);
            await lineSpacingOneAndHalf(paragraph);
            await SetDefaultMargin(doc);
            await SetFont(run);
            await SetFirstLineIndent(paragraph);
        }


        /// <inheritdoc />
        public async Task SetDefaultFormatForHeader(XWPFRun run, XWPFParagraph paragraph)
        {
            CT_RPr runProperties = run.GetCTR().AddNewRPr();
            CT_Fonts fonts = runProperties.AddNewRFonts();
            fonts.ascii = "Arial";
            fonts.hAnsi = "Arial";
            runProperties.AddNewSz().val = 24; // Tamanho da fonte em meio ponto (12pt * 2)
            runProperties.AddNewB().val = true; // Negrito
            runProperties.AddNewI().val = false;
            await SetCentralizedAlighment(paragraph);
        }

        /// <summary>
        /// Adds justified alignment to the paragraph.
        /// </summary>
        /// <param name="paragraph"></param>
        private async Task SetJustifyAlighment(XWPFParagraph paragraph)
        {
             paragraph.Alignment =  ParagraphAlignment.BOTH;
        }

        private async Task SetCentralizedAlighment(XWPFParagraph paragraph)
        {
            paragraph.Alignment = ParagraphAlignment.CENTER;
        }
        /// <summary>
        /// Set the line spacing to 1.5.
        /// </summary>
        /// <param name="paragraph"></param>
        private async Task lineSpacingOneAndHalf(XWPFParagraph paragraph)
        {
            CT_PPr paraProperties = paragraph.GetCTP().AddNewPPr();
            CT_Spacing spacing = paraProperties.AddNewSpacing();
            spacing.lineRule = ST_LineSpacingRule.auto;
            spacing.line = "360";
        }

        /// <summary>
        /// Format the document margin to 3 centimeters.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="application"></param>
        private async Task SetDefaultMargin(XWPFDocument doc)
        {
            CT_SectPr sectPr = doc.Document.body.AddNewSectPr();
            CT_PageMar pageMar = sectPr.AddPageMar();
            pageMar.left = 1440;
            pageMar.right = 1440;
            pageMar.top = 1440;
            pageMar.bottom = 1440;
        }
        /// <summary>
        /// Sets the paragraph font for Arial 12.
        /// </summary>
        /// <param name="paragraph"></param>
        public async Task SetFont(XWPFRun run)
        {
            CT_RPr runProperties = run.GetCTR().AddNewRPr();
            CT_Fonts fonts = runProperties.AddNewRFonts();
            fonts.ascii = "Arial";
            fonts.hAnsi = "Arial";
            runProperties.AddNewSz().val = 24; // Tamanho da fonte em meio ponto (12pt * 2)
            runProperties.AddNewB().val = false; // Negrito
            runProperties.AddNewI().val = false; // Itálico

        }
        /// <summary>
        /// Idents de first line of the paragraph.
        /// </summary>
        /// <param name="paragraph"></param>
        private async Task SetFirstLineIndent(XWPFParagraph paragraph)
        {
            CT_PPr paraProperties =  paragraph.GetCTP().AddNewPPr();
            CT_Ind ind = paraProperties.AddNewInd();
            ind.firstLine = 1440;
        }
    }
}