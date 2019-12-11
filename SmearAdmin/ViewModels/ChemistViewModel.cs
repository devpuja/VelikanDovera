using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmearAdmin.ViewModels
{
    public class ChemistViewModel : ChemistStockistViewModel
    {
        public string MedicalName { get; set; }
        public string Class { get; set; }
    }
}