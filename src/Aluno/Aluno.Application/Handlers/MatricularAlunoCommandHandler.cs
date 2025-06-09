using Aluno.Application.Command;
using Aluno.Domain.AggregateModel;
using Aluno.Infra;
using Aluno.Infra.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Pagamento.Infra;
using Pagamento.Infra.Repository;

namespace Aluno.Application.Handlers
{
    public class MatricularAlunoCommandHandler : IRequestHandler<MatricularAlunoCommand, MatriculaRealizadaDto>
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IPagamentoRepository _pagamentoRepository;

        private readonly AlunoDbContext _alunoDbContext;
        private readonly PagamentoDbContext _pagamentoDbContext;

        public MatricularAlunoCommandHandler(IAlunoRepository alunoRepository, IMatriculaRepository matriculaRepository, 
                                                IPagamentoRepository pagamentoRepository, AlunoDbContext alunoDbContext, PagamentoDbContext pagamentoDbContext)
        {
            _alunoRepository = alunoRepository;
            _matriculaRepository = matriculaRepository;
            _pagamentoRepository = pagamentoRepository;
            _alunoDbContext = alunoDbContext;
            _pagamentoDbContext = pagamentoDbContext;
        }

        public async Task<MatriculaRealizadaDto> Handle(MatricularAlunoCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _alunoDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await _pagamentoDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction(), cancellationToken);

                var aluno = await _alunoRepository.ObterPorIdAsync(request.AlunoId);
                if (aluno == null)
                {
                    throw new Exception("Aluno não encontrado.");
                }

                var matriculaExistente = await _matriculaRepository.ObterPorAlunoECursoIdAsync(request.AlunoId, request.CursoId);
                if (matriculaExistente != null)
                {
                    throw new Exception("Aluno já está matriculado neste curso.");
                }

                var dadosCartao = new DadosCartao(request.DadosCartao.Numero, request.DadosCartao.NomeTitular, request.DadosCartao.Expiracao, request.DadosCartao.Cvv);
                aluno.AtualizarDadosCartao(dadosCartao);
                _alunoRepository.Atualizar(aluno);

                var novaMatricula = new Matricula(request.CursoId, request.AlunoId);
                await _matriculaRepository.AdicionarAsync(novaMatricula);

                var novoPagamento = new Pagamento.Domain.AggregateModel.Pagamento(novaMatricula.Id);
                await _pagamentoRepository.AdicionarAsync(novoPagamento);

                await _alunoDbContext.SaveChangesAsync(cancellationToken);
                await _pagamentoDbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new MatriculaRealizadaDto
                {
                    MatriculaId = novaMatricula.Id,
                    PagamentoId = novoPagamento.Id
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}