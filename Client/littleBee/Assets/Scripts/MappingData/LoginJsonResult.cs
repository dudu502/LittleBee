using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronize.Game.Lockstep.Data{
    public class LoginJsonResultItem
    {
        public string name;
        public string pwd;
        public int money;
        public string date;
        public int state;
        public string GetPassword()
        {
            return pwd;
        }
        public string GetId()
        {
            return name;
        }
        public int GetMoney()
        {
            return money;
        }
        public int GetState()
        {
            return state;
        }
        public string GetDate()
        {
            return date;
        }
    }

    public class LoginJsonResult: RequestResult
    {
        public List<LoginJsonResultItem> results;

        public bool IsEmpty() { return results.Count == 0; }
    }
    public class RequestResult
    {
        public int state;
        public bool IsSuccess()
        {
            return state == 200;
        }
        public bool IsFailure()
        {
            return state == -1;
        }
    }
}
