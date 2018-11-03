using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OblPR.WebService
{
    public class GetAllPlayersModel
    {
        public Guid Id;
        public string Nick;

        public GetAllPlayersModel(Guid id, string nick)
        {
            Id = id;
            Nick = nick;
        }
    }
}