using ERPMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    //public class SolicitudCertificadoDepositoDTO:SolicitudCertificadoDeposito
    //{
    //    public List<Int64> RecibosAsociados { get; set; }
    //    public int editar { get; set; } = 1;
    //}


    public class SolicitudCertificadoDepositoDTO : SolicitudCertificadoDeposito
    {
        public List<Int64> RecibosAsociados { get; set; }
        public int editar { get; set; } = 1;

    }


}
