using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccountLibrary.Entity
{
    public class BlobData
    {
        [DisplayName("Blob Name")]
        public string name { get; set; }

        public Uri path { get; set; }

        
    }
}
