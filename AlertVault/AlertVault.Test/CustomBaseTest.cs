using AlertVault.Core.Test;

namespace AlertVault.Test;

public abstract class CustomBaseTest(CustomWebApplicationFactory factory, Fixture fixture) : BaseTest(fixture), IClassFixture<CustomWebApplicationFactory>
{
}