var currentPlayer = null;
var updateInterval = null;
var currentTime = 0;
var duration = 0;
function SmlPlayAudioFile(file, dotnetHelper) {
    // Stop the currently playing audio and clear interval if there is one
    if (currentPlayer) {
        currentPlayer.stop();
        currentPlayer.unload();
        if (updateInterval) {
            clearInterval(updateInterval);
        }
    }

    currentPlayer = new Howl({
        src: [file],
        html5: true,
        onplay: function () {
            // Update the current time and duration every second
            dotnetHelper.invokeMethodAsync('OnAudioStart');
            updateInterval = setInterval(function () {
                currentTime = currentPlayer.seek();
                duration = currentPlayer.duration();
                dotnetHelper.invokeMethodAsync('UpdateCurrentTimeAndDuration', currentTime, duration);
            }, 10);
        },
        onstop: function () {
            dotnetHelper.invokeMethodAsync('OnAudioStop');
            updateInterval = null;
        }
    });

    currentPlayer.play();
}
function SmlSeek(seconds) {
    if (currentPlayer) {
        currentPlayer.seek(seconds)
    }
}
function SmlStop()
{
    if (!currentPlayer) {
        return;
    }
    currentPlayer.stop();
    if (updateInterval) {
        clearInterval(updateInterval);
    }
}

function SmlPlay() {
    if (currentPlayer) {
        currentPlayer.play();
    }
    
}

function SmlSetPlaybackSpeed(multiplier) {
    if (currentPlayer) {
        currentPlayer.rate(multiplier)
    }
}
