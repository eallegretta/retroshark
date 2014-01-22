using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroShark.Application.Backend.Entities
{
    public class Retrospective : Entity<int>
    {
        public Retrospective()
        {
            Feedbacks = new List<Feedback>();
        }

        public virtual string Title { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string AdminCode { get; set; }
        public virtual string ParticipantCode { get; set; }
        public virtual IList<Feedback> Feedbacks { get; set; }
        public virtual IEnumerable<Feedback> PositiveFeedbacks
        {
            get { return Feedbacks.Where(x => x.Type == FeedbackType.Positive); }
        }
        public virtual IEnumerable<Feedback> NegativeFeedbacks
        {
            get { return Feedbacks.Where(x => x.Type == FeedbackType.Negative); }
        }
        public virtual IEnumerable<Feedback> Conclusions
        {
            get { return Feedbacks.Where(x => x.Type == FeedbackType.Conclusion); }
        }

        public static string GenerateUrlCode()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", string.Empty).Replace("+", string.Empty).Replace("/", string.Empty);
        }
    }
}