using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace OblPR.WebService
{
    public class AddPlayerModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Nick;
        [Required(AllowEmptyStrings = false)]
        public string Image;

        public AddPlayerModel()
        {

        }
    }
}