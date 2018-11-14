using System.ComponentModel.DataAnnotations;

namespace OblPR.WebService
{
    public class UpdatePlayerModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Image;

        public UpdatePlayerModel()
        {

        }
    }
}