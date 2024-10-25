using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Utilities
{
    [Serializable]
    public class ResponseResult
    {
        public ResponseResult()
        {
        }
        private string _ResultMessage = string.Empty;
        private bool _Success = false;
        public bool Success
        {
            get { return _Success; }
            set { _Success = value; }
        }
        public string ResultMessage
        {
            get { return _ResultMessage; }
            set { _ResultMessage = value; }
        }
    }
    [Serializable]
    public class ResponseResult<T> : ResponseResult
    {
        protected T _GenericValue = default(T);
        public ResponseResult()
        {

        }
        public T Value
        {
            get { return _GenericValue; }
            set { _GenericValue = value; }
        }
    }
}
