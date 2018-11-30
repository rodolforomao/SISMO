using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace BusinessSelenium
{
    public class AcaoSelenium
    {

        #region CONSTANTES

        public const int TYPE_METHOD_FIND_BY_ID = 1;
        public const int TYPE_METHOD_FIND_BY_CLASSNAME = 2;
        public const int TYPE_METHOD_FIND_BY_XPATH = 3;
        public const int TYPE_METHOD_FIND_BY_PARTIALTEXTLINK = 4;
        public const int TYPE_METHOD_FIND_BY_LINKTEXT = 5;
        public const int TYPE_METHOD_FIND_BY_NAME = 6;
        public const int TYPE_METHOD_FIND_BY_TAGNAME = 8;
        public const int TYPE_METHOD_FIND_BY_NAME_CLICKABLE = 7;
        public const int TYPE_METHOD_FIND_BY_XPATH_CLICKABLE = 9;
        public const int TYPE_METHOD_FIND_BY_ID_CLICKABLE = 10;


        public const int TYPE_METHOD_ACTION_NONE = 1;
        public const int TYPE_METHOD_ACTION_CLICK = 2;
        public const int TYPE_METHOD_ACTION_SET_VALUE = 3;
        public const int TYPE_METHOD_ACTION_GET_ELEMENT = 4;
        public const int TYPE_METHOD_ACTION_SEND_KEYS = 5;

        public const int TIMEOUT_WAIT_1_SECOND = 1;
        public const int TIMEOUT_WAIT_2_SECONDS = 2;
        public const int TIMEOUT_WAIT_NONE_SECOND = 0;

        public const int NUMBER_ATTEMPTS_1 = 1;
        public const int NUMBER_ATTEMPTS_2 = 2;
        public const int NUMBER_ATTEMPTS_3 = 3;
        public const int NUMBER_ATTEMPTS_5 = 5;
        public const int NUMBER_ATTEMPTS_10 = 10;
        public const int NUMBER_ATTEMPTS_20 = 20;
        public const int NUMBER_ATTEMPTS_30 = 30;
        public const int NUMBER_ATTEMPTS_50 = 50;
        public const int NUMBER_ATTEMPTS_5000 = 5000;

        public const int TIME_SLEEP_OPERATION_NONE = 0;
        public const int TIME_SLEEP_OPERATION_250_MILISECONDS = 250;
        public const int TIME_SLEEP_OPERATION_500_MILISECONDS = 500;
        public const int TIME_SLEEP_OPERATION_1_SECOND = 1000;
        public const int TIME_SLEEP_OPERATION_10_SECONDS = 10000;

        public const int DROPDOWNLIST_TYPE_SEARCH_EXACT_TEXT = 1;
        public const int DROPDOWNLIST_TYPE_SEARCH_EXACT_VALUE = 2;
        public const int DROPDOWNLIST_TYPE_SEARCH_CONTAINS_TEXT = 3;
        public const int DROPDOWNLIST_TYPE_SEARCH_CONTAINS_TEXT_IF_NOT = 4;
        public const int DROPDOWNLIST_TYPE_SEARCH_CONTAINS_TEXT_GET_TEXT = 5;
        public const int DROPDOWNLIST_TYPE_SEARCH_BY_INDEX = 6;
        public const int DROPDOWNLIST_TYPE_SEARCH_BY_ID_GET_TEXT = 7;

        public const int TYPE_VALIDATION_ONLY_NOT_NULL = 1;
        public const int TYPE_VALIDATION_ONE_ITEM = 2;
        public const int TYPE_VALIDATION_TWO_ITEM = 4;

        private const string PAGINA_FORNECEDOR = "/intro.htm";


        #endregion

        #region PUBLICO

        /// <summary>
        /// Procura elemento na página e caso encontre, toma a ação configurada.
        /// </summary>
        /// <param name="_driver">Driver do browser em questão</param>
        /// <param name="_waitTimeOutOperation">Tempo de espera da operação junto ao WebDriverWait</param>
        /// <param name="_numberAttempts">Número de tentativas da operação WebdriverWait.UrlToBe</param>
        /// <param name="_sleepTimeout">Tempo de dormencia para a nova tentativa</param>
        /// <param name="_elemName">Nome do elemento a ser encontrado</param>
        /// <param name="_value">Valor a ser configurado no element, caso necessite</param>
        /// <param name="_typeMethodFind">Tipo do método de procura</param>
        /// <param name="_typeAction">Tipo de ação a ser executada</param>
        /// <returns></returns>
        public bool encontrarElementoAgir(IWebDriver _driver, int _waitTimeOutOperation, int _numberAttempts, int _sleepTimeout, string _elemName, string _value, int _typeMethodFind, int _typeAction, ref IWebElement _elementTarget)
        {
            bool done = false;
            bool repeatOperation = false;
            int numberAttempts = 0;

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(_waitTimeOutOperation));

            if (String.IsNullOrEmpty(_elemName) || _driver == null)
                return false;

            do
            {
                IWebElement elementFinded = null;

                try
                {

                    numberAttempts++;
                    switch (_typeMethodFind)
                    {
                        case TYPE_METHOD_FIND_BY_ID:
                            elementFinded = wait.Until(ExpectedConditions.ElementIsVisible(By.Id(_elemName)));
                            break;
                        case TYPE_METHOD_FIND_BY_CLASSNAME:
                            elementFinded = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(_elemName)));
                            break;
                        case TYPE_METHOD_FIND_BY_XPATH:
                            elementFinded = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(_elemName)));
                            break;
                        case TYPE_METHOD_FIND_BY_PARTIALTEXTLINK:
                            elementFinded = wait.Until(ExpectedConditions.ElementIsVisible(By.PartialLinkText(_elemName)));
                            break;
                        case TYPE_METHOD_FIND_BY_LINKTEXT:
                            elementFinded = wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText(_elemName)));
                            break;
                        case TYPE_METHOD_FIND_BY_NAME:
                            elementFinded = wait.Until(ExpectedConditions.ElementIsVisible(By.Name(_elemName)));
                            break;
                        case TYPE_METHOD_FIND_BY_TAGNAME:
                            elementFinded = wait.Until(ExpectedConditions.ElementIsVisible(By.TagName(_elemName)));
                            break;
                        case TYPE_METHOD_FIND_BY_NAME_CLICKABLE:
                            elementFinded = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name(_elemName)));
                            break;
                        case TYPE_METHOD_FIND_BY_XPATH_CLICKABLE:
                            elementFinded = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(_elemName)));
                            break;
                        case TYPE_METHOD_FIND_BY_ID_CLICKABLE:
                            elementFinded = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(_elemName)));
                            break;
                    }
                    switch (_typeAction)
                    {
                        case TYPE_METHOD_ACTION_CLICK:
                            if (elementFinded != null)
                                elementFinded.Click();
                            break;
                        case TYPE_METHOD_ACTION_SET_VALUE:
                            if (elementFinded != null && !string.IsNullOrEmpty(_value))
                            {
                                elementFinded.Clear();
                                elementFinded.SendKeys(_value);
                            }
                            break;
                        case TYPE_METHOD_ACTION_GET_ELEMENT:
                            if (elementFinded != null)
                            {
                                _elementTarget = elementFinded;
                            }
                            break;
                        case TYPE_METHOD_ACTION_SEND_KEYS:
                            if (!String.IsNullOrEmpty(_value))
                            {
                                elementFinded.SendKeys(_value);
                            }
                            break;
                        default:
                            // noNe action
                            break;

                    }
                    // Executou localização e ação sem erros.
                    repeatOperation = false;
                    done = true;
                }
                catch (Exception _e)
                {
                    if (_e.Message.Contains("InsecureCertificate"))
                    {
                        if (tratamentoPaginaInsegura(_driver))
                        {
                            repeatOperation = false;
                            done = true;
                        }
                    }
                    else
                    {
                        repeatOperation = true;
                        done = false;
                    }

                }

                if (_sleepTimeout > 0 && repeatOperation)
                {
                    Thread.Sleep(_sleepTimeout);
                }

                if (numberAttempts >= _numberAttempts)
                {
                    repeatOperation = false;
                }

            } while (repeatOperation);


            return done;
        }

        /// <summary>
        /// Função que entra em site específico desde que o endereço no browser mude.
        /// </summary>
        /// <param name="_driver">Driver do browser em questão</param>
        /// <param name="_waitTimeOutOperation">Tempo de espera da operação junto ao WebDriverWait</param>
        /// <param name="_numberAttempts">Número de tentativas da operação WebdriverWait.UrlToBe</param>
        /// <param name="_sleepTimeout">Tempo de dormencia para a nova tentativa</param>
        /// <param name="_urlTarget">Endereço de destino</param>
        /// <param name="_urlTag">Endereço para comparação após página carregada</param>
        /// <param name="_typeSearchElement">Tipo de procura pelo elemento da página destino</param>
        /// <param name="_idElement">Aguarda o element da página destino carregar</param>
        /// <param name="_activeSecurite">Ativa ou não o http(S)</param>
        /// <returns>Retorna verdadeiro ou falso, caso o endereço web corresponda a URL destino</returns>
        public bool goToUrl(IWebDriver _driver, int _waitTimeOutOperation, int _numberAttempts, int _sleepTimeout, string _urlTarget, string _urlTag, int _typeSearchElement, String _idElement = null, bool _activeSecurite = false)
        {
            bool done = true;

            try
            {

                bool repeatOperation = false;
                int numberAttempts = 0;

                if (String.IsNullOrEmpty(_urlTarget) || _driver == null)
                    return false;

                if (string.IsNullOrEmpty(_urlTag))
                {
                    int indexLastBar = _urlTarget.LastIndexOf("/");
                    _urlTag = _urlTarget.Substring(indexLastBar + 1);
                }

                do
                {
                    try
                    {
                        numberAttempts++;
                        _driver.Navigate().GoToUrl(enderecoUrl(_urlTarget, _activeSecurite));
                        //new WebDriverWait(_driver, TimeSpan1.FromSeconds(_waitTimeOutOperation)).Until(ExpectedConditions.UrlToBe(_urlTarget));
                        IWebElement element = null;

                        if (!String.IsNullOrEmpty(_idElement))
                        {
                            // bool new AcaoSelenium().encontrarElementoAgir(IWebDriver _driver, int _waitTimeOutOperation, int _numberAttempts, int _sleepTimeout, string _elemName, string _value, int _typeMethodFind, int _typeAction, ref IWebElement _elementTarget)
                            done &= new AcaoSelenium().encontrarElementoAgir(_driver, TIMEOUT_WAIT_1_SECOND, NUMBER_ATTEMPTS_5, TIME_SLEEP_OPERATION_NONE, _idElement, "", _typeSearchElement, TYPE_METHOD_ACTION_NONE, ref element);
                        }
                    }
                    catch (Exception _e)
                    {
                        // Caso não tenha sido encontrada o elemento acima, cairá no try catch.
                        if (tratamentoPaginaInsegura(_driver))
                            repeatOperation = false;
                        else
                            repeatOperation = true;
                    }

                    if (_sleepTimeout > 0 && repeatOperation)
                    {
                        Thread.Sleep(_sleepTimeout);
                    }

                    if (numberAttempts >= _numberAttempts)
                    {
                        repeatOperation = false;
                    }

                } while (repeatOperation);

                if (!String.IsNullOrEmpty(_driver.Url) && _driver.Url.Contains(_urlTag))
                {
                    done &= true;
                }
                else
                {
                    done &= false;
                }
            }
            catch (Exception e)
            {
                done = false;
            }
            return done;
        }

        public Object DropDownList(IWebDriver _driver, String _idElement, String _value, int _attemptDo, int _typeSearch)
        {
            Object retorno = false;
            bool repetir = false;
            int tentativas = 0;

            IWebElement element = null;
            do
            {
                tentativas++;
                try
                {
                    if (tentativas >= _attemptDo)
                    {
                        repetir = false;
                    }

                    if (encontrarElementoAgir(_driver, TIMEOUT_WAIT_1_SECOND, NUMBER_ATTEMPTS_5, TIME_SLEEP_OPERATION_NONE, _idElement, String.Empty, TYPE_METHOD_FIND_BY_ID, TYPE_METHOD_ACTION_GET_ELEMENT, ref element))
                    {
                        if (element != null)
                        {
                            switch (_typeSearch)
                            {
                                case DROPDOWNLIST_TYPE_SEARCH_EXACT_TEXT:
                                    {
                                        new SelectElement(element).SelectByText(_value);
                                        retorno = true;
                                    }
                                    break;
                                case DROPDOWNLIST_TYPE_SEARCH_EXACT_VALUE:
                                    {
                                        new SelectElement(element).SelectByValue(_value);
                                        retorno = true;
                                    }
                                    break;
                                case DROPDOWNLIST_TYPE_SEARCH_CONTAINS_TEXT:
                                    {
                                        IList<IWebElement> AllDropDownList = element.FindElements(By.TagName("option"));
                                        int DpListCount = AllDropDownList.Count;
                                        for (int i = 0; i < DpListCount; i++)
                                        {
                                            if (removeAccents(AllDropDownList[i].Text.ToUpper()).Contains(removeAccents(_value).ToUpper()))
                                            {
                                                AllDropDownList[i].Click();
                                                retorno = true;
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case DROPDOWNLIST_TYPE_SEARCH_CONTAINS_TEXT_IF_NOT:
                                    {
                                        IList<IWebElement> AllDropDownList = element.FindElements(By.TagName("option"));
                                        int DpListCount = AllDropDownList.Count;
                                        for (int i = 0; i < DpListCount; i++)
                                        {
                                            if (!AllDropDownList[i].Text.Contains(_value))
                                            {
                                                AllDropDownList[i].Click();
                                                retorno = true;
                                                break;
                                            }
                                        }

                                    }
                                    break;
                                case DROPDOWNLIST_TYPE_SEARCH_CONTAINS_TEXT_GET_TEXT:
                                    {
                                        IList<IWebElement> AllDropDownList = element.FindElements(By.TagName("option"));
                                        int DpListCount = AllDropDownList.Count;
                                        for (int i = 0; i < DpListCount; i++)
                                        {
                                            if (AllDropDownList[i].Text.Contains(_value))
                                            {
                                                _value = AllDropDownList[i].Text;
                                                retorno = true;
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case DROPDOWNLIST_TYPE_SEARCH_BY_INDEX:
                                    {
                                        IList<IWebElement> AllDropDownList = element.FindElements(By.TagName("option"));
                                        int DpListCount = AllDropDownList.Count;
                                        int i = int.Parse(_value);
                                        if (i < AllDropDownList.Count)
                                        {
                                            AllDropDownList[i].Click();
                                            retorno = true;
                                        }
                                    }
                                    break;
                                case DROPDOWNLIST_TYPE_SEARCH_BY_ID_GET_TEXT:
                                    {
                                        IList<IWebElement> AllDropDownList = element.FindElements(By.TagName("option"));
                                        int DpListCount = AllDropDownList.Count;
                                        int i = int.Parse(_value);
                                        if (i < AllDropDownList.Count)
                                        {
                                            retorno = AllDropDownList[i].Text;
                                        }
                                    }
                                    break;

                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    if (tentativas >= _attemptDo)
                    {
                        repetir = false;
                    }
                    else
                    {
                        repetir = true;
                    }
                }
            } while (repetir);

            return retorno;
        }

        public string trocarDeContextoHandleWindow(IWebDriver driver, String _urlPopUp, Boolean _waitElements = true)
        {
            ////////////////////////////////// AGUARDA ABRIR POPUP E GUARDA A PÁGINA PRINCIPAL //////////////////////////////////

            // List<IWebElement> frames = new List<IWebElement>(driver.FindElements(By.TagName("frame")));

            if (_waitElements)
                while (driver.WindowHandles.Count < 2)
                    Thread.Sleep(250);

            string currentHandle = String.Empty;

            try
            {
                currentHandle = driver.CurrentWindowHandle;
            }
            catch (Exception e)
            { }

            foreach (string handle in driver.WindowHandles)
            {
                IWebDriver popup = driver.SwitchTo().Window(handle);

                if (popup.Url.Contains(_urlPopUp) || handle == _urlPopUp)
                {
                    break;
                }
            }

            return currentHandle;
        }

        public void retornaContextoHandleWindowPopUp(IWebDriver driver, string currentHandle)
        {

            // List<IWebElement> frames = new List<IWebElement>(driver.FindElements(By.TagName("frame")));

            try
            {
                if (driver.WindowHandles.Count > 1)
                {
                    driver.Close();
                }
                driver.SwitchTo().Window(currentHandle);
                driver.SwitchTo().DefaultContent();
            }
            catch (Exception e)
            {

            }
        }

        public Boolean validarBusinesService(BusinessService _bs, int _typeValidation)
        {
            Boolean retorno = true;

            switch (_typeValidation)
            {
                case TYPE_VALIDATION_ONLY_NOT_NULL:
                    {
                        if (_bs == null)
                            retorno = false;
                    }
                    break;
                case TYPE_VALIDATION_ONE_ITEM:
                case TYPE_VALIDATION_TWO_ITEM:
                    {
                        retorno = false;

                        if (_bs != null)
                        {
                            //if (_bs.ObjectDriver != null)
                            {
                                int countItems = 0;
                                if (_typeValidation == TYPE_VALIDATION_ONE_ITEM)
                                    countItems = 1;
                                else if (_typeValidation == TYPE_VALIDATION_TWO_ITEM)
                                    countItems = 2;

                                if (_bs.ListObject != null && _bs.ListObject.Count >= countItems)
                                {
                                    retorno = true;
                                }
                            }
                        }

                    }
                    break;

            }

            return retorno;
        }

        #endregion

        #region PRIVADO

        private string removeAccents(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        /// <summary>
        /// Tratamento para paginas http e https
        /// </summary>
        /// <param name="_driver"></param>
        /// <returns></returns>
        private bool tratamentoPaginaInsegura(IWebDriver _driver)
        {
            try
            {
                if (_driver.Url.Contains("https"))
                {
                    String enderecoUrlString = _driver.Url.Replace("https", "http");
                    string idElement = "nav";
                    goToUrl(_driver, TIMEOUT_WAIT_1_SECOND, NUMBER_ATTEMPTS_5, TIME_SLEEP_OPERATION_NONE, enderecoUrlString, "", TYPE_METHOD_FIND_BY_NAME, idElement);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            if (_driver.Url.Contains(PAGINA_FORNECEDOR))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Retorna um endereço com ou sem security http(S).
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_activeSecurite"></param>
        /// <returns></returns>
        private string enderecoUrl(String _url, bool _activeSecurite = false)
        {
            if (!_url.Contains("http://") && _activeSecurite == false)
            {
                _url = _url.Replace("https", "").Replace(":", "").Replace("//", "");
                _url = "http://" + _url;
            }
            return _url;
        }


        #endregion
    }
}
