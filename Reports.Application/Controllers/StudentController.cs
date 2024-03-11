using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Reports.Application.Models;

namespace Reports.Application.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ShowData([FromForm] IFormFile excelFile)
        {
            Stream stream = excelFile.OpenReadStream();
            IWorkbook excel = null;

            if (Path.GetExtension(excelFile.FileName) == ".xlsx")
            {
                excel = new XSSFWorkbook(stream);
            }
            else
            {
                excel = new HSSFWorkbook(stream);
            }

            ISheet sheet = excel.GetSheetAt(0);
            int linesQuantity = sheet.LastRowNum;
            List<StudentViewModel> students = new List<StudentViewModel>();

            for (int i = 1; i <= linesQuantity; i++)
            {
                IRow line = sheet.GetRow(i);
                //                var columnMapping = new Dictionary<string, string>
                //{
                //    { "Nome", "Name" },

                //};
                //                foreach (var cell in line.Cells.Cast<ICell>())
                //                {
                //                    string columnName = cell.StringCellValue;

                //                    // Verifique se o nome da coluna está no mapeamento
                //                    if (columnMapping.TryGetValue(columnName, out string propertyName))
                //                    {
                //                        // Agora você pode usar 'propertyName' para atribuir valores às propriedades do seu modelo
                //                        // Exemplo: model.Nome = line.GetCell(cell.ColumnIndex).ToString();
                //                    }

                //TODO pegar se a coluna e prop tem o mesmo valor.toupper
                //melhorar a validação se é null line.Cells.Count > 2
                //continuar com a conexão com o db
                if (line.Cells.Count > 2)
                {
                    students.Add(new StudentViewModel
                    {

                        Name = line.GetCell(0).ToString(),
                        Birthday = line.GetCell(1).ToString(),
                        Absences = line.GetCell(2).ToString(),

                    });
                }
            }
            return StatusCode(StatusCodes.Status200OK, students);
        }


    }
}

