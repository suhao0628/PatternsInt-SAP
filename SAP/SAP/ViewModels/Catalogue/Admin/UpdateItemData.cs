using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SAP.ViewModels.Catalogue.Admin
{
    [XmlRoot("XmlItemData")]
    public class UpdateItemData
    {
        [XmlArray]
        [XmlArrayItem(ElementName = "CreateItemViewModel")]
        public List<CreateItemViewModel> CreateItemViewModelList { get; set; }
    }
}
