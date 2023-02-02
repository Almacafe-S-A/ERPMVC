using ERPMVC.Models;
using System;
using System.Collections.Generic;

namespace ERPMVC.DTO
{
    public class FixedAssetDTO : FixedAsset
    {
        public List<FixedAsset> _FixedAsset { get; set; }
        public int editar { get; set; } = 1;
        public string token { get; set; }

        public int MotivoId { get; set; }

        public DateTime FechaBaja { get; set; }
    }
}
