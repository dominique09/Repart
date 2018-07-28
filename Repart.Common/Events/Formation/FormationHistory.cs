using System;
using System.Collections.Generic;
using System.Text;

namespace Repart.Common.Events.Formation
{
    public class FormationHistory
    {
        public Guid Id { get; }
        public DateTime UpdateAt { get; }
        public Guid UpdateBy { get; }
        public Formation Formation { get; }

        public FormationHistory(Guid id, DateTime updateAt, Guid updateBy, Formation formation)
        {
            Id = id;
            UpdateAt = updateAt;
            UpdateBy = updateBy;
            Formation = formation;
        }
    }
}
