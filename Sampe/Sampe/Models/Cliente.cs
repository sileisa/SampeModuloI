using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sampe.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }
        public String NomeCliente { get; set; }
    }
}