using MediatR;
using Moq;
using Pagamento.Application.Command;
using Pagamento.Application.Handlers;
using Pagamento.Domain.AggregateModel;
using Pagamento.Infra.Repository;
using SchoolOfRock.Contracts.Pagamento.Events;
using Xunit;

namespace SchoolOfRock.Tests.Pagamento.Application
{
    public class ConfirmarPagamentoCommandHandlerTests
    {
        private readonly Mock<IPagamentoRepository> _pagamentoRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConfirmarPagamentoCommandHandler _handler;

        public ConfirmarPagamentoCommandHandlerTests()
        {
            _pagamentoRepositoryMock = new Mock<IPagamentoRepository>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new ConfirmarPagamentoCommandHandler(_pagamentoRepositoryMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Confirmar_Pagamento_E_Publicar_Evento()
        {
            // Arrange
            var pagamentoId = Guid.NewGuid();
            var matriculaId = Guid.NewGuid();
            var pagamento = new global::Pagamento.Domain.AggregateModel.Pagamento(matriculaId) { Id = pagamentoId };

            _pagamentoRepositoryMock.Setup(r => r.ObterPorIdAsync(pagamentoId))
                .ReturnsAsync(pagamento);

            var command = new ConfirmarPagamentoCommand { PagamentoId = pagamentoId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(StatusPagamento.Confirmado, pagamento.StatusPagamento);
            _pagamentoRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            _mediatorMock.Verify(m => m.Publish(It.IsAny<PagamentoConfirmadoEvent>(), CancellationToken.None), Times.Once);
        }
    }
}