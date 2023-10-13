using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Liebra_Permana.Models.Tables;

public class Tr_QRCode
{

    public Tr_QRCode()
    {

    }

    public Tr_QRCode(QRCodeGenerateModel model, string type, string user)
    {

        switch (type)
        {
            case "generate":
                CreatedBy = model.CreatedBy;
                CreatedDate = model.CreatedDate;
                QRCode = model.QRCodeValue;
                break;
        }
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Column(TypeName = "varchar")]
    [StringLength(8)]
    [MinLength(8)]
    [MaxLength(8)]
    [Required]
    [Display(Name = "QR Code")]
    public string? QRCode { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "varchar")]
    [StringLength(50)]
    [Required]
    public string? CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    [Column(TypeName = "varchar")]
    [StringLength(50)]
    public string? UpdateBy { get; set; }

    [Column(TypeName = "varchar")]
    [StringLength(250)]
    public string? Remark { get; set; }
}
