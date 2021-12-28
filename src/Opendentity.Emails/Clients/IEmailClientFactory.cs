namespace Opendentity.Emails.Clients;

internal interface IEmailClientFactory
{
    IEmailClient Create();
}