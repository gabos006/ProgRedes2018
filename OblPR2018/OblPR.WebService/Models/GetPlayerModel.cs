using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OblPR.WebService
{
    public class GetPlayerModel
    {
        public Guid Id;
        public string Nick;

        public GetPlayerModel(Guid id, string nick)
        {
            Id = id;
            Nick = nick;
        }
    }
}