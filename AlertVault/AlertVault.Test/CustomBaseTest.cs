using AlertVault.Core.Test;

namespace AlertVault.Test;

public abstract class CustomBaseTest(Fixture fixture) : BaseTest(fixture), IClassFixture<CustomWebApplicationFactory>
{
}