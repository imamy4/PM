﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МенеджерБД.Домен
{
    public class КатегорияТребования : IЗаписьБД
    {
        public virtual int Id { get; set; }
        public virtual string Название { get; set; }
    }
}
