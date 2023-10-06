// Declare a variable to hold the MP3 file path
var mp3FilePath = 'path/to/your/audio/file.mp3';

// Initialize Howler with the first track (index 0) of the playlist
var player = new Howl({
    src: [mp3FilePath], // Use the variable here
    html5: true, // Force HTML5 audio so the audio can stream in.
    onplay: function () {
        console.log("Song is playing");
    },
    onpause: function () {
        console.log("Song is paused");
    },
    onend: function () {
        console.log("Song has ended");
    }
});

// Function to update the MP3 file path
function updateMp3FilePath(newPath) {
    mp3FilePath = newPath;
    player.unload(); // Unload the current sound
    player = new Howl({
        src: [mp3FilePath], // Use the updated variable
        html5: true,
        onplay: function () {
            console.log("Song is playing");
        },
        onpause: function () {
            console.log("Song is paused");
        },
        onend: function () {
            console.log("Song has ended");
        }
    });
}

// Play a sound
function play() {
    player.play();
}

// Pause the sound
function pause() {
    player.pause();
}

// Initialize the controls
document.addEventListener('DOMContentLoaded', function () {
    // Set up play button
    var playButton = document.querySelector('.btn-play');
    if (playButton) {
        playButton.onclick = play;
    }

    // Set up pause button
    var pauseButton = document.querySelector('.btn-pause');
    if (pauseButton) {
        pauseButton.onclick = pause;
    }
});
