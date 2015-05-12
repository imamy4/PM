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
            СозданныеТребования = new HashSet<Требование>();
            СозданныеЗадачи = new HashSet<Задача>();
            НазначенныеЗадачи = new HashSet<Задача>();
        }

        public virtual int Id { get; set; }
        public virtual string Имя { get; set; }
        public virtual string Фамилия { get; set; }
        public virtual string Email { get; set; }
        public virtual string ХэшПароля { get; set; }
        public virtual DateTime ДатаРегистрации { get; set; }
        public virtual DateTime ДатаИзменения { get; set; }

        public virtual ISet<Требование> СозданныеТребования { get; protected set; }
        public virtual ISet<Задача> СозданныеЗадачи { get; protected set; }
        public virtual ISet<Задача> НазначенныеЗадачи { get; protected set; }
        
        public virtual bool ИмеетРоль(string кодРоли)
        {
            System.Diagnostics.Debug.WriteLine("ИмеетРоль не определен.");
            return true;
        }
    }
}
