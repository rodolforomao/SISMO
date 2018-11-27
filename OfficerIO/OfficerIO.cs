using BusinessSelenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistencia;

namespace OfficerIO
{
    public class OfficerIO
    {
        public BusinessService inserirExcel(BusinessService _bs)
        {
            if (new AcaoSelenium().validarBusinesService(_bs, AcaoSelenium.TYPE_VALIDATION_TWO_ITEM))
            {
                String fileName = _bs.ListObject[0].ToString();
                fileName = (new IOObjetos()).gerarDiretorioRoot() + (new IOObjetos()).indentPhrase(fileName);

                try
                {
                    Microsoft.Office.Interop.Excel.Application xlsApp;
                    Microsoft.Office.Interop.Excel.Workbook xlsWorkBook;
                    Microsoft.Office.Interop.Excel.Worksheet xlsWorkSheet;
                    Microsoft.Office.Interop.Excel.Range xlsRange;

                    xlsApp = new Microsoft.Office.Interop.Excel.Application();
                    xlsWorkBook = xlsApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    xlsWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlsWorkBook.Worksheets.get_Item(1);

                    xlsRange = xlsWorkSheet.UsedRange;




                }
                catch(Exception e)
                {

                }
            }
            return _bs;
        }
    }
}
