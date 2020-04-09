import { Howl } from 'howler'
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

    setState(state) {
        this._state = state;
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

        var str = 'http://localhost:3001'
            + url.replace(/\\/g, '/').replace("E:/Music/Main", "");
        this._howl = new Howl({
            src: str,
            html5: true,
            format: Array.from(audioExt)
        });
        this._howl.play();
    }
}

var AudioPlayerInstance = new AudioPlayer();
export default AudioPlayerInstance;