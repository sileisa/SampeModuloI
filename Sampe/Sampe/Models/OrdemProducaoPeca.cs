using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sampe.Models
{
    public class OrdemProducaoPeca
    {
    
        [Key]
        public String CodigoIdentificador { get; set; }
        //Codigo alfanumérico que identifica os formulários
        [ForeignKey("Expectativa")]
        public int ExpectativaId { get; set; }
        public Expectativa Expectativa { get; set; }
        public int OPnoMes { get; set; }
        public DateTime Data { get; set; }
        [Required]
        public String MateriaPrima { get; set; }
        public String MPLote { get; set; }
        public Double MPConsumo { get; set; }
        public String ProdIncio { get; set; }
        public String ProdFim { get; set; }
        //Substituir por chave estrangeira MáquinaID
        public String Maquina { get; set; }
        public String Produto { get; set; }
        public String ProdutoCor { get; set; }
        public String MasterLote { get; set; }
        public String Fornecedor { get; set; }
        public Double TempAgua { get; set; }
        public Double NivelOleo { get; set; }
        public Double Galho { get; set; }
        public Double OffSpec { get; set; }
        public Double RefugoKg { get; set; }
        public int UnidadesProduzidas { get; set; }
        public Double ContadorInicial { get; set; }
        public Double ContadorFinal { get; set; }
        //Colection de assinaturas válidas para o controle de qualidade 
        public ICollection<ControleDeQualidade> ControleDeQualidade { get; set; }
        public ICollection<Parada> Paradas { get; set; }
        public ICollection<int> ParadasId { get; set; }
        public Parada Parada { get; set; }



        public void ConfiguracaoInicial()
        {
            Produto = Expectativa.Produto;
            if (MateriaPrima.Equals("Refugo"))
            {
                MasterLote = "";
                Fornecedor = "";
                MPLote = "Refugo";
            }
            
            GerarCodigo();
        }

        public void GerarCodigo()
        {
            //Ordem de concatenação "I",Ano,Mes,Ordem coronológica das ops no mês
            int ano=0;
            if(Data.Year > 2017)
            {
                ano = (Data.Year - 2017)+1;
            }
          
         CodigoIdentificador = String.Concat("I", ano.ToString(), CodigoMes(), OPnoMes.ToString());
         }
       
        public String CodigoMes()
        {
            String mes = "";    
            switch (Data.Month)
            {
                case 1:
                     mes = "A";
                    break;
                case 2:
                    mes = "B";
                    break;
                case 3:
                    mes = "C";
                    break;
                case 4:
                    mes = "D";
                    break;
                case 5:
                    mes = "E";
                    break;
                case 6:
                    mes = "F";
                    break;
                case 7:
                    mes = "G";
                    break;
                case 8:
                    mes = "H";
                    break;
                case 9:
                    mes = "I";
                    break;
                case 10:
                    mes = "J";
                    break;
                case 11:
                    mes = "K";
                    break;
                case 12:
                    mes = "L";
                    break;
               
            }
            return mes;
            
        }


    }
}