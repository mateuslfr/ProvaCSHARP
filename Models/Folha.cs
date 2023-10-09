namespace Prova.Models
{
    public class Folha
    {

        public int Id { get; set; }
        public int Valor { get; set; }
        public int Quantidade { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public Double SalarioBruto { get; set; }
        public Double ImpostoIrrf { get; set; }
        public Double ImpostoInss { get; set; }
        public Double ImpostoFgts { get; set; }
        public Double SalarioLiquido { get; set; }
        public int FuncionarioId { get; set; }
        public Funcionario? Funcionario { get; set; }
    }
}