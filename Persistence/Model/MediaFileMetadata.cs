using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Model
{
    public class MediaFileMetadata
    {
        public Guid Id { get; set; }
        public string? Station { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? Content { get; set; }
        public string? Notes { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? PublishTime { get; set; }
        public string? Genre { get; set; }
        public string? Frequency { get; set; }
    }
}
