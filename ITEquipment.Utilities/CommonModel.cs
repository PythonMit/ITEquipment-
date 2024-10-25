using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITEquipment.Utilities
{
    public class CommonModel
    {
    }
    public class JQueryDataTableParamModel
    {
        /// <summary>
        /// Request sequence number sent by DataTable,
        /// same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string searchdata { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }
        public string iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; }
        #region Booking
        public string UserSearch { get; set; }
        public string EquipmentSearch { get; set; }
        public string StatusSearch { get; set; }
        public string StartDateSearch { get; set; }
        public string EndDateSearch { get; set; }
        #endregion
        #region Accessory
        public string AccessoryName { get; set; }
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }
        #endregion
        #region
        public string Email { get; set; }
        public string Role { get; set; }

        #endregion
        #region Equipment
        public string EquipmentName { get; set; }
        public string AssetsTag { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        #endregion
        #region Manufacturer
        public string ManufacturerName { get; set; }
        #endregion
        #region Location
        public string LocationName { get; set; }
        #endregion
    }
}
