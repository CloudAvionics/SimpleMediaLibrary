window.myAudioFunctions = {
    reloadAndPlayAudio: function (audioElementId) {
        const audioElement = document.getElementById(audioElementId);
        if (audioElement) {
            audioElement.load();  // Reloads the audio element
            audioElement.play();  // Starts playback
        }
    }
};
