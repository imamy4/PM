using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    public class КатегорияТребования : IЗаписьБД
    {
        public КатегорияТребования()
        {
            Требования = new HashSet<Требование>();
        }

        public virtual int Id { get; set; }
        public virtual string Название { get; set; }

        public virtual ISet<Требование> Требования { get; protected set; }
    }
}
