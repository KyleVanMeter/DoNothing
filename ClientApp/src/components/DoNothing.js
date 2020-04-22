import React, { Component, useState, useEffect } from 'react';
import { Container, Row, Col, Table, Media } from 'reactstrap';
import AudioPlayerInstance from './AudioPlayer';
import AudioSpectrumViz from './AudioSpectrumViz';
import Placeholder from './temp.PNG';
import "bootstrap/dist/css/bootstrap.css";
import './DoNothing.css'

var ImgStyle = {
    minWidth: "64px",
};

class AudioSeekbar extends Component {
    render() {
        return (
            <div className="seekbar">
                <h1> This will be the seekbar </h1>
            </div>
        )
    }
}

class LargeCoverImage extends Component {
    render() {
        return (
            <div className="imageContainer">
                <Media className="rImage" object src={this.props.Image} />
            </div>
        )
    }
}

const CurrentTime = () => {
    const [seconds, setSeconds] = useState(0);

    useEffect(() => {
        const interval = setInterval(() => {
            setSeconds(seconds => seconds = AudioPlayerInstance._howl.seek());
        }, 1000);
        return () => clearInterval(interval);
    }, []);

    const FormatTime = (sec) => {
        var seconds = parseInt(sec % 60);
        var minutes = parseInt(sec / 60 % 60);
        var hours = parseInt(sec / 60 / 60);

        var res = "";
        var resSec = seconds < 9 ? "0" + seconds : seconds.toString();

        if (hours === 0) {
            res = [minutes, resSec].join(':');
        } else {
            var resMin = minutes < 9 ? "0" + minutes : minutes.toString();
            res = [hours, resMin, resSec].join(':');
        }

        return res;
    }

    return (
        <>{FormatTime(seconds) + " / "}</>
    );
};

export class DoNothing extends Component {
    constructor(props) {
        super(props);

        this.populateData = this.populateData.bind(this);
        this.state = {
            playingInfo: {
                isPlaying: false,
                info: {
                    albumID: -1,
                    rowNum: -1
                }
            },
            someImag: Placeholder,
            someFile: []
        };
    }

    DblClickHide = (id) => {
        const elem = document.getElementById(id)

        const child = elem.children[1]
        if (child.style.display === 'none') {
            child.style.display = ''
        } else {
            child.style.display = 'none'
        }
    }

    DblClickEvent = (key, id) => {
        const elem = document.getElementById(id)
        const prevID = this.state.playingInfo['info']['albumID']
        const prevKey = this.state.playingInfo['info']['rowNum']
        var state;

        if (prevID === -1 && prevKey === -1) {
            elem.children[0].children[key].style.backgroundColor = '#4b4750'

            state = {
                playingInfo: {
                    isPlaying: true,
                    info: {
                        albumID: id,
                        rowNum: key
                    }
                }
            }

            this.state.someFile.forEach((element) => {
                if (element['id'] === parseInt(id.replace("Album", ""))) {
                    AudioPlayerInstance.play(element['tracks'][key]['path']);
                    AudioPlayerInstance.setState(state);
                }
            })

            this.setState(state)

        } else if (prevID === id && prevKey === key) {
            if (key % 2 === 0) {
                elem.children[0].children[key].style.backgroundColor = '#292929'
            } else {
                elem.children[0].children[key].style.backgroundColor = '#1e1e1e'
            }

            AudioPlayerInstance.stop();
            state = {
                playingInfo: {
                    isPlaying: false,
                    info: {
                        albumID: -1,
                        rowNum: -1,
                    }
                }
            }

            this.setState(state);
            AudioPlayerInstance.setState(state);

        } else {
            const prevElem = document.getElementById(prevID)
            if (prevKey % 2 === 0) {
                prevElem.children[0].children[prevKey].style.backgroundColor = '#292929'
            } else {
                prevElem.children[0].children[prevKey].style.backgroundColor = '#1e1e1e'
            }

            elem.children[0].children[key].style.backgroundColor = '#4b4750'

            state = {
                playingInfo: {
                    isPlaying: true,
                    info: {
                        albumID: id,
                        rowNum: key,
                    }
                }
            }

            this.state.someFile.forEach((element) => {
                if (element['id'] === parseInt(id.replace("Album", ""))) {
                    AudioPlayerInstance.play(element['tracks'][key]['path'])
                    AudioPlayerInstance.setState(state);
                }
            })

            this.setState(state);
        }
    }

