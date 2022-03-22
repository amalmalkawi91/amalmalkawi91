using System;
using System.Collections.Generic;
using System.Text;

namespace Aquary.Models
{
    class Cls_Comments
    {
        public string operation_type { get; set; }
        public int ads_id { get; set; }
        public int comment_id { get; set; }
        public int fk_sub_service { get; set; }
        public int fk_register_id { get; set; }
        public string description { get; set; }
    }
}
