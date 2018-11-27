using BusinessSelenium;
using Persistencia;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

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
                        for (int i = 0; i < numeroLotes; i++)
                        {
                            VOLote voLoteNovo = new VOLote();
                            objVoCertame.ListTabLotes.Insert(i + 1, voLoteNovo);
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

                            // PARTE 1
                            localizar = "UF";
                            xlsRange = recuperarRange(xlsRange, localizar, linha, coluna);
                            // PROCURA A LINHA DO CABEÇALHO PARA INSERIR NOVA LINHA
                            xlsRange.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, Type.Missing);

                            // PARTE 2
                            linha = linhaAnterior + 1;
                            coluna = 1;

                            // Inserir dados

                            if (objVoCertame.Numero.Contains("-00"))
                            {
                                objVoCertame.UF = "DF";
                            }

                            int indexLote = numeroDoLoteDoCertame - 1;

                            VOLote lote = (VOLote)objVoCertame.ListTabLotes[indexLote];

                            // UF
                            if (inserirValor(xlsRange, lote.Uf, linha, coluna++))
                            {
                                String value = String.Empty;

                                // BR
                                //coluna++;

                                value = lote.Br;
                                if (inserirValor(xlsRange, value, linha, coluna++))
                                {
                                    value = lote.KmInicio;
                                    // KM INICIAL
                                    if (inserirValor(xlsRange, value, linha, coluna++))
                                    {
                                        value = lote.KmFim;
                                        // KM FINAL
                                        if (inserirValor(xlsRange, value, linha, coluna++))
                                        {
                                            value = lote.Extensao;
                                            // EXTENSÃO
                                            if (inserirValor(xlsRange, value, linha, coluna++))
                                            {
                                                if (String.IsNullOrEmpty(lote.Uf))
                                                {
                                                    lote.Uf = String.Empty;
                                                    if (String.IsNullOrEmpty(objVoCertame.UF))
                                                    {
                                                        if (objVoCertame.Uasg == "393031")
                                                        {
                                                            objVoCertame.UF = "MG";
                                                        }
                                                    }
                                                    lote.Uf = objVoCertame.UF;
                                                }
                                                if (!String.IsNullOrEmpty(lote.Uf))
                                                {
                                                    switch (lote.Uf.ToUpper())
                                                    {
                                                        case "AC":
                                                            value = "ACRE";
                                                            break;

                                                        case "DF":
                                                            value = "DISTRITO FEDERAL";
                                                            break;
                                                        case "MG":
                                                            value = "MINAS GERAIS";
                                                            break;
                                                    }
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
                                VOLote voLote = ((VOLote)objVoCertame.ListTabLotes[numeroDoLoteDoCertame - 1]);
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
                                            value += "Lote " + loteDoCertame;
                                            if (inserirValor(xlsRange, value, linha, coluna++))
                                            {
                                                // ORÇAMENTO - VALOR
                                                if (String.IsNullOrEmpty(voLote.ValorEstimado))
                                                    voLote.ValorEstimado = String.Empty;

                                                value = voLote.ValorEstimado.ToString();
                                                if (value.Replace(".", "").Replace(",", "") != ((VOLote)objVoCertame.ListTabLotes[numeroDoLoteDoCertame - 1]).ValorEstimado.Replace(".", "").Replace(",", ""))
                                                {
                                                    value += System.Environment.NewLine + ((VOLote)objVoCertame.ListTabLotes[numeroDoLoteDoCertame - 1]).ValorEstimado;
                                                }
                                                //value += System.Environment.NewLine + ((VOLote)objVoCertame.ListTabLotes[numeroDoLoteDoCertame]).Valor;
                                                xlsRange.NumberFormat = "R$ #.###,00";
                                                if (inserirValor(xlsRange, objVoCertame.Valor.ToString(), linha, coluna++))
                                                {
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
                                                            // DATA TERMO DE REFERENCIA
                                                            coluna++;
                                                            // DATA ATOS PREPARATÓRIOS
                                                            coluna++;
                                                            // DATA DECLARAÇÃO EXISTENCIA DE RECURSOS
                                                            coluna++;
                                                            // DATA ELABORAÇÃO EDITAL
                                                            coluna++;
                                                            // DATA ANALISE PFE
                                                            coluna++;

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

                                                                    if (String.IsNullOrEmpty(voLote.ValorRealizado))
                                                                        voLote.ValorRealizado = String.Empty;

                                                                    //  RESULTADO PREÇO
                                                                    if (inserirValor(xlsRange, voLote.ValorRealizado, linha, coluna++))
                                                                    {
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

                            // Retorna para início da planilha
                            xlsWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlsWorkBook.Worksheets.get_Item(1);
                            xlsRange = xlsWorkSheet.UsedRange;

                            // valores de início de pesquisa
                            linhaAnterior = linha;
                            linha = 2;
                            coluna = 1;

                            numeroDoLoteDoCertame++;
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

                    //if (xlsWorkBook != null)
                    //    xlsWorkBook.Close(false, null, null);

                    //if (xlsApp != null)
                    //    xlsApp.Quit();

                    //if (xlsWorkSheet != null)
                    //    releaseObject(xlsWorkSheet);

                    //if (xlsWorkBook != null)
                    //    releaseObject(xlsWorkBook);

                    //if (xlsApp != null)
                    //    releaseObject(xlsApp);

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

        private Microsoft.Office.Interop.Excel.Range recuperarRange(Microsoft.Office.Interop.Excel.Range _xlsRange, String _fraseReferencia, int _linhaReferencia, int _colunaReferencia)
        {
            Microsoft.Office.Interop.Excel.Range _xlsRangeResult = null;

            _xlsRangeResult = _xlsRange.Cells.Find(_fraseReferencia, Missing.Value, Microsoft.Office.Interop.Excel.XlFindLookIn.xlValues, Microsoft.Office.Interop.Excel.XlLookAt.xlPart,
                Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlNext, false, Missing.Value, Missing.Value);

            if (_xlsRangeResult == null)
                return null;

            return ((_xlsRangeResult.Cells[_linhaReferencia, _colunaReferencia] as Microsoft.Office.Interop.Excel.Range));
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
