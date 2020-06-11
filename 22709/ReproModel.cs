using System.ComponentModel.DataAnnotations;

namespace repro
{
    public class ReproModel
    {
        [Required, Display(Name = "Password"), DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword"), DataType(DataType.Password)]
        [CompareProperty("Password")]
        public string ConfirmPassword { get; set; }
    }
}
