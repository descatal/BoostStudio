using BoostStudio.Application.Common.Interfaces.Formats.AudioFormats;
using BoostStudio.Domain.Events;

namespace BoostStudio.Application.ApplicationLifetime.ApplicationStarted;

public class InitializeAudioFormatOnAppStartup(IFFMpegDownloader downloader)
    : INotificationHandler<ApplicationStartedEvent>
{
    public ValueTask Handle(
        ApplicationStartedEvent notification,
        CancellationToken cancellationToken
    )
    {
        _ = downloader.DownloadFFMpegSuite();

        return default;
    }
}
