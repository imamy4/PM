using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    public class Пользователь : IЗаписьБД
    {
        public Пользователь()
        {
            Роли = new List<Роль>();
            СозданныеТребования = new HashSet<Требование>();
            Назначения = new HashSet<Назначение>();
            Активности = new HashSet<Активность>();
        }

        public virtual int Id { get; set; }
        public virtual string Имя { get; set; }
        public virtual string Фамилия { get; set; }

        public virtual string ФИО
        {
            get
            {
                return string.Format("{0} {1}", Имя, Фамилия);
            }
        }

        public virtual string Email { get; set; }
        public virtual string ХэшПароля { get; set; }
        public virtual DateTime ДатаРегистрации { get; set; }
        public virtual DateTime ДатаИзменения { get; set; }

        public virtual IList<Роль> Роли { get; protected set; }
        public virtual ISet<Требование> СозданныеТребования { get; protected set; }
        public virtual ISet<Назначение> Назначения { get; protected set; }
        public virtual ISet<Активность> Активности { get; protected set; }
    }
}
