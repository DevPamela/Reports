using NPOI.XWPF.UserModel;

namespace Reports.Domain.Interfaces.Service
{
    public interface IFormatterService
    {
        /// <summary>
        /// Formats each paragraph according to ABNT rules.
        /// </summary>
        /// <param name="paragraph"></param>
        /// <param name="application"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
         Task SetDefaultFormatForReport(XWPFParagraph paragraph, XWPFRun run, XWPFDocument doc);

        /// <summary>
        /// Sets de format for header.
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
         Task SetDefaultFormatForHeader(XWPFRun run, XWPFParagraph paragraph);
    }
}