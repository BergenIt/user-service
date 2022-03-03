using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using UserService.Core.SenderInteraces;

namespace UserService.Core.Senders
{
    public class NotificationLoadBalancer : INotificationLoadBalancer, IDisposable, IAsyncDisposable
    {
        private readonly Timer _timer;

        private readonly IList<SenderContract> _msgs;
        private Action<SenderContract> _sendAction = null;

        public NotificationLoadBalancer(int timerDelaySec)
        {
            _msgs = new List<SenderContract>();

            _timer = new Timer(new TimerCallback(TimerEmailSend), _msgs, TimeSpan.Zero, new(0, 0, timerDelaySec));
        }

        public void AddToHandle(IEnumerable<SenderContract> senderContracts, Action<SenderContract> sendAction)
        {
            _sendAction ??= sendAction;

            foreach (SenderContract senderContract in senderContracts)
            {
                _msgs.Add(senderContract);

                Serilog.Log.Logger.ForContext<INotificationLoadBalancer>().Debug("Add message to email balanser {senderContract}", senderContract);
            }
        }

        public void AddToHandle(SenderContract senderContract, Action<SenderContract> sendAction)
        {
            _sendAction ??= sendAction;

            _msgs.Add(senderContract);

            Serilog.Log.Logger.ForContext<INotificationLoadBalancer>().Debug("Add message to email balanser {senderContract}", senderContract);
        }

        private void TimerEmailSend(object _)
        {
            if (_sendAction is not null && _msgs.Any())
            {
                _msgs.GroupBy(c => c.Subject)
                .AsParallel()
                .ForAll(group => group
                    .SelectMany(c => c.Receivers)
                    .Distinct()
                    .AsParallel()
                    .ForAll(r =>
                    {
                        IEnumerable<string> msgs = group
                            .Where(s => s.Receivers.Contains(r))
                            .SelectMany(c => c.Msgs);

                        string msg = string.Join("\n\n", msgs);

                        _sendAction(new SenderContract(msg, r, group.Key));
                    })
                );
            }

            _msgs.Clear();
            _sendAction = null;
        }

        void IDisposable.Dispose()
        {
            TimerEmailSend(_msgs);
            _sendAction = null;

            _timer.Dispose();

            GC.SuppressFinalize(this);
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            TimerEmailSend(_msgs);
            _sendAction = null;

            await _timer.DisposeAsync();

            GC.SuppressFinalize(this);
        }
    }
}
