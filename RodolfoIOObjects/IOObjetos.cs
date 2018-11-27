using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using BusinessSelenium;
using System.Globalization;

namespace Persistencia
{
    public class IOObjetos
    {

        #region CONST

        public const string WARNING_PRIMEIRO_OBJETO = "PRIMEIRO OBJETO";

        #region NAME FILE DB

        private const string DB_X = "DB_X.rod";

        #endregion

        #endregion

        #region PUBLIC

        public BusinessService CRUDObjetos(BusinessService _bs)
        {
            if (_bs != null)
            {
                switch (_bs.Operation)
                {
                    case BusinessService.OPRATION_SAVE:
                    case BusinessService.OPRATION_CREATE:
                    case BusinessService.OPRATION_UPDATE:
                        {
                            if (_bs.ListObject != null && _bs.ListObject.Count > 0)
                            {
                                BusinessService bs = new BusinessService();
                                bs.ListObject = new ArrayList();
                                bs.ListObject.Add(_bs.ListObject[0]);
                                bs = enderecoArquivo(bs);
                                if (bs.Result == BusinessService.RESULT_SUCESS)
                                {
                                    bs.ListObject.Add(_bs.ListObject[1]);
                                    bs = Save(bs);

                                    _bs.Result = bs.Result;
                                    if (bs.Result == BusinessService.RESULT_WARNING)
                                        _bs.Message = bs.Message;
                                }

                            }
                        }
                        break;
                    case BusinessService.OPRATION_READ:
                        {
                            BusinessService bs = new BusinessService();
                            bs.ListObject = new ArrayList();
                            bs.ListObject.Add(_bs.ListObject[0]);
                            bs = enderecoArquivo(bs);
                            if (bs.Result == BusinessService.RESULT_SUCESS)
                            {
                                bs = Load(bs);
                                if(bs.Result == BusinessService.RESULT_SUCESS)
                                    _bs.ListObject = bs.ListObject;
                                else
                                    _bs.ListObject = new ArrayList();
                                _bs.Result = bs.Result;
                                _bs = new BusinessService().verifyWarning(_bs, bs);
                            }
                        }
                        break;

                }

            }
            return _bs;
        }
        

        public string gerarDiretorio(String _subDirectorie)
        {
            return gerarDiretorioRoot() + _subDirectorie + "\\";
        }

        public string gerarDiretorioRoot()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public string gerarDiretorioDB()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "database\\";
        }


        #endregion

        #region PRIVATE 

        #region ADDRESS

        private BusinessService enderecoArquivo(BusinessService _bs)
        {
            _bs.Result = BusinessService.RESULT_EMPTY;

            try
            {
                int opcao = (int)_bs.ListObject[0];

                string fileName = AppDomain.CurrentDomain.BaseDirectory;

                // Cria o diretório caso ele não exista
                if (!Directory.Exists(fileName))
                    Directory.CreateDirectory(fileName);

                fileName += DB_X.Replace("X", opcao.ToString());

                _bs.ListObject.Clear();
                _bs.ListObject.Add(fileName);

                _bs.Result = BusinessService.RESULT_SUCESS;

            }
            catch (Exception e)
            {
                _bs.Result = BusinessService.RESULT_ERROR;
            }

            return _bs;
        }

        #endregion

        #region IO

        public BusinessService Save(BusinessService _bs)
        {
            _bs.Result = BusinessService.RESULT_EMPTY;

            string filename = _bs.ListObject[0].ToString();
            filename = indentPhrase(filename);
            IList _lista = (IList)_bs.ListObject[1];

            try
            {

                FieldInfo[] fields = _lista.GetType().GetFields(BindingFlags.Static | BindingFlags.Public);
                object[,] a = new object[fields.Length, 2];
                Stream f;
                if (File.Exists(filename))
                    f = File.Open(filename, FileMode.OpenOrCreate);
                else
                    f = File.Create(filename);
                SoapFormatter formatter = new SoapFormatter();
                formatter.Serialize(f, _lista);
                f.Close();

                _bs.Result = BusinessService.RESULT_SUCESS;

            }
            catch(Exception e)
            {
                _bs.Result = BusinessService.RESULT_ERROR;
                _bs.Message.Add(e.Message);
            }

            return _bs;
        }


        public BusinessService Load(BusinessService _bs)
        {
            _bs.Result = BusinessService.RESULT_EMPTY;

            string filename = _bs.ListObject[0].ToString();
            filename = indentPhrase(filename);

            IList lista = new ArrayList();

            try
            {
                FieldInfo[] fields = lista.GetType().GetFields(BindingFlags.Static | BindingFlags.Public);
                Stream f = File.Open(filename, FileMode.Open);
                SoapFormatter formatter = new SoapFormatter();
                lista = formatter.Deserialize(f) as IList;
                f.Close();

                _bs.ListObject = lista;
                _bs.Result = BusinessService.RESULT_SUCESS;

            }
            catch (Exception e)
            {
                if (e.Message.ToUpper().Contains("NÃO FOI POSSÍVEL LOCALIZAR O ARQUIVO"))
                {
                    // Não existe arquivo ainda, não é um erro pois quando for o primeiro objeto a ser salvo realmente não será encontrado.
                    _bs.Result = BusinessService.RESULT_WARNING;
                    _bs.Message.Add(WARNING_PRIMEIRO_OBJETO);
                }
                else
                    _bs.Result = BusinessService.RESULT_ERROR;
            }

            return _bs;
        }

        public Boolean lastChange(BusinessService _bs)
        {
            if (_bs != null && _bs.ListObject.Count > 1)
            {
                string filename = indentPhrase(_bs.ListObject[0].ToString());
                int _horas = (int)_bs.ListObject[1];

                try
                {
                    DateTime FileGetLastWriteTime = File.GetLastWriteTime(AppDomain.CurrentDomain.BaseDirectory + "//" + filename);
                    if (DateTime.Now.AddHours(-_horas) > FileGetLastWriteTime)
                        return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return false;
        }

        #endregion

        #region AUXILIARY

        public string indentPhrase(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }

            sbReturn = sbReturn.Replace(" ", "");

            return sbReturn.ToString().ToLower();
        }

        #endregion

        #endregion
    }
}
