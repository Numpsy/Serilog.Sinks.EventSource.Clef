namespace Serilog.Sinks.EventSource.Clef
{
    using System.Diagnostics.Tracing;
    using Serilog.Events;

    /// <summary>
    /// An event source that we write CLEF formatted strings to.
    /// </summary>
    [EventSource(Name = "Serilog-Sinks-EventSource-Clef")]
    public sealed class ClefEventSource : EventSource
    {
        /// <summary>
        ///  The one and only instance of the LoggingEventSource.
        /// </summary>
        internal static readonly ClefEventSource Instance = new ClefEventSource();

        // Check if the source is enabled at the specified level.
        [NonEvent]
        internal bool IsEnabled(LogEventLevel logEventLevel)
        {
            switch (logEventLevel)
            {
                case LogEventLevel.Fatal:
                    return IsEnabled(EventLevel.Critical, EventKeywords.None);
                case LogEventLevel.Error:
                    return IsEnabled(EventLevel.Error, EventKeywords.None);
                case LogEventLevel.Warning:
                    return IsEnabled(EventLevel.Warning, EventKeywords.None);
                case LogEventLevel.Information:
                    return IsEnabled(EventLevel.Informational, EventKeywords.None);
                case LogEventLevel.Debug:
                    return IsEnabled(EventLevel.Verbose, EventKeywords.None);
                case LogEventLevel.Verbose:
                    return IsEnabled(EventLevel.Verbose, EventKeywords.None);
                default:
                    return true; // @@TBD@@ On or off by default?
            }
        }

        // We only define a single event function, and specify the level as 'always'
        // could think about different events for different levels later, but for now we'll
        // just depend on the level stored in the JSON payload.
        [Event(1, Level = EventLevel.LogAlways)]
        internal void Message(string clefPayload)
        {
            WriteEvent(1, clefPayload);
        }
    }
}
