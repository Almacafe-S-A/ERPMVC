﻿using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class ControlPalletsDTO : ControlPallets
    {
        public int editar { get; set; } = 1;
       
    }
}