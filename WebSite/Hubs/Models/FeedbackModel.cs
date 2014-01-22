using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RetroShark.Application.Backend.Entities;

namespace RetroShark.Application.Hubs.Models
{
    public class FeedbackModel
    {
        public string Id { get; set; }
        public FeedbackType Type { get; set; }
        public string Text { get; set; }
    }
}