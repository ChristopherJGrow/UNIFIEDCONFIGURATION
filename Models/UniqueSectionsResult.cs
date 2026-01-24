using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Models
{
    public class UniqueSectionsResult
    {
        public List<string> Sections { get; set; } = new List<string> ();
    }

    //public class UniqueSectionsRequest
    //{
    //    public string Environment { get; set; }
    //    public string Application { get; set; }
    //    public string Module { get; set; }
    //}
}