    renderContainerSidebar() {
        return (
            <Col xs="1" className="rightItem">
                <Row className="rTopContent">
                    <LargeCoverImage Image={this.state.someImag} />
                </Row>
                <Row className="rLowContent">
                    {
                        this.state.playingInfo.isPlaying ?
                            <AudioSpectrumViz /> :
                            <canvas></canvas>
                    }
                </Row>
            </Col>)
    }

    renderContainerHeader() {
        return (
            this.state.someFile.map((item, index) => {
                var imgPath;
                // Get either the correct file from the ImageCDN, or uses base64 encoded image if possible
                if (item['isAlbumArtEmbedded']) {
                    imgPath = item['albumArtPath'];
                } else if (item['albumArtPath'] === 'N/A') {
                    imgPath = Placeholder;
                } else {
                    imgPath = "http://localhost:3000" + item['albumArtPath'].replace(/\\/g, '/').replace("E:/Music/Main", "");
                }

                const clickEvent = () => {
                    this.setState({ someImag: imgPath });
                }

                return (
                    <Container fluid={true} key={index} id={"Container" + index.toString()} className="albumItem">
                        <Row onDoubleClick={() => this.DblClickHide("Container" + index.toString())}>
                            <Col>
                                <div className="header">
                                    <div className="header-text">
                                        <h1 className="test">{item['artists']} - {item['albumName']} ({item['year']}) </h1>
                                    </div>
                                    <div className="divider"></div>
                                </div>
                            </Col>
                        </Row>
                        <Row>
                            <Col xs="2">
                                <Media>
                                    <Media style={ImgStyle} object src={imgPath} />
                                </Media>
                            </Col>
                            <Col>
                                <Table striped dark borderless size="sm" id={"Album".concat(item['id'].toString())} onClick={clickEvent}>
                                    <tbody>
                                        {item['tracks'].map((c, i) => {
                                            const trackNum = (c['trackNumber'] < 10 ? "0" + c['trackNumber'].toString() : c['trackNumber']);
                                            return (
                                                <tr key={i} onDoubleClick={() => this.DblClickEvent(i, "Album".concat(item['id'].toString()))}>
                                                    <td style={{ width: "1%" }} align="left">
                                                        {
                                                            (this.state.playingInfo.isPlaying &&
                                                                this.state.playingInfo.info.rowNum === i &&
                                                                this.state.playingInfo.info.albumID === "Album".concat(item['id'].toString())) ?
                                                                "►" : null
                                                        }
                                                    </td>
                                                    <td className="color1" align="left">{c['disk']}.{trackNum}</td>
                                                    <td className="color2" align="left">{c['trackArtist']}</td>
                                                    <td className="color3" align="left">{c['trackTitle']}</td>
                                                    <td className="color1" align="right">
                                                        {
                                                            (this.state.playingInfo.isPlaying &&
                                                                this.state.playingInfo.info.rowNum === i &&
                                                                this.state.playingInfo.info.albumID === "Album".concat(item['id'].toString())) ?
                                                                <CurrentTime /> : null
                                                        }
                                                        {c['duration']}</td>
                                                </tr>)
                                        })}
                                    </tbody>
                                </Table>
                            </Col>
                        </Row>
                    </Container>)
            }))
    }

    componentDidMount() {
        this.populateData();
    }

    render() {
        let playList = this.renderContainerHeader();
        let rightImg = this.renderContainerSidebar();


        return (
            <div className="wrapper">
                <AudioSeekbar />
                <div className="content">
                    <div className="wrapContent">
                        <Container fluid={true} style={{ width: "100vw" }}>
                            <Row>
                                <Col xs="1" className="leftItem">
                                    {playList}
                                </Col>
                                <div className="sticky-top">
                                    {rightImg}
                                </div>
                            </Row>
                        </Container>
                    </div>
                </div>
            </div>
        );
    }

    async populateData() {
        const resp = await fetch('donothing');
        const data = await resp.json();
        this.setState({ someFile: data })
    }
}