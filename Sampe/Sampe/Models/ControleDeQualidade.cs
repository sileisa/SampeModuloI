using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sampe.Models
{
    public class ControleDeQualidade
    {
        [Key]
        public int ControleDeQualidadeId { get; set; }
        public float Ciclo { get; set; }
        public String Hora { get; set; }
        public float PesoDaPeca { get; set; }
        public Boolean Peso { get; set; }
        public Boolean Cor { get; set; }
        public Boolean Dimensao { get; set; }
        public String Assinatura { get; set; }
        public Boolean Liberado { get; set; }
        [ForeignKey("OrdemProducaoPeca")]
        public String OrdemProducaoPecaId { get; set; }
        public OrdemProducaoPeca OrdemProducaoPeca { get; set; }

        //Se todos os campos de inspeção estiverem 
        //preenchidos retorna true para liberar o formulário 
        public Boolean ValidaInspecao()
        {
            if (Peso && Cor && Dimensao)
                return true;

            return false;
        }

    }
}