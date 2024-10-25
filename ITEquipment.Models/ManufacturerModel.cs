using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Models
{
    public class ManufacturerModel
    {

        public int Id { get; set; }

        [Required]
        [DisplayName("Manufacturer ")]
        public string ManufacturerName { get; set; }
    }
    public class ManufacturerNameListModel
    {
        public int noOfRecords { get; set; }
        public List<ManufacturerModel> manufacturerModel { get; set; }
    }
}
