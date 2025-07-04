﻿using Aluno.Application.Command;
using Aluno.Domain.AggregateModel;
using Aluno.Infra.Repository;
using MediatR;
using SchoolOfRock.Contracts.Aluno.Events;

namespace Aluno.Application.Handlers
{
    public class MatricularAlunoCommandHandler : IRequestHandler<MatricularAlunoCommand, Guid>
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IMediator _mediator;
        private readonly IMatriculaRepository _matriculaRepository;

        public MatricularAlunoCommandHandler(IAlunoRepository alunoRepository, IMediator mediator, IMatriculaRepository matriculaRepository)
        {
            _alunoRepository = alunoRepository;
            _mediator = mediator;
            _matriculaRepository = matriculaRepository;
        }

        public async Task<Guid> Handle(MatricularAlunoCommand request, CancellationToken cancellationToken)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(request.AlunoId);
            if (aluno == null)
            {
                throw new Exception("Aluno não encontrado.");
            }

            var matricula = await _matriculaRepository.ObterPorAlunoECursoIdAsync(request.AlunoId, request.CursoId);

            if (matricula != null)
            {
                throw new Exception("Aluno já está matriculado neste curso.");
            }

            var dadosCartao = new DadosCartao(request.DadosCartao.Numero, request.DadosCartao.NomeTitular, request.DadosCartao.Expiracao, request.DadosCartao.Cvv);
            aluno.AtualizarDadosCartao(dadosCartao);
            _alunoRepository.Atualizar(aluno);

            var novaMatricula = new Matricula(request.CursoId, request.AlunoId);

            await _matriculaRepository.AdicionarAsync(novaMatricula);

            await _matriculaRepository.SaveChangesAsync();

            await _mediator.Publish(new AlunoMatriculadoEvent(novaMatricula.Id, aluno.Id, request.CursoId), cancellationToken);

            return novaMatricula.Id;
        }
    }
}