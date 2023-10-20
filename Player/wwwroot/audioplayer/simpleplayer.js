let curr_track = document.getElementById('audio');
let curr_time = "00:00"
let total_duration = "00:00"
let seek_slider_value = 0;
let updateTimer = 0;
let isPlaying = false;

function loadTrack(file, dotNetHelper) {
    clearInterval(updateTimer);
    resetValues();
    curr_track.addEventListener("loadedmetadata", function () {
        console.log("Metadata loaded.Duration is now: ", curr_track.duration);
        console.log("File: ", curr_track.src)
    });

    updateTimer = setInterval(seekUpdate, 1000);
    curr_track.addEventListener("play", function () {
        isPlaying = true;
        dotNetHelper.invokeMethodAsync('OnAudioStart');
    });

    curr_track.addEventListener('error', function () {
        console.error('Error loading audio...', curr_track.error)
    })
    curr_track.addEventListener("stop", function () {
        isPlaying = false;
        dotNetHelper.invokeMethodAsync('OnAudioStop');
    });

    curr_track.addEventListener("ended", function () {
        isPlaying = false;

        dotNetHelper.invokeMethodAsync('OnAudioStop');
    });

    curr_track.addEventListener("canplay", function () {
        isPlaying = false;
        dotNetHelper.invokeMethodAsync('OnCanPlay', file);
    });

    curr_track.src = file;
    curr_track.preload = "metadata";
    curr_track.load();

    console.log("Current Track Duration: ", curr_track.total_duration)
}

function resetValues() {
    curr_time = "00:00";
    total_duration = "00:00";
    //seek_slider.value = 0;
}

function playpauseTrack() {
    if (!isPlaying) playTrack();
    else pauseTrack();
}

function playTrack() {
    curr_track.play();
}

function pauseTrack() {
    curr_track.pause();
}

function stopTrack() {
    curr_track.pause();
    curr_track.currentTime = 0;
}

function seekTo(sliderValue) {
    console.log("sliderValue: ", sliderValue)
    console.log("curr_track.duration:", curr_track.duration);
    let seekto = curr_track.duration * (sliderValue / 100);
    console.log("seekto:", seekto);

    if (isFinite(seekto)) {
        curr_track.currentTime = seekto;
    } else {
        console.error('Computed seekto is non-finite:', seekto);
    }
}

function setSpeed(speed) {
    curr_track.playbackRate = speed;
}

function setVolume() {
    curr_track.volume = volume_slider.value / 100;
}

function seekUpdate() {
    let seekPosition = 0;

    if (!isNaN(curr_track.duration)) {
        seekPosition = curr_track.currentTime * (100 / curr_track.duration);

       // seek_slider.value = seekPosition;

        let currentMinutes = Math.floor(curr_track.currentTime / 60);
        let currentSeconds = Math.floor(curr_track.currentTime - currentMinutes * 60);
        let durationMinutes = Math.floor(curr_track.duration / 60);
        let durationSeconds = Math.floor(curr_track.duration - durationMinutes * 60);

        if (currentSeconds < 10) { currentSeconds = "0" + currentSeconds; }
        if (durationSeconds < 10) { durationSeconds = "0" + durationSeconds; }
        if (currentMinutes < 10) { currentMinutes = "0" + currentMinutes; }
        if (durationMinutes < 10) { durationMinutes = "0" + durationMinutes; }

        curr_time.textContent = currentMinutes + ":" + currentSeconds;
        total_duration.textContent = durationMinutes + ":" + durationSeconds;
    }
}

