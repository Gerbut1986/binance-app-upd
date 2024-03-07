using Arbitrage.Api.Enums;

namespace Arbitrage.Api.Dto
{
  public class ClientResponseDto<T> : IClientResponseDto
  {
    public ResponseStatus Status { get; set; }

    public SubscriptionDto Subscription { get; set; }

    public T Result { get; set; }

    public ClientResponseDto() => this.Status = ResponseStatus.Ok;
  }
}
