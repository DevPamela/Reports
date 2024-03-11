//using Microsoft.Office.Interop.Word;
//using Reports.Domain.Interfaces.Service;

//namespace Reports.Service
//{
//    public class FormatterService : IFormatterService
//    {
//        public FormatterService()
//        {
                
//        }
       
//        /// <inheritdoc />
//        public Paragraph SetDefaultFormatForReport(Paragraph paragraph, Application application, Document doc)
//        {
//            SetJustifyAlighment(paragraph);
//            lineSpacingOneAndHalf(paragraph);
//            SetDefaultMargin(doc, application);
//            SetFont(paragraph);
//            SetFirstLineIndent(paragraph);
//            return paragraph;
//        }

     
//        /// <inheritdoc />
//        public Paragraph SetDefaultFormatForHeader(Paragraph paragraph)
//        {
//            lineSpacingOneAndHalf(paragraph);
//            SetFont(paragraph);
//            return paragraph;
//        }

//        /// <summary>
//        /// Adds justified alignment to the paragraph.
//        /// </summary>
//        /// <param name="paragraph"></param>
//        private void SetJustifyAlighment(Paragraph paragraph)
//        {
//            paragraph.Format.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
//        }
//        /// <summary>
//        /// Set the line spacing to 1.5.
//        /// </summary>
//        /// <param name="paragraph"></param>
//        private void lineSpacingOneAndHalf(Paragraph paragraph)
//        {
//            paragraph.Space15();
//        }

//        /// <summary>
//        /// Format the document margin to 3 centimeters.
//        /// </summary>
//        /// <param name="doc"></param>
//        /// <param name="application"></param>
//        private void SetDefaultMargin(Document doc, Application application)
//        {
//            doc.PageSetup.TopMargin = application.CentimetersToPoints(3);
//            doc.PageSetup.BottomMargin = application.CentimetersToPoints(3);
//            doc.PageSetup.LeftMargin = application.CentimetersToPoints(3);
//            doc.PageSetup.RightMargin = application.CentimetersToPoints(3);
//        }
//        /// <summary>
//        /// Sets the paragraph font for Arial 12.
//        /// </summary>
//        /// <param name="paragraph"></param>
//        public void SetFont(Paragraph paragraph)
//        {
//            var range = paragraph.Range;
//            range.Font.Name = "Arial";
//            range.Font.Size = 12;
//            range.Font.Bold = 0;
//            range.Font.Italic = 0;
//        }
//        /// <summary>
//        /// Idents de first line of the paragraph.
//        /// </summary>
//        /// <param name="paragraph"></param>
//        private void SetFirstLineIndent(Paragraph paragraph)
//        {
//            var specialIndentation = 36.0f;
//            paragraph.Range.ParagraphFormat.FirstLineIndent = specialIndentation;
//        }
//    }
//}