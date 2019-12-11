using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace SmearAdmin.Helpers
{
    public class GeneratePDF
    {
        private readonly IHostingEnvironment _host;
        private IConverter _converter;
        public GeneratePDF() { }
        public GeneratePDF(IConverter converter, IHostingEnvironment host)
        {
            _converter = converter;
            _host = host;
        }

        public byte[] GetPdfFile(string fileName, string getPdfHtml, string documentTitle, string headerSettingsText, string footerSettingsText)
        {
            var uploadFolderPath = Path.Combine(_host.WebRootPath, "Downloads");
            var fullPath = $"{uploadFolderPath}\\{fileName}";

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = documentTitle,
                Out = fullPath
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = getPdfHtml,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(_host.WebRootPath, "AssetsPdf", "pdfStyle.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = headerSettingsText, Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Left = footerSettingsText }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return _converter.Convert(pdf);
        }
    }
}
