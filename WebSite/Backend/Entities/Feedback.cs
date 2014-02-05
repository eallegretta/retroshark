using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RetroShark.Application.Backend.Entities
{
    public class Feedback: Entity<int>
    {
        public virtual Retrospective Retrospective { get; set; }
        public virtual FeedbackType Type { get; set; }
        [StringLength(int.MaxValue)]
        public virtual string Text { get; set; }
        public virtual int Score { get; set; }
    }

    public enum FeedbackType
    {
        Positive,
        Negative,
        Conclusion
    }
}