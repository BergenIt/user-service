using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MailKit;
using MailKit.Net.Proxy;
using MailKit.Security;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MimeKit;

namespace UserService.Core_Tests.Moqups
{
    public class MoqupsMailTransport : IMailTransport
    {

        #region UnusePart
        private bool _disposedValue;

        public object SyncRoot { get; }
        public SslProtocols SslProtocols { get; set; }
        public X509CertificateCollection ClientCertificates { get; set; }
        public bool CheckCertificateRevocation { get; set; }
        public RemoteCertificateValidationCallback ServerCertificateValidationCallback { get; set; }
        public IPEndPoint LocalEndPoint { get; set; }
        public IProxyClient ProxyClient { get; set; }
        public HashSet<string> AuthenticationMechanisms { get; }
        public bool IsAuthenticated { get; }
        public bool IsConnected { get; }
        public bool IsSecure { get; }
        public bool IsEncrypted { get; }
        public bool IsSigned { get; }
        public int Timeout { get; set; }

        event EventHandler<MessageSentEventArgs> IMailTransport.MessageSent { add { } remove { } }

        event EventHandler<ConnectedEventArgs> IMailService.Connected { add { } remove { } }

        event EventHandler<DisconnectedEventArgs> IMailService.Disconnected { add { } remove { } }

        event EventHandler<AuthenticatedEventArgs> IMailService.Authenticated { add { } remove { } }

        public void Authenticate(ICredentials credentials, CancellationToken cancellationToken = default) { }

        public void Authenticate(Encoding encoding, ICredentials credentials, CancellationToken cancellationToken = default) { }

        public void Authenticate(Encoding encoding, string userName, string password, CancellationToken cancellationToken = default) { }

        public void Authenticate(string userName, string password, CancellationToken cancellationToken = default) { }

        public void Authenticate(SaslMechanism mechanism, CancellationToken cancellationToken = default) { }

        public Task AuthenticateAsync(ICredentials credentials, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task AuthenticateAsync(Encoding encoding, ICredentials credentials, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task AuthenticateAsync(Encoding encoding, string userName, string password, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task AuthenticateAsync(string userName, string password, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task AuthenticateAsync(SaslMechanism mechanism, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Connect(string host, int port, bool useSsl, CancellationToken cancellationToken = default) { }

        public void Connect(string host, int port = 0, SecureSocketOptions options = SecureSocketOptions.Auto, CancellationToken cancellationToken = default) { }

        public void Connect(Socket socket, string host, int port = 0, SecureSocketOptions options = SecureSocketOptions.Auto, CancellationToken cancellationToken = default) { }

        public void Connect(Stream stream, string host, int port = 0, SecureSocketOptions options = SecureSocketOptions.Auto, CancellationToken cancellationToken = default) { }

        public Task ConnectAsync(string host, int port, bool useSsl, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task ConnectAsync(string host, int port = 0, SecureSocketOptions options = SecureSocketOptions.Auto, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task ConnectAsync(Socket socket, string host, int port = 0, SecureSocketOptions options = SecureSocketOptions.Auto, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task ConnectAsync(Stream stream, string host, int port = 0, SecureSocketOptions options = SecureSocketOptions.Auto, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Disconnect(bool quit, CancellationToken cancellationToken = default) { }

        public Task DisconnectAsync(bool quit, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void NoOp(CancellationToken cancellationToken = default) { }

        public Task NoOpAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        #endregion

        private void SendValidate(MimeMessage mimeMessage)
        {
            Assert.IsTrue(mimeMessage.To.Any());
            Assert.IsTrue(!string.IsNullOrWhiteSpace(mimeMessage.TextBody));
        }

        public void Send(MimeMessage message, CancellationToken cancellationToken = default, ITransferProgress progress = null)
        {
            SendValidate(message);
        }

        public void Send(MimeMessage message, MailboxAddress sender, IEnumerable<MailboxAddress> recipients, CancellationToken cancellationToken = default, ITransferProgress progress = null)
        {
            SendValidate(message);
        }

        public void Send(FormatOptions options, MimeMessage message, CancellationToken cancellationToken = default, ITransferProgress progress = null)
        {
            SendValidate(message);
        }

        public void Send(FormatOptions options, MimeMessage message, MailboxAddress sender, IEnumerable<MailboxAddress> recipients, CancellationToken cancellationToken = default, ITransferProgress progress = null)
        {
            SendValidate(message);
        }

        public Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default, ITransferProgress progress = null)
        {
            SendValidate(message);
            return Task.CompletedTask;
        }

        public Task SendAsync(MimeMessage message, MailboxAddress sender, IEnumerable<MailboxAddress> recipients, CancellationToken cancellationToken = default, ITransferProgress progress = null)
        {
            SendValidate(message);
            return Task.CompletedTask;
        }

        public Task SendAsync(FormatOptions options, MimeMessage message, CancellationToken cancellationToken = default, ITransferProgress progress = null)
        {
            SendValidate(message);
            return Task.CompletedTask;
        }

        public Task SendAsync(FormatOptions options, MimeMessage message, MailboxAddress sender, IEnumerable<MailboxAddress> recipients, CancellationToken cancellationToken = default, ITransferProgress progress = null)
        {
            SendValidate(message);
            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
