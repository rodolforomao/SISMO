using BusinessSelenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Persistencia;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace SMG
{
    public class Navegar
    {

        public const String MODALIDADE_PREGAO = "PREGAO";




        public BusinessService realizarPesquisalistaPregaoFiltro(BusinessService _bs)
        {
            BusinessService bs = new BusinessService();

            bs.Operation = BusinessService.OPRATION_READ;

            ChromeOptions options = new ChromeOptions();
            int option = 0;


            //options.AddArguments("--proxy-server=138.94.71.202");
            //options.AddArgument(@"--incognito");
            options.AddArgument("--start-maximized");

            using (var driver = new ChromeDriver(options))
            {
                // Rastrear outros tipos de licitação
                //for (option = 0; option < 5; option++)
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=0
                // 	Pesquisa por: PREGÕES AGENDADOS
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=1
                // 	Pesquisa por: PREGÕES REALIZADOS, PENDENTES DE RECURSO/ ADJUDICAÇAO / HOMOLOGAÇÃO
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=2
                // 	Pesquisa por:  PREGÕES EM ANDAMENTO
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=3
                // 	Pesquisa por:  PREGÕES REVOGADOS, ANULADOS OU ABANDONADOS

                IList listaUASG = listaUASGDnit();

                foreach (ComboItem objComboUASG in listaUASG)
                {

                    _bs.ObjectDriver = driver;
                    IWebDriver _driver = _bs.ObjectDriver;
                    String modalidade = _bs.ListObject[2].ToString();

                    // Pregões agendados
                    //  "http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=0";
                    String enderecoUrlString = _bs.ListObject[0].ToString();
                    String idElement = "";
                    String keysSendElement = String.Empty;
                    IWebElement element = null;

                    AcaoSelenium acaoSelenium = new AcaoSelenium();

                    if ((acaoSelenium.goToUrl(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, enderecoUrlString, "", AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, idElement, false)))
                    {

                        driver.SwitchTo().Frame(1);

                        Boolean repetir = false;
                        int contador = 0;
                        do
                        {
                            contador++;
                            idElement = "lstSituacao";
                            keysSendElement = "Todas";

                            if ((bool)acaoSelenium.DropDownList(_driver, idElement, keysSendElement, AcaoSelenium.NUMBER_ATTEMPTS_3, AcaoSelenium.DROPDOWNLIST_TYPE_SEARCH_EXACT_TEXT))
                            {

                                idElement = "co_uasg";

                                VOCertame vo = (VOCertame)_bs.ListObject[1];
                                //keysSendElement = vo.Uasg;
                                keysSendElement = objComboUASG.Value.ToString();
                                String uasg = keysSendElement;
                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_SEND_KEYS, ref element))
                                {

                                    idElement = "dt_entrega";
                                    //keysSendElement = DateTime.Now.AddDays(-contador).ToShortDateString();
                                    keysSendElement = String.Empty;
                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_SEND_KEYS, ref element))
                                    {
                                        idElement = "ok";
                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                        {
                                            Boolean tryAgain = false;
                                            int contadorErrotryAgain = 0;

                                            do
                                            {
                                                contadorErrotryAgain++;
                                                try
                                                {
                                                    idElement = "//*[@id=\"content\"]/div/fieldset/h2";
                                                    acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_2, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element);

                                                    if ((element == null) || (element != null && !element.Text.ToLower().Contains("server error")))
                                                    {
                                                        idElement = "/html/body/table/tbody/tr[2]/td/table[2]/tbody/tr[2]/td[2]/table/tbody/tr/td/table/tbody/tr[3]/td/table";
                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                        {
                                                            String tagTable = "td";
                                                            int quantidadeColunas = 7;
                                                            IWebElement baseTable = element;

                                                            // gets all table rows
                                                            IList rows = baseTable.FindElements(By.TagName(tagTable));

                                                            int quantidadeItem = rows.Count / quantidadeColunas;

                                                            if (quantidadeItem > 1)
                                                            {
                                                                for (int i = 1; i < quantidadeItem; i++)
                                                                {
                                                                    VOCertame certame = new VOCertame();
                                                                    int index = 0;

                                                                    certame.Status = modalidade;
                                                                    certame.Modalidade = VOCertame.MODALIDADE_PREGAO_ELETRONICO;
                                                                    certame.Sigla = VOCertame.MODALIDADE_PREGAO_ELETRONICO_SIGLA;
                                                                    certame.Numero = (((IWebElement)rows[index++ + quantidadeColunas * i]).Text).Replace("/", "");
                                                                    certame.Uasg = ((((IWebElement)rows[index++ + quantidadeColunas * i]).Text).Replace("/", ""));
                                                                    certame.Orgao = ((IWebElement)rows[index++ + quantidadeColunas * i]).Text;
                                                                    if (option == 0)
                                                                    {
                                                                        certame.DataInicioProposta = DateTime.Parse(((IWebElement)rows[index++ + quantidadeColunas * i]).Text);
                                                                        certame.DataFimProposta = DateTime.Parse(((IWebElement)rows[index++ + quantidadeColunas * i]).Text);
                                                                        certame.Situacao = (((IWebElement)rows[index++ + quantidadeColunas * i]).Text);
                                                                        certame.InformacoesCertame = (((IWebElement)rows[index++ + quantidadeColunas * i]).Text);
                                                                    }

                                                                    // recolher informações
                                                                    // link do número do certame
                                                                    idElement = "/html/body/table/tbody/tr[2]/td/table[2]/tbody/tr[2]/td[2]/table/tbody/tr/td/table/tbody/tr[3]/td/table/tbody/tr[" + (i + 1) + "]/td[1]/a";
                                                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                    {
                                                                        // Entra na janela aberta
                                                                        String currentContext = acaoSelenium.trocarDeContextoHandleWindow(driver, "Pregao/dados_pregao.asp");

                                                                        idElement = "/html/body/table/tbody/tr[2]/td[2]/form/table/tbody/tr[2]/td/table/tbody/tr/td";
                                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                        {
                                                                            if (!String.IsNullOrEmpty(element.Text.Trim()))
                                                                            {
                                                                                String tagStringInicio = "Objeto: ";
                                                                                String tagStringFim = "Descrição: ";
                                                                                int indexInicio = element.Text.IndexOf(tagStringInicio, 0);
                                                                                int indexFim = element.Text.IndexOf(tagStringFim, 0);
                                                                                if (indexFim < 0)
                                                                                {
                                                                                    tagStringFim = "Data da Realização";
                                                                                    indexFim = element.Text.IndexOf(tagStringFim, 0);
                                                                                }

                                                                                //String str = element.Text.Substring(indexInicio + tagStringInicio.Length, indexFim - (indexInicio + tagStringFim.Length - 1));
                                                                                indexInicio = indexInicio + tagStringInicio.Length;
                                                                                int tamanho = indexFim - indexInicio;
                                                                                String str = element.Text.Substring(indexInicio, tamanho);
                                                                                certame.Objeto = str;

                                                                                tagStringInicio = "Data da Realização (início dos lances):";
                                                                                indexInicio = element.Text.IndexOf(tagStringInicio, 0);
                                                                                if (indexFim < 0)
                                                                                {
                                                                                    tagStringFim = "Data da Realização";
                                                                                    indexFim = element.Text.IndexOf(tagStringFim, 0);
                                                                                }

                                                                                str = element.Text.Substring(indexInicio + tagStringInicio.Length, element.Text.Length - indexInicio - tagStringInicio.Length);

                                                                                String value = Regex.Match(str, @"(\d{2}/\d{2}/\d{4} \d{2}:\d{2})").Value;

                                                                                certame.DataCertame = DateTime.Parse(value);

                                                                            }

                                                                        }

                                                                        idElement = "fechar";
                                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_NAME, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                        { }

                                                                        acaoSelenium.retornaContextoHandleWindowPopUp(driver, currentContext);

                                                                        ///////////////////////////////
                                                                        driver.SwitchTo().Frame(1);

                                                                        idElement = "/html/body/table/tbody/tr[2]/td/table[2]/tbody/tr[2]/td[2]/table/tbody/tr/td/table/tbody/tr[3]/td/table/tbody/tr[" + (i + 1) + "]/td[7]/a[2]";
                                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                        {
                                                                            currentContext = acaoSelenium.trocarDeContextoHandleWindow(driver, "Pregao/avisos1.asp");

                                                                            driver.SwitchTo().Frame(1);
                                                                            idElement = "/html/body/table/tbody/tr/td[2]/p/a";
                                                                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                            {
                                                                                acaoSelenium.trocarDeContextoHandleWindow(driver, "Pregao/avisos4.asp");
                                                                                idElement = "//*[@id=\"form1\"]/table/tbody/tr[1]/td/span";
                                                                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                                {
                                                                                    certame.Mensagens += element.Text + " - ";

                                                                                    idElement = "//*[@id=\"form1\"]/table/tbody/tr[3]/td";
                                                                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                                    {
                                                                                        certame.Mensagens += element.Text + System.Environment.NewLine;
                                                                                        idElement = "fechar";
                                                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_NAME, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                                        {
                                                                                            acaoSelenium.trocarDeContextoHandleWindow(driver, "Pregao/avisos1.asp");
                                                                                        }
                                                                                    }

                                                                                }
                                                                            }

                                                                            acaoSelenium.retornaContextoHandleWindowPopUp(driver, currentContext);
                                                                            driver.SwitchTo().Frame(1);
                                                                        }


                                                                    }
                                                                    // link das informacoes do certame

                                                                    bs.ListObject.Add(certame);
                                                                }

                                                            }
                                                            else
                                                            {
                                                                idElement = "voltar";
                                                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_NAME, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                {
                                                                    repetir = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bs.Message.Add(element.Text);
                                                        bs.Result = BusinessService.RESULT_ERROR;
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    tryAgain = true;
                                                    if (contadorErrotryAgain > 3)
                                                        tryAgain = false;
                                                }
                                            } while (tryAgain);
                                        }
                                    }


                                }

                            }
                            else
                            {
                                if (contador > 3)
                                    repetir = false;
                            }


                        } while (repetir);


                    }

                }// for
            }

            return bs;
        }

        public BusinessService realizarPesquisalistaPregaoEmAndamento(BusinessService _bs)
        {
            BusinessService bs = new BusinessService();

            bs.Operation = BusinessService.OPRATION_READ;

            ChromeOptions options = new ChromeOptions();
            int option = 0;


            //options.AddArguments("--proxy-server=138.94.71.202");
            //options.AddArgument(@"--incognito");
            options.AddArgument("--start-maximized");

            using (var driver = new ChromeDriver(options))
            {
                // Rastrear outros tipos de licitação
                //for (option = 0; option < 5; option++)
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=0
                // 	Pesquisa por: PREGÕES AGENDADOS
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=1
                // 	Pesquisa por: PREGÕES REALIZADOS, PENDENTES DE RECURSO/ ADJUDICAÇAO / HOMOLOGAÇÃO
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=2
                // 	Pesquisa por:  PREGÕES EM ANDAMENTO
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=3
                // 	Pesquisa por:  PREGÕES REVOGADOS, ANULADOS OU ABANDONADOS

                IList listaUASG = listaUASGDnit();

                foreach (ComboItem objComboUASG in listaUASG)
                {

                    _bs.ObjectDriver = driver;
                    IWebDriver _driver = _bs.ObjectDriver;
                    String statusDoCertame = _bs.ListObject[2].ToString();

                    // Pregões agendados
                    //  "http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=0";
                    String enderecoUrlString = _bs.ListObject[0].ToString();
                    String idElement = "";
                    String keysSendElement = String.Empty;
                    IWebElement element = null;

                    AcaoSelenium acaoSelenium = new AcaoSelenium();

                    if ((acaoSelenium.goToUrl(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, enderecoUrlString, "", AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, idElement, false)))
                    {

                        driver.SwitchTo().Frame(1);

                        Boolean repetir = false;
                        int contador = 0;
                        do
                        {
                            contador++;
                            idElement = "lstSituacao";
                            keysSendElement = "Todas";

                            if ((bool)acaoSelenium.DropDownList(_driver, idElement, keysSendElement, AcaoSelenium.NUMBER_ATTEMPTS_3, AcaoSelenium.DROPDOWNLIST_TYPE_SEARCH_EXACT_TEXT))
                            {

                                idElement = "co_uasg";

                                VOCertame vo = (VOCertame)_bs.ListObject[1];
                                //keysSendElement = vo.Uasg;
                                keysSendElement = objComboUASG.Value.ToString();
                                String uasg = keysSendElement;
                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_SEND_KEYS, ref element))
                                {

                                    idElement = "dt_entrega";
                                    //keysSendElement = DateTime.Now.AddDays(-contador).ToShortDateString();
                                    keysSendElement = String.Empty;
                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_SEND_KEYS, ref element))
                                    {
                                        idElement = "ok";
                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                        {
                                            Boolean tryAgain = false;
                                            int contadorErrotryAgain = 0;

                                            do
                                            {
                                                contadorErrotryAgain++;
                                                try
                                                {
                                                    idElement = "//*[@id=\"content\"]/div/fieldset/h2";
                                                    acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_2, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element);

                                                    if ((element == null) || (element != null && !element.Text.ToLower().Contains("server error")))
                                                    {
                                                        idElement = "/html/body/table/tbody/tr[2]/td/table[2]/tbody/tr[2]/td[2]/table/tbody/tr/td/table/tbody/tr[3]/td/table";
                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                        {
                                                            String tagTable = "td";
                                                            int quantidadeColunas = 7;
                                                            IWebElement baseTable = element;

                                                            // gets all table rows
                                                            IList rows = baseTable.FindElements(By.TagName(tagTable));

                                                            int quantidadeItem = rows.Count / quantidadeColunas;

                                                            if (quantidadeItem > 1)
                                                            {
                                                                for (int i = 1; i < quantidadeItem; i++)
                                                                {
                                                                    VOCertame certame = new VOCertame();
                                                                    int index = 0;

                                                                    certame.Status = statusDoCertame;
                                                                    certame.Modalidade = VOCertame.MODALIDADE_PREGAO_ELETRONICO;
                                                                    certame.Sigla = VOCertame.MODALIDADE_PREGAO_ELETRONICO_SIGLA;
                                                                    certame.Numero = (((IWebElement)rows[index++ + quantidadeColunas * i]).Text).Replace("/", "");
                                                                    certame.Uasg = ((((IWebElement)rows[index++ + quantidadeColunas * i]).Text).Replace("/", ""));
                                                                    certame.Orgao = ((IWebElement)rows[index++ + quantidadeColunas * i]).Text;
                                                                    if (option == 0)
                                                                    {
                                                                        certame.DataInicioProposta = DateTime.Parse(((IWebElement)rows[index++ + quantidadeColunas * i]).Text);
                                                                        certame.DataFimProposta = DateTime.Parse(((IWebElement)rows[index++ + quantidadeColunas * i]).Text);
                                                                        certame.Situacao = (((IWebElement)rows[index++ + quantidadeColunas * i]).Text);
                                                                        certame.InformacoesCertame = (((IWebElement)rows[index++ + quantidadeColunas * i]).Text);
                                                                    }

                                                                    /////////////////////////////////////

                                                                    idElement = "/html/body/table/tbody/tr[2]/td/table[2]/tbody/tr[2]/td[2]/table/tbody/tr/td/table/tbody/tr[3]/td/table/tbody/tr[" + (i + 1) + "]/td[7]/a";
                                                                    //idElement = "/html/body/table/tbody/tr[2]/td/table[2]/tbody/tr[2]/td[2]/table/tbody/tr/td/table/tbody/tr[3]/td/table/tbody/tr[" + (i + 1) + "]/td";
                                                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                    {
                                                                        String currentContext = acaoSelenium.trocarDeContextoHandleWindow(driver, "Pregao/motiv_suspensao.asp");

                                                                        idElement = "/html/body/table/tbody/tr[3]/td/table/tbody/tr/td/table/tbody/tr[1]/td[2]";
                                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                        {
                                                                            //acaoSelenium.trocarDeContextoHandleWindow(driver, "Pregao/avisos4.asp");

                                                                            idElement = "/html/body/table/tbody/tr[3]/td/table/tbody/tr/td/table/tbody/tr[5]/td[2]";
                                                                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                            {
                                                                                certame.Mensagens += element.Text + System.Environment.NewLine;

                                                                                idElement = "/html/body/table/tbody/tr[3]/td/table/tbody/tr/td/table/tbody/tr[2]/td[2]";
                                                                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                                {
                                                                                    certame.Status = element.Text;

                                                                                    idElement = "/html/body/table/tbody/tr[3]/td/table/tbody/tr/td/table/tbody/tr[3]/td[2]";
                                                                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                                    {
                                                                                        DateTime data = DateTime.MinValue;
                                                                                        if (DateTime.TryParse(element.Text, out data))
                                                                                            certame.DataAtualizacao = data;

                                                                                        idElement = "btFechar";
                                                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID_CLICKABLE, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                                        {
                                                                                            acaoSelenium.retornaContextoHandleWindowPopUp(driver, currentContext);
                                                                                            driver.SwitchTo().Frame(1);
                                                                                        }
                                                                                    }
                                                                                }

                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            String context2 = acaoSelenium.trocarDeContextoHandleWindow(driver, "Pregao/avisos1.asp");
                                                                            driver.SwitchTo().Frame(1);

                                                                            idElement = "/html/body/table/tbody/tr/td[2]/p/a";
                                                                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                            {
                                                                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                                {
                                                                                    String currentContext2 = acaoSelenium.trocarDeContextoHandleWindow(driver, "Pregao/avisos4.asp");

                                                                                    idElement = "//*[@id='form1']/table/tbody/tr[1]/td/span";
                                                                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                                    {
                                                                                        DateTime data = DateTime.MinValue;
                                                                                        if (DateTime.TryParse(element.Text, out data))
                                                                                            certame.DataAtualizacao = data;

                                                                                        idElement = "//*[@id='form1']/table/tbody/tr[3]/td";
                                                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                                        {
                                                                                            certame.Mensagens += element.Text + System.Environment.NewLine;

                                                                                            idElement = "fechar";
                                                                                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_NAME_CLICKABLE, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                                            {
                                                                                                acaoSelenium.trocarDeContextoHandleWindow(driver, context2);

                                                                                                driver.SwitchTo().Frame(2);

                                                                                                idElement = "fechar";
                                                                                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_NAME_CLICKABLE, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                                                {
                                                                                                    acaoSelenium.retornaContextoHandleWindowPopUp(driver, currentContext);
                                                                                                }

                                                                                            }
                                                                                        }
                                                                                            
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    bs.ListObject.Add(certame);
                                                                }

                                                            }
                                                            else
                                                            {
                                                                idElement = "voltar";
                                                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_NAME, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                {
                                                                    repetir = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bs.Message.Add(element.Text);
                                                        bs.Result = BusinessService.RESULT_ERROR;
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    tryAgain = true;
                                                    if (contadorErrotryAgain > 3)
                                                        tryAgain = false;
                                                }
                                            } while (tryAgain);
                                        }
                                    }


                                }

                            }
                            else
                            {
                                if (contador > 3)
                                    repetir = false;
                            }


                        } while (repetir);


                    }

                }// for
            }

            return bs;
        }

        public BusinessService realizarPesquisalistaPregaoRealizadosPendentesdeRecursoAdjudicaçãoHomologação(BusinessService _bs)
        {
            BusinessService bs = new BusinessService();

            bs.Operation = BusinessService.OPRATION_READ;

            ChromeOptions options = new ChromeOptions();
            int option = 0;


            //options.AddArguments("--proxy-server=138.94.71.202");
            //options.AddArgument(@"--incognito");
            options.AddArgument("--start-maximized");

            using (var driver = new ChromeDriver(options))
            {
                // Rastrear outros tipos de licitação
                //for (option = 0; option < 5; option++)
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=0
                // 	Pesquisa por: PREGÕES AGENDADOS
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=1
                // 	Pesquisa por: PREGÕES REALIZADOS, PENDENTES DE RECURSO/ ADJUDICAÇAO / HOMOLOGAÇÃO
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=2
                // 	Pesquisa por:  PREGÕES EM ANDAMENTO
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=3
                // 	Pesquisa por:  PREGÕES REVOGADOS, ANULADOS OU ABANDONADOS

                IList listaUASG = listaUASGDnit();

                foreach (ComboItem objComboUASG in listaUASG)
                {

                    _bs.ObjectDriver = driver;
                    IWebDriver _driver = _bs.ObjectDriver;
                    String statusDoCertame = _bs.ListObject[2].ToString();

                    // Pregões agendados
                    //  "http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=0";
                    String enderecoUrlString = _bs.ListObject[0].ToString();
                    String idElement = "";
                    String keysSendElement = String.Empty;
                    IWebElement element = null;

                    AcaoSelenium acaoSelenium = new AcaoSelenium();

                    if ((acaoSelenium.goToUrl(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, enderecoUrlString, "", AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, idElement, false)))
                    {

                        driver.SwitchTo().Frame(1);

                        Boolean repetir = false;
                        int contador = 0;
                        do
                        {
                            contador++;
                            idElement = "lstSituacao";
                            keysSendElement = "Todas";

                            if ((bool)acaoSelenium.DropDownList(_driver, idElement, keysSendElement, AcaoSelenium.NUMBER_ATTEMPTS_3, AcaoSelenium.DROPDOWNLIST_TYPE_SEARCH_EXACT_TEXT))
                            {

                                idElement = "co_uasg";

                                VOCertame vo = (VOCertame)_bs.ListObject[1];
                                //keysSendElement = vo.Uasg;
                                keysSendElement = objComboUASG.Value.ToString();
                                String uasg = keysSendElement;
                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_SEND_KEYS, ref element))
                                {

                                    idElement = "dt_entrega";
                                    //keysSendElement = DateTime.Now.AddDays(-contador).ToShortDateString();
                                    keysSendElement = String.Empty;
                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_SEND_KEYS, ref element))
                                    {
                                        idElement = "ok";
                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                        {
                                            Boolean tryAgain = false;
                                            int contadorErrotryAgain = 0;

                                            do
                                            {
                                                contadorErrotryAgain++;
                                                try
                                                {
                                                    idElement = "//*[@id=\"content\"]/div/fieldset/h2";
                                                    acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_2, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element);

                                                    if ((element == null) || (element != null && !element.Text.ToLower().Contains("server error")))
                                                    {
                                                        idElement = "/html/body/table/tbody/tr[2]/td/table[2]/tbody/tr[2]/td[2]/table/tbody/tr/td/table/tbody/tr[3]/td/table";
                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                        {
                                                            String tagTable = "td";
                                                            int quantidadeColunas = 6;
                                                            IWebElement baseTable = element;

                                                            // gets all table rows
                                                            IList rows = baseTable.FindElements(By.TagName(tagTable));

                                                            int quantidadeItem = rows.Count / quantidadeColunas;

                                                            if (quantidadeItem >= 1)
                                                            {
                                                                for (int i = 1; i < quantidadeItem; i++)
                                                                {
                                                                    VOCertame certame = new VOCertame();
                                                                    int index = 0;

                                                                    certame.Status = statusDoCertame;
                                                                    certame.Modalidade = VOCertame.MODALIDADE_PREGAO_ELETRONICO;
                                                                    certame.Sigla = VOCertame.MODALIDADE_PREGAO_ELETRONICO_SIGLA;
                                                                    certame.Numero = (((IWebElement)rows[index++ + quantidadeColunas * i]).Text).Replace("/", "");
                                                                    certame.Uasg = ((((IWebElement)rows[index++ + quantidadeColunas * i]).Text).Replace("/", ""));
                                                                    certame.Orgao = ((IWebElement)rows[index++ + quantidadeColunas * i]).Text;
                                                                    if (option == 0)
                                                                    {
                                                                        certame.DataAtualizacao = DateTime.Parse(((IWebElement)rows[index++ + quantidadeColunas * i]).Text);
                                                                        certame.Situacao = (((IWebElement)rows[index++ + quantidadeColunas * i]).Text);
                                                                        certame.InformacoesCertame = (((IWebElement)rows[index++ + quantidadeColunas * i]).Text);
                                                                    }

                                                                    //idElement = "/html/body/table/tbody/tr[2]/td/table[2]/tbody/tr[2]/td[2]/table/tbody/tr/td/table/tbody/tr[3]/td/table/tbody/tr[" + (i + 1) + "]/td[7]/a";
                                                                    idElement = "/html/body/table/tbody/tr[2]/td/table[2]/tbody/tr[2]/td[2]/table/tbody/tr/td/table/tbody/tr[3]/td/table/tbody/tr[" + (i + 1) + "]/td[6]/a";
                                                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                    {
                                                                        String currentContext = acaoSelenium.trocarDeContextoHandleWindow(driver, "Pregao/motiv_suspensao.asp");

                                                                        idElement = "/html/body/table/tbody/tr[3]/td/table/tbody/tr/td/table/tbody/tr[1]/td[2]";
                                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                        {
                                                                            //acaoSelenium.trocarDeContextoHandleWindow(driver, "Pregao/avisos4.asp");

                                                                            idElement = "/html/body/table/tbody/tr[3]/td/table/tbody/tr/td/table/tbody/tr[5]/td[2]";
                                                                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                            {
                                                                                certame.Mensagens += element.Text + System.Environment.NewLine;

                                                                                idElement = "/html/body/table/tbody/tr[3]/td/table/tbody/tr/td/table/tbody/tr[2]/td[2]";
                                                                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                                {
                                                                                    certame.Status = element.Text;

                                                                                    idElement = "/html/body/table/tbody/tr[3]/td/table/tbody/tr/td/table/tbody/tr[3]/td[2]";
                                                                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                                    {
                                                                                        DateTime data = DateTime.MinValue;
                                                                                        if (DateTime.TryParse(element.Text, out data))
                                                                                            certame.DataAtualizacao = data;

                                                                                        idElement = "btFechar";
                                                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID_CLICKABLE, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                                        {
                                                                                            acaoSelenium.retornaContextoHandleWindowPopUp(driver, currentContext);
                                                                                            driver.SwitchTo().Frame(1);
                                                                                        }
                                                                                    }
                                                                                }

                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            String context2 = acaoSelenium.trocarDeContextoHandleWindow(driver, "Pregao/avisos1.asp");
                                                                            driver.SwitchTo().Frame(1);

                                                                            idElement = "/html/body/table/tbody/tr/td[2]/p/a";
                                                                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                            {
                                                                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                                {
                                                                                    String currentContext2 = acaoSelenium.trocarDeContextoHandleWindow(driver, "Pregao/avisos4.asp");

                                                                                    idElement = "//*[@id='form1']/table/tbody/tr[1]/td/span";
                                                                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                                    {
                                                                                        DateTime data = DateTime.MinValue;
                                                                                        if (DateTime.TryParse(element.Text, out data))
                                                                                            certame.DataAtualizacao = data;

                                                                                        idElement = "//*[@id='form1']/table/tbody/tr[3]/td";
                                                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                                        {
                                                                                            certame.Mensagens += element.Text + System.Environment.NewLine;

                                                                                            idElement = "fechar";
                                                                                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_NAME_CLICKABLE, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                                            {
                                                                                                acaoSelenium.trocarDeContextoHandleWindow(driver, context2);

                                                                                                driver.SwitchTo().Frame(2);

                                                                                                idElement = "fechar";
                                                                                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_NAME_CLICKABLE, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                                                {
                                                                                                    acaoSelenium.retornaContextoHandleWindowPopUp(driver, currentContext);
                                                                                                }

                                                                                            }
                                                                                        }

                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                    bs.ListObject.Add(certame);
                                                                }

                                                            }
                                                            else
                                                            {
                                                                idElement = "voltar";
                                                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_NAME, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                                {
                                                                    repetir = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bs.Message.Add(element.Text);
                                                        bs.Result = BusinessService.RESULT_ERROR;
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    tryAgain = true;
                                                    if (contadorErrotryAgain > 3)
                                                        tryAgain = false;
                                                }
                                            } while (tryAgain);
                                        }
                                    }


                                }

                            }
                            else
                            {
                                if (contador > 3)
                                    repetir = false;
                            }


                        } while (repetir);


                    }

                }// for
            }

            return bs;
        }

        public BusinessService realizarPesquisalistaPregaoAtaFiltro(BusinessService _bs)
        {
            BusinessService bs = new BusinessService();

            bs.Operation = BusinessService.OPRATION_READ;

            ChromeOptions options = new ChromeOptions();
            int option = 0;


            //options.AddArguments("--proxy-server=138.94.71.202");
            //options.AddArgument(@"--incognito");
            options.AddArgument("--start-maximized");

            using (var driver = new ChromeDriver(options))
            {
                // Rastrear outros tipos de licitação
                //for (option = 0; option < 5; option++)
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=0
                // 	Pesquisa por: PREGÕES AGENDADOS
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=1
                // 	Pesquisa por: PREGÕES REALIZADOS, PENDENTES DE RECURSO/ ADJUDICAÇAO / HOMOLOGAÇÃO
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=2
                // 	Pesquisa por:  PREGÕES EM ANDAMENTO
                // http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=3
                // 	Pesquisa por:  PREGÕES REVOGADOS, ANULADOS OU ABANDONADOS

                IList listaUASG = listaUASGDnit();

                foreach (ComboItem objUasg in listaUASG)
                {


                    _bs.ObjectDriver = driver;
                    IWebDriver _driver = _bs.ObjectDriver;

                    // Pregões agendados
                    //  "http://comprasnet.gov.br/acesso.asp?url=/livre/Pregao/lista_pregao_filtro.asp?Opc=0";
                    String enderecoUrlString = _bs.ListObject[0].ToString();
                    VOCertame voCertame = (VOCertame)_bs.ListObject[1];
                    String idElement = "";
                    String keysSendElement = String.Empty;
                    IWebElement element = null;

                    AcaoSelenium acaoSelenium = new AcaoSelenium();

                    if ((acaoSelenium.goToUrl(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, enderecoUrlString, "", AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, idElement, false)))
                    {

                        driver.SwitchTo().Frame(1);

                        Boolean repetir = false;
                        int contador = 0;
                        do
                        {

                            if (!String.IsNullOrEmpty(voCertame.Uasg))
                            {
                                idElement = "co_uasg";
                                keysSendElement = objUasg.Value.ToString();

                                //keysSendElement = voCertame.Uasg;
                                acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_SEND_KEYS, ref element);
                            }

                            {

                                idElement = "numprp";
                                keysSendElement = voCertame.Numero;

                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_SEND_KEYS, ref element))
                                {

                                    idElement = "ok";
                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_NAME, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                    {

                                        idElement = "/html/body/table[1]/tbody/tr/td[2]/table[2]";
                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_2, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                        {
                                            //if ((element == null) || (element != null && !element.Text.ToLower().Contains("server error")))
                                            {
                                                //idElement = "/html/body/table/tbody/tr[2]/td/table[2]/tbody/tr[2]/td[2]/table/tbody/tr/td/table/tbody/tr[3]/td/table";
                                                //if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                {
                                                    String tagTable = "td";
                                                    int quantidadeColunas = 4;
                                                    IWebElement baseTable = element;

                                                    // gets all table rows
                                                    IList rows = baseTable.FindElements(By.TagName(tagTable));

                                                    int quantidadeItem = rows.Count / quantidadeColunas;

                                                    IList listaCertamesTabelaPesqisa1 = new ArrayList();

                                                    if (quantidadeItem > 1)
                                                    {
                                                        contador = 0;

                                                        for (int i = 1; i < quantidadeItem; i++)
                                                        {
                                                            VOCertame certame = new VOCertame();
                                                            int index = 0;

                                                            certame.Status = VOCertame.CERTAME_ATAS;
                                                            certame.Modalidade = VOCertame.MODALIDADE_PREGAO_ELETRONICO;
                                                            certame.Numero = (((IWebElement)rows[index++ + quantidadeColunas * i]).Text).Replace("/", "");
                                                            certame.Uasg = ((((IWebElement)rows[index++ + quantidadeColunas * i]).Text).Replace("/", ""));
                                                            certame.Orgao = ((IWebElement)rows[index++ + quantidadeColunas * i]).Text;
                                                            certame.DataCertame = DateTime.Parse(((IWebElement)rows[index++ + quantidadeColunas * i]).Text);

                                                            listaCertamesTabelaPesqisa1.Add(certame);
                                                        }

                                                        IList listaCertamesDNIT = new ArrayList();

                                                        // selecionar certames do dnit
                                                        for (int i = 0; i < listaCertamesTabelaPesqisa1.Count; i++)
                                                        {
                                                            VOCertame certame = (VOCertame)listaCertamesTabelaPesqisa1[i];
                                                            if (certame.Orgao.ToUpper().Contains("SUPERINTENDENCIA REGIONAL DO DNIT") || certame.Orgao.ToUpper().Contains("DEPTO. NAC. DE INFRA-ESTRUTURA DE TRANSPORTES"))
                                                            {
                                                                listaCertamesDNIT.Add(i);
                                                            }
                                                        }

                                                        IList listaCertamesSelecionados = new ArrayList();

                                                        // recolher informações certames DNIT
                                                        for (int i = 0; i < listaCertamesDNIT.Count; i++)
                                                        {
                                                            int enderecoInicial = 2;
                                                            int linha = int.Parse(listaCertamesDNIT[i].ToString());
                                                            idElement = "/html/body/table[1]/tbody/tr/td[2]/table[2]/tbody/tr[" + (enderecoInicial + linha).ToString() + "]/td[1]/a";
                                                            BusinessService bsConsultaAtaDoPregao = new BusinessService();
                                                            bsConsultaAtaDoPregao.ListObject.Add(idElement);
                                                            bsConsultaAtaDoPregao.ObjectDriver = _bs.ObjectDriver;
                                                            bsConsultaAtaDoPregao = consultaAtaDoPregao(bsConsultaAtaDoPregao);
                                                            if (bsConsultaAtaDoPregao.Result == BusinessService.RESULT_SUCESS)
                                                            {
                                                                VOCertame objVoCertame1 = (VOCertame)listaCertamesTabelaPesqisa1[int.Parse(listaCertamesDNIT[i].ToString())];
                                                                VOCertame objVoCertame2 = (VOCertame)bsConsultaAtaDoPregao.ListObject[0];
                                                                objVoCertame1.ListTabLotes = objVoCertame2.ListTabLotes;
                                                                listaCertamesSelecionados.Add(objVoCertame1);
                                                                bs.ListObject.Add(objVoCertame1);
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        idElement = "voltar";
                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_NAME, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                        {
                                                            repetir = true;
                                                            contador++;
                                                            if (contador > 3)
                                                                repetir = false;
                                                        }
                                                    }
                                                }
                                            }


                                        }
                                    }


                                }

                            }

                        } while (repetir);


                    }

                }// for
            }

            return bs;
        }


        private BusinessService consultaAtaDoPregao(BusinessService _bs)
        {
            VOCertame voCertame = new VOCertame();

            if (_bs != null)
            {
                if (_bs.ListObject.Count > 0)
                {
                    AcaoSelenium acaoSelenium = new AcaoSelenium();
                    IWebDriver _driver = _bs.ObjectDriver;
                    IWebElement element = null;

                    String idElement = _bs.ListObject[0].ToString();
                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, String.Empty, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                    {
                        idElement = "termodehomologacao";
                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, String.Empty, AcaoSelenium.TYPE_METHOD_FIND_BY_NAME, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                        {


                            idElement = "/html/body/table[1]/tbody/tr[5]/td";
                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, String.Empty, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                            {
                                String descricoesCabecalhoTermoHomologacao = element.Text;

                                int limite = int.MaxValue;
                                int lotes = 0;

                                // Encontra a quantidade de lotes.
                                for (int i = 1; i < limite; i++)
                                {
                                    idElement = "/html/body/table[2]/tbody/tr[" + (i).ToString() + "]/td/table[1]/tbody/tr[1]/td";
                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_3, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, String.Empty, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                    {
                                        lotes++;
                                    }
                                    else
                                    {
                                        // finaliza a procura
                                        limite = i;
                                    }
                                    i++;
                                }

                                // Adicionar lote

                                int indice = 1;
                                VOLote voLotes = null;

                                for (int i = 0; i < lotes; i++)
                                // for (int i = 0; i < 1; i++)
                                {
                                    voLotes = new VOLote();

                                    idElement = "/html/body/table[1]/tbody/tr[5]/td";
                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_3, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, String.Empty, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                    {
                                        String parteStr = converterMesExtensoEmNumero(element.Text);

                                        Regex rgx = new Regex(@"(\d{2}/\d{2}/\d{4})");
                                        DateTime dt = new DateTime();
                                        Match mat = rgx.Match(parteStr);
                                        if (DateTime.TryParse(mat.ToString(), out dt))
                                        {
                                            voLotes.DataHoraHomologacao = dt;
                                            rgx = new Regex(@"(\d{2}:\d{2})");
                                            mat = rgx.Match(parteStr);
                                            if (DateTime.TryParse(mat.ToString(), out dt))
                                            {
                                                voLotes.DataHoraHomologacao = voLotes.DataHoraHomologacao.AddHours(dt.Hour);
                                                voLotes.DataHoraHomologacao = voLotes.DataHoraHomologacao.AddMinutes(dt.Minute);
                                            }
                                        }

                                        // Descrição
                                        idElement = "/html/body/table[2]/tbody/tr[" + (indice).ToString() + "]/td/table[1]/tbody/tr[2]/td/table/tbody/tr[2]/td";
                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_3, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, String.Empty, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                        {
                                            voLotes.Descricao = element.Text.Replace("Descrição Complementar:", "");
                                            voLotes.Descricao = voLotes.Descricao.Trim();

                                            // Capturar informações auxiliares
                                            {
                                                String frase = voLotes.Descricao;
                                                String tag1 = "BR-";
                                                int index1 = frase.IndexOf(tag1) + tag1.Length;
                                                String tag2 = "/";
                                                int index2 = frase.IndexOf(tag2, index1);
                                                int resultado = index2 - index1;
                                                if (resultado <= 3)
                                                {
                                                    voLotes.Br = frase.Substring(index1, resultado).Trim();
                                                    voLotes.Uf = frase.Substring(index2 + tag2.Length, 2).Trim();
                                                }

                                                tag1 = "Segmento:";
                                                index1 = frase.IndexOf(tag1) + tag1.Length;
                                                tag2 = "ao";
                                                index2 = frase.IndexOf(tag2);
                                                resultado = index2 - index1;
                                                if (resultado > 3)
                                                {
                                                    if (resultado > 10)
                                                    {
                                                        tag1 = "Segmento 1:";
                                                        index1 = frase.IndexOf(tag1) + tag1.Length;
                                                        tag2 = "ao";
                                                        index2 = frase.IndexOf(tag2);
                                                        resultado = index2 - index1;
                                                    }
                                                    voLotes.KmInicio = frase.Substring(index1, index2 - index1).Trim();
                                                    voLotes.KmInicio = voLotes.KmInicio.ToUpper().Replace("km".ToUpper(), "").Replace("-", "").Trim();
                                                    index1 = index2 + tag2.Length;
                                                    tag2 = "Extensão:";
                                                    index2 = frase.IndexOf(tag2, index1);
                                                    voLotes.KmFim = frase.Substring(index1, index2 - index1).Trim();
                                                    voLotes.KmFim = voLotes.KmFim.ToUpper().Replace("km".ToUpper(), "").Replace("-", "").Trim();
                                                    if (voLotes.KmFim.Length > 10)
                                                    {
                                                        voLotes.KmFim = "-";
                                                        voLotes.Extensao = "Varios segmentos.";
                                                    }
                                                    else if (resultado > 3)
                                                    {
                                                        index1 = index2 + tag2.Length;
                                                        tag2 = "km";
                                                        index2 = frase.IndexOf(tag2, index1) + tag2.Length;
                                                        voLotes.Extensao = frase.Substring(index1, index2 - index1).Trim();
                                                        voLotes.Extensao = voLotes.Extensao.ToUpper().Replace("km".ToUpper(), "").Trim();
                                                    }
                                                }

                                            }

                                            // Valor estimado
                                            idElement = "/html/body/table[2]/tbody/tr[" + (indice).ToString() + "]/td/table[1]/tbody/tr[2]/td/table/tbody/tr[7]/td[1]";
                                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_3, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, String.Empty, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                            {
                                                voLotes.ValorEstimado = element.Text.Replace("Valor estimado:", "");

                                                // Empresa vencedora
                                                idElement = "/html/body/table[2]/tbody/tr[" + (indice).ToString() + "]/td/table[2]/tbody/tr/td";
                                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_3, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, String.Empty, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                {
                                                    String adjudicação = element.Text;
                                                    String tag = "Adjudicado para:";
                                                    int index1 = adjudicação.IndexOf(tag, 0) + tag.Length;
                                                    tag = ", pelo melhor lance de";
                                                    int index2 = adjudicação.IndexOf(tag, 0);
                                                    String empresa = adjudicação.Substring(index1, index2 - index1).Trim();
                                                    String valorRealizado = String.Empty;

                                                    if (adjudicação.Contains(", com valor negociado a"))
                                                    {
                                                        tag = ", com valor negociado a";
                                                        index1 = adjudicação.IndexOf(tag, 0) + tag.Length;
                                                    }
                                                    else
                                                    {
                                                        tag = ", pelo melhor lance de";
                                                        index1 = adjudicação.IndexOf(tag, 0) + tag.Length;

                                                    }
                                                    valorRealizado = adjudicação.Substring(index1, adjudicação.Length - index1 - 2).Trim();

                                                    voLotes.EmpresaVencedora = empresa;
                                                    voLotes.ValorRealizado = valorRealizado;

                                                }
                                            }
                                        }
                                    }
                                    indice += 2;

                                    voCertame.ListTabLotes.Add(voLotes);
                                }

                            }
                        }
                    }
                }
                else
                {
                    _bs.Result = BusinessService.RESULT_ERROR;
                    _bs.Message.Add("consultaAtaDoPregao: BusinessService sem itens na lista ListObject");
                }

            }
            else
            {
                _bs.Result = BusinessService.RESULT_ERROR;
                _bs.Message.Add("consultaAtaDoPregao: BusinessService nulo");
            }

            if (voCertame.ListTabLotes.Count > 0)
            {
                _bs.ListObject.Clear();
                _bs.ListObject.Add(voCertame);
                _bs.Result = BusinessService.RESULT_SUCESS;
            }

            return _bs;
        }

        private String converterMesExtensoEmNumero(String _str)
        {
            bool continuar = false;
            if (String.IsNullOrEmpty(_str))
                return String.Empty;

            _str = _str.ToUpper();

            IList mesExtenso = new ArrayList();
            mesExtenso.Add("JANEIRO");
            mesExtenso.Add("FEVEREIRO");
            mesExtenso.Add("MARÇO");
            mesExtenso.Add("ABRIL");
            mesExtenso.Add("MAIO");
            mesExtenso.Add("JUNHO");
            mesExtenso.Add("JULHO");
            mesExtenso.Add("AGOSTO");
            mesExtenso.Add("SETEMBRO");
            mesExtenso.Add("OUTUBRO");
            mesExtenso.Add("NOVEMBRO");
            mesExtenso.Add("DEZEMBRO");
            mesExtenso.Add("MARCO");

            foreach (string eachStr in mesExtenso)
            {
                if (_str.Contains(eachStr))
                {
                    continuar = true;
                    break;
                }
            }

            if (continuar == false)
            {
                return String.Empty;
            }

            if (_str.Contains("JANEIRO"))
            {
                _str = _str.Replace("JANEIRO", "/01/");
            }
            if (_str.Contains("FEVEREIRO"))
            {
                _str = _str.Replace("FEVEREIRO", "/02/");
            }
            if (_str.Contains("MARÇO") || _str.Contains("MARCO"))
            {
                _str = _str.Replace("MARÇO", "/03/").Replace("MARCO", "/03/");
            }
            if (_str.Contains("ABRIL"))
            {
                _str = _str.Replace("ABRIL", "/04/");
            }
            if (_str.Contains("MAIO"))
            {
                _str = _str.Replace("MAIO", "/05/");
            }
            if (_str.Contains("JUNHO"))
            {
                _str = _str.Replace("JUNHO", "/06/");
            }
            if (_str.Contains("JULHO"))
            {
                _str = _str.Replace("JULHO", "/07/");
            }
            if (_str.Contains("AGOSTO"))
            {
                _str = _str.Replace("AGOSTO", "/08/");
            }
            if (_str.Contains("SETEMBRO"))
            {
                _str = _str.Replace("SETEMBRO", "/09/");
            }
            if (_str.Contains("OUTUBRO"))
            {
                _str = _str.Replace("OUTUBRO", "/10/");
            }
            if (_str.Contains("NOVEMBRO"))
            {
                _str = _str.Replace("NOVEMBRO", "/11/");
            }
            if (_str.Contains("DEZEMBRO"))
            {
                _str = _str.Replace("DEZEMBRO", "/12/");
            }

            _str = _str.Replace("DE", "");
            _str = _str.Replace("º", "");
            _str = _str.Replace("ª", "");
            _str = _str.Replace("DO", "");
            _str = _str.Replace(" ", "");
            _str = _str.Replace("MÊS", "");
            _str = _str.Replace("MES", "");
            _str = _str.Replace(",", "");
            _str = _str.Replace("ANO", "");

            // Remover string entre parenteses
            if (_str.Contains("(") && _str.Contains(")"))
            {
                do
                {
                    int indexInicio = _str.IndexOf('(');
                    int indexFim = _str.IndexOf(')');

                    if (indexInicio > 0 && indexFim > 0)
                        _str = _str.Remove(indexInicio, indexFim - indexInicio + 1);

                } while (_str.Contains("(") && _str.Contains(")"));
            }

            return _str;
        }

        public BusinessService realizarPesquisaDNIT(BusinessService _bs)
        {
            BusinessService bs = new BusinessService();

            bs.Operation = BusinessService.OPRATION_READ;

            ChromeOptions options = new ChromeOptions();


            //options.AddArguments("--proxy-server=138.94.71.202");
            //options.AddArgument(@"--incognito");
            options.AddArgument("--start-maximized");

            using (var driver = new ChromeDriver(options))
            {
                IList listaCertames = _bs.ListObject;

                //foreach(VOCertame voCertame in listaCertames)
                {
                    _bs.ObjectDriver = driver;
                    IWebDriver _driver = _bs.ObjectDriver;


                    String enderecoUrlString = "http://www1.dnit.gov.br/editais/consulta/editais2.asp";
                    String idElement = "";
                    String keysSendElement = String.Empty;

                    IWebElement element = null;

                    AcaoSelenium acaoSelenium = new AcaoSelenium();

                    if ((acaoSelenium.goToUrl(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, enderecoUrlString, "", AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, idElement, false)))
                    {

                        //driver.SwitchTo().Frame(1);

                        Boolean repetir = true;
                        int contadorCertames = 0;

                        do
                        {
                            if (_bs.ListObject.Count > 0)
                            {
                                VOCertame certame = (VOCertame)_bs.ListObject[contadorCertames];
                                idElement = "//*[@id=\"yadcf-filter-wrapper--tabela-0\"]/span/span[1]/span/ul/li/input";

                                switch (certame.Uasg)
                                {
                                    case "393009":
                                        keysSendElement = "SUP. REG. DNIT AMAZONAS";
                                        break;
                                    case "393031":
                                        keysSendElement = "SUP. REG. DNIT MINAS GERAIS";
                                        break;
                                    case "393016":
                                        keysSendElement = "SUP. REG. DNIT PARÁ";
                                        break;
                                    case "393024":
                                        keysSendElement = "SUP. REG. DNIT CEARÁ";
                                        break;
                                    case "393029":
                                        keysSendElement = "SUP. REG. DNIT PERNANBUCO";
                                        break;
                                    case "393027":
                                        keysSendElement = "SUP. REG. DNIT BAHIA";
                                        break;
                                    case "393019":
                                        keysSendElement = "SUP. REG. DNIT RIO DE JANEIRO";
                                        break;
                                    case "39302S":
                                        keysSendElement = "SUP. REG. DNIT SÃO PAULO";
                                        break;
                                    case "393028":
                                        keysSendElement = "SUP. REG. DNIT PARANÁ";
                                        break;
                                    case "393012":
                                        keysSendElement = "SUP. REG. DNIT RIO GRANDE DO SUL";
                                        break;
                                    case "393020":
                                        keysSendElement = "SUP. REG. DNIT MATO GROSSO";
                                        break;
                                    case "393011":
                                        keysSendElement = "SUP. REG. DNIT GOIÁS E DISTRITO FEDERAL";
                                        break;
                                    case "393017":
                                        keysSendElement = "SUP. REG. DNIT PARAÍBA";
                                        break;
                                    case "393021":
                                        keysSendElement = "SUP. REG. DNIT RIO GRANDE DO NORTE";
                                        break;
                                    case "393030":
                                        keysSendElement = "SUP. REG. DNIT MARANHÃO";
                                        break;
                                    case "393013":
                                        keysSendElement = "SUP. REG. DNIT SEGIPE";
                                        break;
                                    case "393018":
                                        keysSendElement = "SUP. REG. ESPÍRITO SANTO";
                                        break;
                                    case "393022":
                                        keysSendElement = "SUP. REG. PIAUÍ";
                                        break;
                                    case "393010":
                                        keysSendElement = "SUP. REG. MATO GROSSO DO SUL";
                                        break;
                                    case "393026":
                                        keysSendElement = "SUP. REG. ALAGOAS";
                                        break;
                                    case "393014":
                                        keysSendElement = "SUP. REG. RONDÔNIA";
                                        break;
                                    case "393023":
                                        keysSendElement = "SUP. REG. TOCANTINS";
                                        break;
                                    case "390070":
                                        keysSendElement = "SUP. REG. RORAIMA";
                                        break;
                                    case "390071":
                                        keysSendElement = "SUP. REG. AMAPÁ";
                                        break;
                                    case "390084":
                                        keysSendElement = "SUP. REG. ACRE";
                                        break;
                                    case "393003":
                                        keysSendElement = "SEDE";
                                        break;
                                }

                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_SEND_KEYS, ref element))
                                {
                                    acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_2, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element);

                                    // enter
                                    element.SendKeys(Keys.Enter);

                                    idElement = "yadcf-filter--tabela-4";
                                    keysSendElement = certame.Numero.Replace("20", "/").Replace("19", "/");
                                    if (!keysSendElement.Contains("/"))
                                    {
                                        keysSendElement = keysSendElement.Insert(4, "/");
                                    }
                                    int tamanho = keysSendElement.Length;
                                    for (int i = 0; keysSendElement.Length < "0450/17".Length; i++)
                                    {
                                        keysSendElement = "0" + keysSendElement;
                                    }
                                    String uasg = certame.Uasg;
                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_SET_VALUE, ref element))
                                    {
                                        // Espera carregar
                                        idElement = "//*[@id=\"tabela\"]/tbody/tr/td";
                                        Boolean tryAgain = false;
                                        int contadorErro = 0;
                                        do
                                        {
                                            contadorErro++;
                                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                            {
                                                if (element.Text.ToUpper().Contains("NENHUM"))
                                                {
                                                    tryAgain = true;
                                                    Thread.Sleep(500);
                                                }
                                                else
                                                {
                                                    tryAgain = false;
                                                }
                                            }
                                            if (contadorErro > 10)
                                                tryAgain = false;

                                        } while (tryAgain);

                                        {
                                            idElement = "//*[@id=\"tabela\"]/tbody/tr";
                                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                            {
                                                String tagTable = "td";
                                                int quantidadeColunas = 7;
                                                IWebElement baseTable = element;

                                                // gets all table rows
                                                IList rows = baseTable.FindElements(By.TagName(tagTable));

                                                int quantidadeItem = rows.Count / quantidadeColunas;


                                                if (quantidadeItem > 0)
                                                {
                                                    for (int i = 0; i < quantidadeItem; i++)
                                                    {
                                                        int index = 0;

                                                        certame.Orgao = (((IWebElement)rows[index++ + quantidadeColunas * i]).Text).Replace("/", "");
                                                        certame.Modalidade = (((IWebElement)rows[index++ + quantidadeColunas * i]).Text).Replace("/", "");
                                                        IWebElement elementStatus = null;
                                                        idElement = @"//*[@id=""tabela""]/tbody/tr[" + (i + 1) + "]/td[3]/a";
                                                        acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref elementStatus);
                                                        certame.Status = elementStatus.GetAttribute("value");
                                                        index++;
                                                        //certame.Status = (((IWebElement)rows[index++ + quantidadeColunas * i]).Text).Replace("/", "");
                                                        certame.Numero = (((IWebElement)rows[index++ + quantidadeColunas * i]).Text).Replace("/", "");
                                                        String tempStr = ((((IWebElement)rows[index++ + quantidadeColunas * i]).Text).Replace("/", ""));

                                                        DateTime data = DateTime.MinValue;

                                                        if (!String.IsNullOrEmpty(tempStr) && !tempStr.Contains("/"))
                                                        {
                                                            tempStr = tempStr.Insert(2, "/");
                                                            tempStr = tempStr.Insert(5, "/");
                                                            data = ((tempStr != null) && DateTime.Parse(tempStr) != DateTime.MinValue) ? DateTime.Parse(tempStr) : certame.DataInicioProposta;
                                                        }

                                                        if (data != DateTime.MinValue)
                                                            certame.DataFimProposta = data;

                                                        certame.Objeto = ((IWebElement)rows[index++ + quantidadeColunas * i]).Text;

                                                        BusinessService bsTemp = new BusinessService();
                                                        bsTemp.ObjectDriver = _bs.ObjectDriver;
                                                        bsTemp.ListObject.Add(certame);

                                                        certame = preencherDadosCertame(bsTemp);

                                                        bs.ListObject.Add(certame);

                                                        bs.Result = BusinessService.RESULT_SUCESS;

                                                    }

                                                }
                                                else
                                                {
                                                    if (bs.Result == BusinessService.RESULT_EMPTY)
                                                    {
                                                        contadorCertames++;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }

                                if (bs.Result == BusinessService.RESULT_SUCESS)
                                {
                                    contadorCertames++;

                                    if ((acaoSelenium.goToUrl(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, enderecoUrlString, "", AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, idElement, false)))
                                    {
                                    }
                                }

                                if (contadorCertames >= _bs.ListObject.Count)
                                    repetir = false;
                            }
                            else
                            {
                                repetir = false;
                            }

                        } while (repetir);

                    }

                }// for
            }

            return bs;
        }

        VOCertame preencherDadosCertame(BusinessService _bs)
        {
            IWebDriver _driver = _bs.ObjectDriver;

            VOCertame certame = (VOCertame)_bs.ListObject[0];

            String idElement = "";
            String keysSendElement = String.Empty;

            IWebElement element = null;

            AcaoSelenium acaoSelenium = new AcaoSelenium();

            int contador = 0;

            idElement = "//*[@id=\"tabela\"]/tbody/tr/td[7]/a/img";
            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
            {

                // numero processo
                idElement = "//*[@id=\"collapseproc\"]/div/p[2]";
                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                {
                    certame.NumeroProcesso = element.Text;

                    //critério de julgamento
                    idElement = "//*[@id=\"collapseproc\"]/div/p[8]";
                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                    {
                        certame.CriterioJulgamento = element.Text;

                        //valor global
                        idElement = "//*[@id=\"collapseproc\"]/div/p[10]";
                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                        {
                            if (!element.Text.ToUpper().Contains("informado".ToUpper()))
                                certame.Valor = double.Parse(element.Text);

                            //data base orçamento
                            idElement = "//*[@id=\"collapseproc\"]/div/p[12]";
                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                            {
                                certame.DataBase = DateTime.Parse(element.Text);

                                // Tab Arquivos
                                idElement = "//*[@id=\"hdarq\"]/h4/a";
                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                {
                                    // Tabela de arquivos
                                    Boolean procuraAtiva = true;
                                    int contadorProcuraLinks = 2;
                                    DateTime dataHoraInicioDownloads = DateTime.Now;

                                    do
                                    {
                                        idElement = "//*[@id=\"collapsearq\"]/div/ul/li[" + contadorProcuraLinks + "]/a";
                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                        {

                                            if (!String.IsNullOrEmpty(element.Text))
                                            {
                                                String strURL = element.GetAttribute("href");
                                                String tagFind = "/";
                                                int indexName = strURL.LastIndexOf(tagFind) + tagFind.Length;
                                                WebClient req = new WebClient();

                                                String directory = (new IOObjetos().gerarDiretorioDB()) + certame.Numero.Replace(".", "").Replace("/", "").Replace("-", "");
                                                String nameFile = directory + @"\" + strURL.Substring(indexName, (strURL.Length - indexName));

                                                if (!System.IO.Directory.Exists(directory))
                                                {
                                                    Directory.CreateDirectory(directory);
                                                }

                                                // Verifica se existe o arquivo e deleta
                                                if (!System.IO.File.Exists(nameFile))
                                                    req.DownloadFileTaskAsync(strURL, nameFile);

                                                FileInfo file = new FileInfo(nameFile);

                                                Boolean aguardar = true;
                                                do
                                                {
                                                    if (file.Exists && (file.Length > 0))
                                                    {
                                                        aguardar = false;
                                                    }
                                                    file.Refresh();
                                                }
                                                while (aguardar);

                                                certame.ListTabArquivos.Add(nameFile);

                                                contadorProcuraLinks += 2;

                                                //acaoSelenium.retornaContextoHandleWindowPopUp(driver, currentContext);

                                            }
                                        }
                                        else
                                        {
                                            procuraAtiva = false;
                                        }

                                    } while (procuraAtiva);
                                    DateTime dataHoraFimDownloads = DateTime.Now;


                                    idElement = "//*[@id=\"hdlote\"]/h4/a";
                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                    {
                                        contador = 1;
                                        int numeroAnterior = 0;
                                        procuraAtiva = true;
                                        int contadorErroProcuraAtiva = 0;

                                        do
                                        {

                                            // downloads
                                            idElement = "lotes-button";
                                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                            {
                                                Boolean repetirLote = true;
                                                Boolean lerLote = true;
                                                int tentativasRealizadas = 0;

                                                do
                                                {
                                                    idElement = "ui-id-" + contador.ToString();
                                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_3, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                    {
                                                        repetirLote = false;
                                                    }
                                                    else
                                                    {

                                                        tentativasRealizadas++;
                                                        idElement = "lotes-button";
                                                        acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element);
                                                    }

                                                    if (tentativasRealizadas > 2)
                                                    {
                                                        repetirLote = false;
                                                        lerLote = false;
                                                    }

                                                } while (repetirLote);


                                                /////////////////////
                                                ///
                                                if (lerLote)
                                                {

                                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_3, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element))
                                                    {
                                                        VOLote voLote = new VOLote();

                                                        if (contador < 10)
                                                            idElement = "//*[@id=\"0" + contador.ToString() + "\"]/p[2]";
                                                        else
                                                            idElement = "//*[@id=\"" + contador.ToString() + "\"]/p[2]";

                                                        Boolean canRead = false;
                                                        if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                        {
                                                            canRead = true;
                                                        }
                                                        else
                                                        {
                                                            idElement = "//*[@id=\"Único\"]/p[2]";
                                                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                            {
                                                                canRead = true;
                                                            }
                                                        }

                                                        if (canRead)
                                                        {
                                                            voLote.Descricao = element.Text;

                                                            if (contador < 10)
                                                                idElement = "//*[@id=\"0" + contador.ToString() + "\"]/p[4]";
                                                            else
                                                                idElement = "//*[@id=\"" + contador.ToString() + "\"]/p[4]";


                                                            canRead = false;
                                                            if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                            {
                                                                canRead = true;
                                                            }
                                                            else
                                                            {
                                                                idElement = "//*[@id=\"Único\"]/p[4]";
                                                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                {
                                                                    canRead = true;
                                                                }
                                                            }

                                                            if (canRead)
                                                            {
                                                                voLote.ValorEstimado = element.Text;

                                                                if (contador < 10)
                                                                    idElement = "//*[@id=\"0" + contador.ToString() + "\"]/p[6]";
                                                                else
                                                                    idElement = "//*[@id=\"" + contador.ToString() + "\"]/p[6]";

                                                                canRead = false;
                                                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                {
                                                                    canRead = true;
                                                                }
                                                                else
                                                                {
                                                                    idElement = "//*[@id=\"Único\"]/p[6]";
                                                                    if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_5, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                                    {
                                                                        canRead = true;
                                                                    }
                                                                }

                                                                if (canRead)
                                                                {
                                                                    Boolean encontrado = false;
                                                                    for (int i = 0; i < certame.ListTabLotes.Count; i++)
                                                                    {
                                                                        VOLote objVoLote = (VOLote)certame.ListTabLotes[i];

                                                                        if (objVoLote.ValorEstimado.Contains(voLote.ValorEstimado))
                                                                        {
                                                                            encontrado = true;
                                                                            break;
                                                                        }
                                                                    }

                                                                    if (encontrado == false)
                                                                    {
                                                                        voLote.Prazo = element.Text;
                                                                        certame.ListTabLotes.Add(voLote);
                                                                    }

                                                                    numeroAnterior = contador;
                                                                    contador++;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            procuraAtiva = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        idElement = "lotes-button";
                                                        acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_NONE_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_3, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_ID, AcaoSelenium.TYPE_METHOD_ACTION_CLICK, ref element);

                                                        procuraAtiva = false;
                                                        _bs.Result = BusinessService.RESULT_SUCESS;
                                                    }

                                                } // ler lote
                                                else
                                                {
                                                    procuraAtiva = false;
                                                }
                                            }
                                            else
                                            {
                                                idElement = @"//*[@id='collapselote']/div";
                                                if (acaoSelenium.encontrarElementoAgir(_driver, AcaoSelenium.TIMEOUT_WAIT_1_SECOND, AcaoSelenium.NUMBER_ATTEMPTS_2, AcaoSelenium.TIME_SLEEP_OPERATION_NONE, idElement, keysSendElement, AcaoSelenium.TYPE_METHOD_FIND_BY_XPATH, AcaoSelenium.TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                                                {
                                                    if (element.Text.ToUpper().Contains("Nenhum anexo cadastrado".ToUpper()))
                                                    {
                                                        procuraAtiva = false;
                                                    }
                                                }
                                            }

                                        } while (procuraAtiva);

                                    }

                                }
                            }
                        }

                    }

                }
            }

            return certame;
        }

        String gerarIdCertame(VOCertame _certame)
        {
            return (_certame.Uasg.ToString().Trim() + _certame.SiglaModalidade.Trim() + _certame.Numero.Trim()).Replace("/", "");
        }

        private IList listaUASGDnit()
        {
            IList lista = new ArrayList();

            ComboItem item = new ComboItem();
            item.Text = "AM"; item.Value = "393009";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "PA"; item.Value = "393016";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "CE"; item.Value = "393024";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "PE"; item.Value = "393029";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "BA"; item.Value = "393027";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "MG"; item.Value = "393031";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "RJ"; item.Value = "3930l9";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "SP"; item.Value = "39302S";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "PR"; item.Value = "393028";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "RS"; item.Value = "393012";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "MT"; item.Value = "393020";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "GO/DF"; item.Value = "393011";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "PB"; item.Value = "393017";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "RN"; item.Value = "393021";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "MA"; item.Value = "393030";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "SE"; item.Value = "393013";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "ES"; item.Value = "393018";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "PI"; item.Value = "393022";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "MS"; item.Value = "393010";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "AL"; item.Value = "393026";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "SE"; item.Value = "39301S";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "RO"; item.Value = "393014";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "TO"; item.Value = "393023";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "RR"; item.Value = "390070";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "AP"; item.Value = "390071";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "AC"; item.Value = "390084";
            lista.Add(item);

            item = new ComboItem();
            item.Text = "SEDE"; item.Value = "393003";
            lista.Add(item);

            return lista;
        }

    }

    public class ComboItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
