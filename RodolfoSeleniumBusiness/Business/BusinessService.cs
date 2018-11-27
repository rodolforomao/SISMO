using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessSelenium
{

    [Serializable]
    public class BusinessService
    {
        private const string OPERATION_DB = "_DB";
        private const string OPERATION_EXCEL = "_EXCEL";

        public const string OPRATION_CREATE = "CREATE" + OPERATION_DB;
        public const string OPRATION_SAVE = "SAVE" + OPERATION_DB;
        public const string OPRATION_READ = "READ" + OPERATION_DB;
        public const string OPRATION_UPDATE = "UPDATE" + OPERATION_DB;
        public const string OPRATION_DELETE = "DELETE" + OPERATION_DB;
        public const string OPRATION_IMPORT_EXCEL = "IMPORT" + OPERATION_EXCEL;
        public const string OPRATION_IMPORT_NONE = "NONE";

        public const string RESULT_SUCESS = "SUCESS";
        public const string RESULT_ERROR = "ERROR";
        public const string RESULT_WARNING = "WARNING";
        public const string RESULT_EMPTY = "EMPTY";

        private string operation;
        private string result = RESULT_ERROR;
        private IList listObject;
        private IList mesage;
        private IWebDriver objectDriver;
        private String lastContextHandle;

        public BusinessService()
        {
            result = RESULT_EMPTY;
            operation = OPRATION_IMPORT_NONE;
        }

        public string Operation
        {
            get
            {
                return operation;
            }

            set
            {
                operation = value;
            }
        }

        public string Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
            }
        }

        public IList ListObject
        {
            get
            {
                if (listObject == null)
                    listObject = new ArrayList();
                return listObject;
            }

            set
            {
                if (listObject == null)
                    listObject = new ArrayList();
                listObject = value;
            }
        }

        public IList Message
        {
            get
            {
                if (mesage == null)
                    mesage = new ArrayList();
                return mesage;
            }

            set
            {
                if (mesage == null)
                    mesage = new ArrayList();
                mesage = value;
            }
        }

        public IWebDriver ObjectDriver
        {
            get
            {
                return objectDriver;
            }

            set
            {
                objectDriver = value;
            }
        }

        public string LastContextHandle
        {
            get
            {
                return lastContextHandle;
            }

            set
            {
                lastContextHandle = value;
            }
        }

        public BusinessService verifyWarning(BusinessService _bsOld, BusinessService _bsNew)
        {
            if (_bsNew.result == RESULT_WARNING)
                _bsOld.Message = _bsNew.Message;
            return _bsOld;
        }

    }
}
