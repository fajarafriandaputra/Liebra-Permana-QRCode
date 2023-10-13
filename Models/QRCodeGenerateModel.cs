using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Liebra_Permana.Models;

public class QRCodeGenerateModel
{

    public Guid Id { get; set; }
    [StringLength(8, ErrorMessage = "The {0} minimum & maximum characters {2}", MinimumLength = 8)]
    [MinLength(8)]
    [MaxLength(8)]
    [Required]
    [Display(Name = "QR Code")]
    public string? QRCodeValue { get; set; }

    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; }


    [Display(Name = "Created by")]
    public string? CreatedBy { get; set; }
    public string? Remark { get; set; }
    public string? ImagecodeGenerator { get; set; }
}
