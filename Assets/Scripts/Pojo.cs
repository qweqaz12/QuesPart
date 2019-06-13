using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pojo
{
    public class Roletype
    {
        public long Id { get; set; }
        public string roletype { get;set;}
    }

     public class BaseQuestion
     {
         public long ID { get; set; }
         public string QuesID { get; set; }
         public string Title { get; set; }
         public string QusOption { get; set; }
         public int OptiCount { get; set; }
         public string CorrAns { get; set; }
         public string AnsGroup { get; set; }
         public string Score { get; set; }
         public int IsReject { get; set; }
         public string ExamTime { get; set; }
     }

    public class SingleQuestion : BaseQuestion
    {
        public string QuesSort { get; set; }
    }

    public class MultiQuestion : BaseQuestion
    {
        public string  AnsStyle{ get; set; }
    }

}

