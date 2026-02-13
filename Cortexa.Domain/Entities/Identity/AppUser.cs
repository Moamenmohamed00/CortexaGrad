namespace Cortexa.Domain.Entities.Identity
{
    public class AppUser
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string UserName { get; private set; }
        public string Email { get; private set; }

    }
}
