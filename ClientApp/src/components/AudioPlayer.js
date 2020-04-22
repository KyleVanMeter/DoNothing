import { Howl, Howler } from 'howler'
import { audioExt } from '../util/Extensions'

const _initstate = {
    playingInfo: {
        isPlaying: false,
        info: {
            albumID: -1,
            rowNum: -1
        }
    },
    isPaused: false,
    isContinue: true,
    sourceURL: ""
}

class AudioPlayer {
    constructor() {
        if (!AudioPlayer.instance) {
            this._state = _initstate;
            this._howl = null;
            AudioPlayer.instance = this;
        }
    }

    getState() {
        return this._state;
    }

    getAnalyser() {
        var analyser = Howler.ctx.createAnalyser();
        Howler.masterGain.connect(analyser);
        analyser.connect(Howler.ctx.destination);

        return analyser;
    }

    setState(state) {
        // TODO: Look into JSON validation
        this._state.playingInfo.isPlaying = state.playingInfo.isPlaying;
        this._state.playingInfo.info.albumID = state.playingInfo.info.albumID;
        this._state.playingInfo.info.rowNum = state.playingInfo.info.rowNum;
    }

    nextTrack() {
        if (this._state.isContinue) {
            const nextElem = document.getElementById(this._state.playingInfo.info.albumID);
            const elemArr = Array.from(nextElem.rows);

            // TODO: Refactor to not use a loop
            // loop should not be need, and is a remnant of previous code
            var broke = false;
            let row;
            for (row of elemArr) {
                if (row.rowIndex > this._state.playingInfo.info.rowNum) {
                    row.dispatchEvent(new Event(
                        'dblclick',
                        { 'bubbles': true }
                    ));

                    broke = true;
                    break;
                }
            }

            if (!broke) {
                // Default case at end of album.  Send dblclick to current playing track
                // sets to 'not playing' state
                row.dispatchEvent(new Event(
                    'dblclick',
                    { 'bubbles': true }
                ));
            }
        }
    }

    stop() {
        if (this._howl !== null) {
            this._howl.unload();
        }
    }

    play(url) {
        if (this._howl !== null) {
            this._howl.unload();
        }

        // Get audio stream from ChunkAudioCDN
        // at the moment this needs to use WebAudioAPI for visualizations
        // but that also means that the WHOLE file needs to be downloaded
        // & in memory to play.  The only fix at this point is to write
        // a decoder that can handle chunks for EACH audio format (not just ogg / wav)
        this._state.sourceURL = 'https://localhost:3001'
            + url.replace(/\\/g, '/').replace("E:/Music/Main", "");
        this._howl = new Howl({
            src: this._state.sourceURL,
            html5: false,
            format: Array.from(audioExt)
        });

        this._howl.on('end', () => { this.nextTrack() });

        this._howl.play();
    }
}

var AudioPlayerInstance = new AudioPlayer();
export default AudioPlayerInstance;