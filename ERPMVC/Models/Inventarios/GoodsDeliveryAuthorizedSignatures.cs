using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class GoodsDeliveryAuthorizedSignatures
    {
        public int Id { get; set; }

        public Int64 GoodsDeliveryAuthorizationId { get; set; }
        [ForeignKey("GoodsDeliveryAuthorizationId")]
        public GoodsDeliveryAuthorization GoodsDeliveryAuthorization { get; set; }

        public Int64 CustomerAuthorizedSignatureId { get; set; }
        [ForeignKey("CustomerAuthorizedSignatureId")]
        public CustomerAuthorizedSignature CustomerAuthorizedSignature { get; set; }

        public string NombreAutorizado { get; set; }
    }
}
