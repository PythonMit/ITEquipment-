using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Models
{
    public class LocationModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Location")]
        public string LocationName { get; set; }
    }
    public class LocationModelListModel
    {
        public int noOfRecords { get; set; }
        public List<LocationModel> locationModels { get; set; }
    }
}

