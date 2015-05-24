using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    public class Роль : IЗаписьБД
    {
        public Роль()
        {
            Пользователи = new List<Пользователь>();
        }

        public virtual int Id { get; set; }
        public virtual string Название { get; set; }

        public virtual Проект Проект { get; set; }

        public virtual IList<Пользователь> Пользователи { get; protected set; }

        public const string Администратор = "Администратор";
        public const string МенеджерПроекта = "Менеджер проекта";
        public const string Пользователь = "Пользователь";
    }
}
