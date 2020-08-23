namespace Serilog.Sinks.EventSource.Clef
{
    using System;
    using System.IO;
    using Serilog.Configuration;
    using Serilog.Core;
    using Serilog.Events;
    using Serilog.Formatting;
    using Serilog.Formatting.Compact;

    /// <summary>
    /// An implementation of ILogEventSink that formats events using CompactJsonFormatter and writes them to an EventSource/
    /// </summary>
    public sealed class EventSourceClefSink : ILogEventSink
    {
        private readonly ITextFormatter textFormatter = new CompactJsonFormatter();

        /// <inheritdoc/>
        void ILogEventSink.Emit(LogEvent logEvent)
        {
            if (ClefEventSource.Instance.IsEnabled(logEvent.Level))
            {
                // @@TBD@@ Make this thread/async local so we don't need piles of them?
                string message = null;
                using (var writer = new StringWriter())
                {
                    textFormatter.Format(logEvent, writer);
                    message = writer.ToString();
                }

                ClefEventSource.Instance.Message(message);
            }
        }
    }

    /// <summary>
    /// Extensions for setting up the sink.
    /// </summary>
    public static class ClefLoggerConfigurationExtensions
    {
        /// <summary>
        /// Adds the event source sink to the specified LoggerSinkConfiguration.
        /// </summary>
        /// <param name="sinkConfiguration">The sink configuration being created.</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for events passed through the sink.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration ClefSource(
            this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));

            var sink = new EventSourceClefSink();
            return sinkConfiguration.Sink(sink, restrictedToMinimumLevel, null);
        }
    }
}
