using BusinessSelenium;
using Persistencia;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace SMG
{
    public class ExportarCertames
    {

        public BusinessService exportarcertames(BusinessService _bs)
        {
            if (new AcaoSelenium().validarBusinesService(_bs, AcaoSelenium.TYPE_VALIDATION_TWO_ITEM))
            {
                String fileName = String.Empty;
                IList listaCertames = new ArrayList();

                if (_bs.ListObject[0] != null)
                    fileName = _bs.ListObject[0].ToString();

                if (_bs.ListObject[1] != null)
                    listaCertames = (IList)_bs.ListObject[1];

                Microsoft.Office.Interop.Excel.Application xlsApp = null;
                Microsoft.Office.Interop.Excel.Workbook xlsWorkBook = null;
                Microsoft.Office.Interop.Excel.Worksheet xlsWorkSheet = null;
                Microsoft.Office.Interop.Excel.Range xlsRange = null;
                Microsoft.Office.Interop.Excel.Range xlsRangeEntireRow = null;

                try
                {


                    xlsApp = new Microsoft.Office.Interop.Excel.Application();
                    String path = new IOObjetos().gerarDiretorioDB() + fileName;
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + fileName;
                    xlsWorkBook = xlsApp.Workbooks.Open(path, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", true, false, 1, true, 1, 0);
                    xlsWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlsWorkBook.Worksheets.get_Item(1);
                    xlsRange = xlsWorkSheet.UsedRange;

                    String localizar = String.Empty;
                    int linha = 2;
                    int coluna = 1;
                    int linhaAnterior = -1;
                    Boolean certamePrincipal = true;
                    String loteDoCertame = String.Empty;

                    foreach (VOCertame objVoCertame in listaCertames)
                    {
                        int numeroLotes = objVoCertame.ListTabLotes.Count;
                        for (int i = numeroLotes; i >= 1; i--)
                        {
                            VOLote voLoteNovo = new VOLote();
                            objVoCertame.ListTabLotes.Insert(i, voLoteNovo);
                        }
                    }


                    foreach (VOCertame objVoCertame in listaCertames)
                    {
                        int numeroDoLoteDoCertame = 1;

                        while (numeroDoLoteDoCertame <= objVoCertame.ListTabLotes.Count)
                        {
                            xlsWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlsWorkBook.Worksheets.get_Item(1);
                            xlsRange = xlsWorkSheet.UsedRange;

                            if (objVoCertame.ListTabLotes.Count == 1)
                                loteDoCertame = "Único";
                            else
                                loteDoCertame = numeroDoLoteDoCertame.ToString();

                            // Recupera célula
                            localizar = "UF";
                            xlsRange = recuperarRange(xlsRange, localizar, linha, coluna);
                            //xlsRangeBackup = xlsRange;

                            // Recupera linha inteira
                            xlsRangeEntireRow = xlsRange.EntireRow;
                            xlsRangeEntireRow.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, Type.Missing);

                            // PARTE 2
                            // Indica linha atual da célula que foi selecionada pelo xlsRange
                            linha = 0;
                            coluna = 1;

                            // Inserir dados

                            //if (objVoCertame.Numero.Contains("-00"))
                            //{
                            //    objVoCertame.UF = "DF";
                            //}

                            int indexLote = objVoCertame.ListTabLotes.Count - numeroDoLoteDoCertame;

                            VOLote voLote = (VOLote)objVoCertame.ListTabLotes[indexLote];

                            String value = String.Empty;

                            Boolean secondLine = false;

                            // UF
                            if (String.IsNullOrEmpty(voLote.Uf))
                            {
                                secondLine = true;


                            }
                            else
                            {
                                value = voLote.Uf;
                                inserirValor(xlsRange, value, linha, coluna++);
                            }
                            if (secondLine == false)
                            {


                                // BR
                                //coluna++;

                                value = voLote.Br;
                                if (inserirValor(xlsRange, value, linha, coluna++))
                                {
                                    value = voLote.KmInicio;
                                    // KM INICIAL
                                    if (inserirValor(xlsRange, value, linha, coluna++))
                                    {
                                        value = voLote.KmFim;
                                        // KM FINAL
                                        if (inserirValor(xlsRange, value, linha, coluna++))
                                        {
                                            value = voLote.Extensao;
                                            // EXTENSÃO
                                            if (inserirValor(xlsRange, value, linha, coluna++))
                                            {
                                                if (String.IsNullOrEmpty(voLote.Uf))
                                                {
                                                    voLote.Uf = String.Empty;
                                                    if (!String.IsNullOrEmpty(objVoCertame.UF))
                                                        voLote.Uf = objVoCertame.UF;
                                                }
                                                switch (voLote.Uf.ToUpper())
                                                {
                                                    case "AC":
                                                        value = "ACRE";
                                                        break;

                                                    case "DF":
                                                        value = "DISTRITO FEDERAL";
                                                        break;
                                                }
                                                // REGIÃO
                                                if (inserirValor(xlsRange, value, linha, coluna++))
                                                {

                                                }
                                            }
                                        }
                                    }
                                }

                                // OBJETO
                                value = objVoCertame.Objeto;
                                //VOLote voLote = ((VOLote)objVoCertame.ListTabLotes[numeroDoLoteDoCertame - 1]);
                                if (!String.IsNullOrEmpty(voLote.Descricao) && value != voLote.Descricao)
                                {
                                    //value += System.Environment.NewLine + ((VOLote)objVoCertame.ListTabLotes[numeroDoLoteDoCertame - 1]).Descricao;
                                    value = voLote.Descricao;
                                }

                                if (inserirValor(xlsRange, value, linha, coluna++))
                                {
                                    // UG (UNIDADE GESTORA)
                                    if (inserirValor(xlsRange, objVoCertame.Orgao, linha, coluna++))
                                    {
                                        // MODALIDADE
                                        if (inserirValor(xlsRange, objVoCertame.Modalidade, linha, coluna++))
                                        {
                                            // EDITAL
                                            value = objVoCertame.Numero + System.Environment.NewLine;
                                            value += "Lote " + (Math.Truncate((Decimal)((indexLote + 2) / 2))).ToString();
                                            if (inserirValor(xlsRange, value, linha, coluna++))
                                            {
                                                if (String.IsNullOrEmpty(voLote.ValorEstimado))
                                                    voLote.ValorEstimado = String.Empty;
                                                // ORÇAMENTO - VALOR
                                                value = voLote.ValorEstimado.ToString();
                                                if (value.Replace(".", "").Replace(",", "") != (voLote.ValorEstimado.Replace(".", "").Replace(",", "")))
                                                {
                                                    value += System.Environment.NewLine + voLote.ValorEstimado;
                                                }

                                                //var FormatBackup = xlsRange.NumberFormat;

                                                //value += System.Environment.NewLine + ((VOLote)objVoCertame.ListTabLotes[numeroDoLoteDoCertame]).Valor;
                                                // xlsRange.NumberFormat = "R$ #.###,00";
                                                if (inserirValor(xlsRange, value, linha, coluna++))
                                                {
                                                    //xlsRange.NumberFormat = FormatBackup;
                                                    // DISP. LOA
                                                    coluna++;

                                                    // N. PROCESSO
                                                    if (inserirValor(xlsRange, objVoCertame.NumeroProcesso, linha, coluna++))
                                                    {
                                                        // CARACTERÍSTICA DAS DATAS DOS EVENTOS
                                                        String característicaEvento = "PREVISTO";
                                                        if (!certamePrincipal)
                                                        {
                                                            característicaEvento = "REALIZADO";
                                                        }
                                                        if (inserirValor(xlsRange, característicaEvento, linha, coluna++))
                                                        {
                                                            // DATA APROVAÇÃO PROJETO / ANTEPROJETO
                                                            coluna++;

                                                            String address = recuperarEndereco(xlsRange, linha, coluna);

                                                            String address1 = alterarEnderecoCelula(address, 0, -1);
                                                            String address2 = alterarEnderecoCelula(address, 1, -1);

                                                            //String formula = @"=SEERRO(SE(" + address2 + @"="""";" + address1 + "+30;" + address2 + @"+30);"""")";
                                                            String formula = @"=IFERROR(IF(" + address2 + @"=""""," + address1 + "+30," + address2 + @"+30),"""")";

                                                            // DATA TERMO DE REFERENCIA
                                                            if (inserirFormula(xlsRange, formula, linha, coluna++))
                                                            {
                                                                // CELULA = P4
                                                                // "=SEERRO(SE(O5="";O4+30;O5+30);"")"
                                                                // "=IFERROR(IF(O5="";04+30;O5+30);"")"

                                                                // DATA ATOS PREPARATÓRIOS
                                                                if (inserirFormula(xlsRange, formula, linha, coluna++))
                                                                {
                                                                    address = recuperarEndereco(xlsRange, linha, coluna);
                                                                    address1 = alterarEnderecoCelula(address, 0, -1);
                                                                    address2 = alterarEnderecoCelula(address, 1, -2);
                                                                    String address3 = alterarEnderecoCelula(address, 1, -1);

                                                                    //address1 

                                                                    // =SEERRO(SE(E(P5="";Q5="")=VERDADEIRO;Q4+30;MAIOR(P5:Q5;1)+30);"")
                                                                    //formula = @"=SEERRO(SE(E(" + address2 + @"="""";" + address3 + @"="""")=VERDADEIRO;" + address1 + @"+30;MAIOR(" + address2 + @":" + address3 + @";1)+30);"""")";
                                                                    formula = @"=IFERROR(IF(AND(" + address2 + @"=""""," + address3 + @"="""")=TRUE," + address1 + @"+30,LARGE(" + address2 + @":" + address3 + @",1)+30),"""")";

                                                                    // DECLARAÇÃO EXISTÊNCIA DE RECURSOS
                                                                    if (inserirFormula(xlsRange, formula, linha, coluna++))
                                                                    {
                                                                        //  DATA ELABORAÇÃO EDITAL
                                                                        if (inserirFormula(xlsRange, formula, linha, coluna++))
                                                                        {
                                                                            address = recuperarEndereco(xlsRange, linha, coluna);
                                                                            address1 = alterarEnderecoCelula(address, 0, -1);
                                                                            address2 = alterarEnderecoCelula(address, 1, -1);
                                                                            // SEERRO(SE(S5="";S4+30;S5+30);"")
                                                                            //formula = @"=SEERRO(SE(" + address2 + @"="""";" + address1 + @"+30;" + address1 + @"+30);"""")";
                                                                            formula = @"=IFERROR(IF(" + address2 + @"=""""," + address1 + @"+30," + address1 + @"+30),"""")";

                                                                            // DATA ANALISE PFE
                                                                            if (inserirFormula(xlsRange, formula, linha, coluna++))
                                                                            {
                                                                                // DATA PUBLICAÇÃO
                                                                                if (inserirValor(xlsRange, objVoCertame.DataInicioProposta.ToShortDateString(), linha, coluna++))
                                                                                {
                                                                                    // LICITAÇAO AJUIZADA
                                                                                    coluna++;

                                                                                    // DATA ABERTURA DAS PROPOSTA
                                                                                    if (inserirValor(xlsRange, objVoCertame.DataFimProposta.ToShortDateString(), linha, coluna++))
                                                                                    {
                                                                                        // DATA RESULTADO HABILITAÇÃO
                                                                                        coluna++;

                                                                                        //  RESULTADO PREÇO
                                                                                        if (inserirValor(xlsRange, voLote.ValorRealizado, linha, coluna++))
                                                                                        {
                                                                                            //xlsRange.NumberFormat = FormatBackup;

                                                                                            // RECURSO (FASE LICITATÓRIA)
                                                                                            coluna++;
                                                                                            // HOMOLOGAÇÃO / ADJUDICAÇÃO
                                                                                            coluna++;
                                                                                            // PRIMEIRO DESEMBOLSO (2018)
                                                                                            coluna++;
                                                                                            // OBSERVAÇÕES DE MONITORAMENTO
                                                                                            String mensagensConcatenadas = String.Empty;

                                                                                            //if (inserirValor(xlsRange, objVoCertame.Mensagens, linha, coluna++))
                                                                                            {
                                                                                                // NOTA DE EMPENHO
                                                                                                coluna++;
                                                                                                // CONTRATO
                                                                                                coluna++;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Configurações para a segunda linha
                            }



                            // Retorna para início da planilha
                            xlsWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlsWorkBook.Worksheets.get_Item(1);
                            xlsRange = xlsWorkSheet.UsedRange;

                            // valores de início de pesquisa
                            linhaAnterior = linha;
                            linha = 2;
                            coluna = 1;

                            numeroDoLoteDoCertame++;
                        }

                        // Adicionar referências da segunda linha
                        {
                            // Recupera célula
                            localizar = "UF";
                            linha = 1;
                            coluna = 1;
                            xlsRange = recuperarRange(xlsRange, localizar, linha, coluna);

                            for (int i = 0; i < (objVoCertame.ListTabLotes.Count / 2); i++)
                            {
                                String formula = xlsRange.get_AddressLocal(false, false, Microsoft.Office.Interop.Excel.XlReferenceStyle.xlA1, Type.Missing, Type.Missing);
                                String tmpColumn = Regex.Replace(formula, @"[^A-Z]+", String.Empty);
                                String tmpLine = Regex.Replace(formula, @"[^0-9]+", String.Empty);
                                int tmpPointLine = -1;
                                if (int.TryParse(tmpLine, out tmpPointLine))
                                {
                                    if (tmpPointLine > 0)
                                    {
                                        tmpPointLine = tmpPointLine + 1 + i * 2;
                                        //formula = formula.Insert(0,"=");
                                        formula = "=" + tmpColumn + tmpPointLine;
                                        inserirFormula(xlsRange, formula, 3 + i * 2, coluna);
                                    }
                                }
                            }
                        }

                    } // Foreach

                    //////////////
                    ///// FINALIZAR EXCEL
                    //////////////
                    xlsWorkBook.Close(true, path, null);
                    xlsApp.Quit();

                    ReleaseObject(xlsWorkSheet);
                    ReleaseObject(xlsWorkBook);
                    ReleaseObject(xlsApp);

                    _bs.Result = BusinessService.RESULT_SUCESS;

                }
                catch (Exception e)
                {
                    _bs.Message.Add(e.Message);
                    _bs.Result = BusinessService.RESULT_ERROR;

                    if (xlsWorkBook != null)
                        xlsWorkBook.Close(false, null, null);

                    if (xlsApp != null)
                        xlsApp.Quit();

                    if (xlsWorkSheet != null)
                        ReleaseObject(xlsWorkSheet);
                    if (xlsWorkBook != null)
                        ReleaseObject(xlsWorkBook);
                    if (xlsApp != null)
                        ReleaseObject(xlsApp);

                }
            }
            else
            {
                _bs.Result = BusinessService.RESULT_ERROR;
            }

            return _bs;
        }

        private String recuperarValor(Microsoft.Office.Interop.Excel.Range _xlsRange, String _fraseReferencia, int _linhaReferencia, int _colunaReferencia)
        {
            Microsoft.Office.Interop.Excel.Range _xlsRangeResult = null;

            _xlsRangeResult = _xlsRange.Cells.Find(_fraseReferencia, Missing.Value, Microsoft.Office.Interop.Excel.XlFindLookIn.xlValues, Microsoft.Office.Interop.Excel.XlLookAt.xlPart,
                Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlNext, false, Missing.Value, Missing.Value);

            if (_xlsRangeResult == null)
                return null;

            return System.Convert.ToString((_xlsRangeResult.Cells[_linhaReferencia, _colunaReferencia] as Microsoft.Office.Interop.Excel.Range).Value2);
        }

        private bool inserirValor(Microsoft.Office.Interop.Excel.Range _xlsRange, String _value, int _linhaReferencia, int _colunaReferencia)
        {
            if (_xlsRange == null)
                return false;

            (_xlsRange.Cells[_linhaReferencia, _colunaReferencia] as Microsoft.Office.Interop.Excel.Range).Value2 = _value;

            return true;
        }

        private String recuperarEndereco(Microsoft.Office.Interop.Excel.Range _xlsRange, int _linhaReferencia, int _colunaReferencia)
        {

            if (_xlsRange == null || _linhaReferencia < 0 || _colunaReferencia < 0)
                return String.Empty;

            return (_xlsRange.Cells[_linhaReferencia, _colunaReferencia] as Microsoft.Office.Interop.Excel.Range).get_AddressLocal(false, false, Microsoft.Office.Interop.Excel.XlReferenceStyle.xlA1, Type.Missing, Type.Missing);

        }

        private bool inserirFormula(Microsoft.Office.Interop.Excel.Range _xlsRange, String _value, int _linhaReferencia, int _colunaReferencia)
        {
            if (_xlsRange == null)
                return false;

            (_xlsRange.Cells[_linhaReferencia, _colunaReferencia] as Microsoft.Office.Interop.Excel.Range).Formula = _value;

            return true;
        }

        private Microsoft.Office.Interop.Excel.Range recuperarRange(Microsoft.Office.Interop.Excel.Range _xlsRange, String _fraseReferencia, int _linhaReferencia, int _colunaReferencia)
        {
            Microsoft.Office.Interop.Excel.Range _xlsRangeResult = null;

            _xlsRangeResult = _xlsRange.Cells.Find(_fraseReferencia, Missing.Value, Microsoft.Office.Interop.Excel.XlFindLookIn.xlValues, Microsoft.Office.Interop.Excel.XlLookAt.xlPart,
                Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlNext, false, Missing.Value, Missing.Value);

            if (_xlsRangeResult == null)
                return null;

            return ((_xlsRangeResult.Cells[_linhaReferencia, _colunaReferencia] as Microsoft.Office.Interop.Excel.Range));
        }

        private Microsoft.Office.Interop.Excel.Range recuperarLinhaInteira(Microsoft.Office.Interop.Excel.Range _xlsRange, String _fraseReferencia, int _linhaReferencia, int _colunaReferencia)
        {
            Microsoft.Office.Interop.Excel.Range _xlsRangeResult = null;

            _xlsRangeResult = _xlsRange.Cells.Find(_fraseReferencia, Missing.Value, Microsoft.Office.Interop.Excel.XlFindLookIn.xlValues, Microsoft.Office.Interop.Excel.XlLookAt.xlPart,
                Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlNext, false, Missing.Value, Missing.Value);

            if (_xlsRangeResult == null)
                return null;

            return ((_xlsRangeResult.EntireRow[_linhaReferencia, _colunaReferencia] as Microsoft.Office.Interop.Excel.Range));
        }

        private String alterarEnderecoCelula(String _str, int _addLine, int _addColumn)
        {
            if (!String.IsNullOrEmpty(_str))
            {

                String tmpLine = String.Empty;
                String tmpColumn = String.Empty;

                tmpLine = Regex.Replace(_str, @"[^0-9]+", String.Empty);
                tmpColumn = Regex.Replace(_str, @"[^A-Z]+", String.Empty);

                if (tmpColumn.Length == 1)
                {
                    Boolean minus = false;
                    if (_addColumn < 0)
                    {
                        minus = true;
                        _addColumn = _addColumn * -1;
                    }

                    Char caracter = tmpColumn[0];

                    for (int i = 0; i < _addColumn; i++)
                    {
                        if (minus == true)
                            caracter--;
                        else
                            caracter++;
                    }
                    tmpColumn = caracter.ToString();
                }
                else
                {
                    return String.Empty;
                }

                tmpLine = Regex.Replace(_str, @"[^0-9]+", String.Empty);
                int tmpPointLine = -1;

                if (int.TryParse(tmpLine, out tmpPointLine))
                {
                    if (tmpPointLine > 0)
                    {
                        tmpPointLine = tmpPointLine + _addLine;
                        _str = tmpColumn + tmpPointLine.ToString();
                    }
                }
            }
            else
                _str = String.Empty;

            return _str;
        }


        /// <summary>
        /// Libera o objeto passado por parametro.
        /// </summary>
        /// <param name="obj"></param>
        private void ReleaseObject(object obj)
        {
            try
            {
                while (Marshal.ReleaseComObject(obj) != 0) { };
                if (obj != null && Marshal.IsComObject(obj))
                {
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(obj);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

    }
}
