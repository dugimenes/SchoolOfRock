using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagamento.Application.Command
{
    // IRequest<bool> significa que este comando, ao ser executado, retornará um booleano (sucesso/falha)
    public class ConfirmarPagamentoCommand : IRequest<bool>
    {
        public Guid PagamentoId { get; set; }

        // Em um cenário real, você poderia receber aqui os dados do cartão,
        // mas no nosso caso, vamos assumir que o pagamento já foi criado
        // com os dados do cartão e está pendente.
        // public string NumeroCartao { get; set; }
        // public string NomeTitular { get; set; }
        // ... etc
    }
}
