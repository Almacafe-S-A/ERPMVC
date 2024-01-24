using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.DTO
{
    public class DocumentoDTO
    {
        public string Title { get; set; }

        public string DocumentId { get; set; }


        public int DocumentoId { get; set; }

        public string DocumentType { get; set; }

        public int DocumentTypeId { get; set; }

        public string NumeroDEI { get; set; }

        public string Sucursal { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }


        public decimal Saldo { get; set; }

        public decimal SaldoImpuesto { get; set; }

        public int? JournalId { get; set; }

        public DateTime FechaDocumento { get; set; }

        public DateTime FechaVencimiento { get; set; }


        public Identificador Identificador { get; set; } 

        public decimal Total { get; set; }


    }


    public class Identificador
    {
        public int Id { get; set; }

        public int Tipo { get; set; }
    }
}

