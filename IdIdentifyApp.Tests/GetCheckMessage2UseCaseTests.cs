using IdIdentifyApp.Applications.Feature.Check.Ports;
using IdIdentifyApp.Applications.Feature.Check.UseCases;

namespace IdIdentifyApp.Tests
{
    public class GetCheckMessage2UseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_リポジトリがメッセージを返す_成功結果を返す()
        {
            // Arrange
            var repository = new FakeCheckRepository
            {
                Message2 = "テストメッセージ"
            };

            var useCase = new GetCheck2MessageUseCase(repository);

            // Act
            var result = await useCase.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Equal("テストメッセージ", result.Value);
        }

        [Fact]
        public async Task ExecuteAsync_リポジトリで例外発生_失敗結果を返す()
        {
            // Arrange
            var repository = new FakeCheckRepository
            {
                ExceptionToThrow = new InvalidOperationException("想定外エラー")
            };

            var useCase = new GetCheck2MessageUseCase(repository);

            // Act
            var result = await useCase.ExecuteAsync(CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.NotNull(result.Error);
        }

        private sealed class FakeCheckRepository : ICheckRepository
        {
            public string Message2 { get; set; } = string.Empty;
            public Exception? ExceptionToThrow { get; set; }

            public Task<string> GetMessage2Async(CancellationToken cancellationToken)
            {
                if (ExceptionToThrow is not null)
                {
                    throw ExceptionToThrow;
                }

                return Task.FromResult(Message2);
            }

            // 他のメソッドが ICheckRepository にある場合は、
            // 使わないものだけ NotImplementedException でよい
            public Task<string> GetMessageAsync(CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}