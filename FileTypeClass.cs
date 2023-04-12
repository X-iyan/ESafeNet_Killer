using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DeCodeApp
{
    public class FileTypeClass
    {
        public string strFile { get; set; }
        public List<string> decodeTypes { get; set; }
    }

    public class AllTypes
    {
        public List<FileTypeClass> fTypes { get; set; }
    }
}
