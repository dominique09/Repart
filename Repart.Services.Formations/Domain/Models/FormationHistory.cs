using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repart.Services.Formations.Domain.Models
{
    public class FormationHistory
    {
        public Guid Id { get; protected set; }
        public DateTime UpdateAt { get; protected set; }
        public Guid UpdateBy { get; protected set; }
        public Formation Formation { get; protected set; }

        public FormationHistory(Guid updateBy, Formation formation)
        {
            Id = Guid.NewGuid();
            UpdateAt = DateTime.UtcNow;
            UpdateBy = updateBy;
            Formation = formation;
        }
    }
}
