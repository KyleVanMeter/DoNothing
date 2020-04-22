import React, { Component } from 'react'
import AudioPlayerInstance from './AudioPlayer'

class AudioSpectrumViz extends Component {
    constructor(props) {
        super(props);

        this.canvasRef = React.createRef();
    }

    componentDidMount() {
        const canvas = this.canvasRef.current;
        const ctx = canvas.getContext("2d");

        canvas.style.width = '100%';
        canvas.style.height = '100%';
        canvas.width = canvas.offsetWidth;
        canvas.height = canvas.offsetHeight

        var analyser = AudioPlayerInstance.getAnalyser();
        analyser.fftSize = 256;

        var data = new Uint8Array(analyser.frequencyBinCount);

        ctx.clearRect(0, 0, canvas.width, canvas.height);

        function draw() {
            var viz = requestAnimationFrame(draw);
            analyser.getByteFrequencyData(data);

            ctx.fillStyle = 'rgb(0,0,0)';
            ctx.fillRect(0, 0, canvas.width, canvas.height);

            var barW = (canvas.width / data.length) * 2.5;
            var barH;
            var x = 0;

            for (var i = 0; i < data.length; i++) {
                barH = data[i] / 2;

                // TODO: color should be a gradient based on barH
                ctx.fillStyle = 'rgb(' + (barH + 100) + ',50,50)';
                ctx.fillRect(x, canvas.height - barH / 2, barW, barH);

                x += barW + 1;
            }
        };

        draw();
    }

    render() {
        return (
            <canvas ref={this.canvasRef} id="AudioSpectrumViz"></canvas>
        )
    }
}

export default AudioSpectrumViz