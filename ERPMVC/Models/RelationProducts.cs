﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPAPI.Models
{
    public class ProductRelation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 RelationProductId { get; set; }
        public Int64 ProductId { get; set; }
        public Int64 SubProductId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }

    }



}
