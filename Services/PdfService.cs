using System.IO;
using System.Text;
using UglyToad.PdfPig;

namespace Course_Pilot
{
    public static class PdfService
    {
        //extract all readable text from a PDF
        public static string ExtractPdfText(string filePath)
        {
            StringBuilder text = new();

            using (PdfDocument document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    text.AppendLine(page.Text);
                }
            }

            return text.ToString();
        }

        //check that the file is a PDF and PdfPig can open it
        public static bool VerifyPdf(string filePath)
        {
            //if not a pdf, return false
            if (Path.GetExtension(filePath).ToLower() != ".pdf")
            {
                return false;
            }

            try
            {
                //open file using PdfPig, if it succeeds file is valid
                using (PdfDocument document = PdfDocument.Open(filePath))
                {
                    return true;
                }
            }
            catch
            {
                //if PdfPig cannot open the file, it is not a valid PDF.
                return false;
            }
        }
    }
}