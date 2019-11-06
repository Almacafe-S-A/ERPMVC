using ERPMVC.Models;
using System.Collections.Generic;

namespace ERPMVC.DTO
{
    public class DepreciationFixedAssetDTO : DepreciationFixedAsset
    {
        public List<DepreciationFixedAsset> _DepreciationFixedAsset { get; set; }
        public int editar { get; set; } = 1;
        public string token { get; set; }
    }
}
