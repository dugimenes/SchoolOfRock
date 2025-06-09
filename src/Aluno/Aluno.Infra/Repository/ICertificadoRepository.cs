using Aluno.Domain.AggregateModel;

namespace Aluno.Infra.Repository
{
    public interface ICertificadoRepository
    {
        Task<Certificado> ObterPorIdAsync(Guid id);

        Task<IEnumerable<Certificado>> ObterTodosAsync();

        Task AdicionarAsync(Certificado certificado);

        void Atualizar(Certificado certificado);

        void Remover(Certificado certificado);

        Task<int> SaveChangesAsync();

        Task<IEnumerable<Certificado>> ObterPorAlunoIdAsync(Guid alunoId);

        Task<IEnumerable<Certificado>> ObterPorCursoIdAsync(Guid cursoId);

        Task<Certificado> ObterPorCursoEAlunoIdAsync(Guid alunoId, Guid cursoId);
    }
}
