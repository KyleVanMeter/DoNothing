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
        this._state.playingInfo.isPlaying = state.playingInfo.isPlaying;
        this._state.playingInfo.info.albumID = state.playingInfo.info.albumID;
        this._state.playingInfo.info.rowNum = state.playingInfo.info.rowNum;
    }

    nextTrack() {
        if (this._state.isContinue) {
            const nextElem = document.getElementById(this._state.playingInfo.info.albumID);
            const elemArr = Array.from(nextElem.rows);

            let row;
            for (row of elemArr) {
                // Each row in the playlist has a column that is of the form 'disk#'.'track#'
                // trackNumber converts that into an integer (ignoring disk number)
                var trackNumber = row.cells[1].innerHTML.split('.').map(c => parseInt(c))[1];

                if (trackNumber > this._state.playingInfo.info.rowNum + 1) {
                    row.dispatchEvent(new Event(
                        'dblclick',
                        { 'bubbles': true }
                    ));

                    break;
                }
            }
            row.dispatchEvent(new Event(
                'dblclick',
                { 'bubbles': true }
            ));
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