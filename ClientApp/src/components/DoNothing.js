import React, { Component } from 'react';
import { ReactDOM } from 'react-dom';
import { Container, Row, Col, Table, Media } from 'reactstrap';
import Placeholder from './temp.PNG';

import "bootstrap/dist/css/bootstrap.css";
import './DoNothing.css'


var ImgStyle = {
    minWidth: "64px",
};

var ImgStyle2 = {
    maxWidth: "100%",
    maxHeight: "100%",
    objectFit: "cover",
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
            someFile: [],
            someData: {
                headers: [
                    "Track #",
                    "Artist",
                    "Album",
                    "Year",
                    "Time"
                ]
            }
        };
    }

    DblClickHide = (id) => {
        const elem = document.getElementById(id)
        console.log(elem)
        const child = elem.children[1]
        if (child.style.display === 'none') {
            child.style.display = ''
        } else {
            child.style.display = 'none'
        }
    }

    DblClickEvent = (key, id) => {
        const elem = document.getElementById(id)

        if (this.state.playingInfo['info']['albumID'] === id && this.state.playingInfo['info']['rowNum'] === key) {
            this.setState({
                playingInfo: {
                    isPlaying: false,
                    info: {
                        albumID: -1,
                        rowNum: -1,
                    }
                }
            })
            if (key % 2 == 0) {
                elem.children[0].children[key].style.backgroundColor = '#292929'
            } else {
                elem.children[0].children[key].style.backgroundColor = '#1e1e1e'
            }
        } else {
            this.setState({
                playingInfo: {
                    isPlaying: true,
                    info: {
                        albumID: id,
                        rowNum: key,
                    }
                }
            })
            console.log(elem.children[0].children[key])
            elem.children[0].children[key].style.backgroundColor = '#4b4750'
        }
        console.log(this.state.playingInfo)

    }

    renderContainerHeader() {
        return (
            this.state.someFile.map((item, index) => {
                // Get either the correct file from the ImageCDN, or uses base64 encoded image if possible
                if (item['isAlbumArtEmbedded']) {
                    var imgPath = item['albumArtPath'];
                } else if (item['albumArtPath'] === 'N/A') {
                    var imgPath = Placeholder;
                } else {
                    var imgPath = "http://localhost:3000" + item['albumArtPath'].replace(/\\/g, '/').replace("E:/Music/Main", "");
                }

                const clickEvent = () => {
                    this.setState({ someImag: imgPath });
                }

                return (
                    <Container fluid={true} key={index} id={"Container" + index.toString()} className="albumItem" style={{
                        backgroundImage: 'url(' + imgPath + ')',
                        backgroundSize: 'cover',
                    }}>
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
                                <Media left-href="#">
                                    <Media style={ImgStyle} object src={imgPath} />
                                </Media>
                            </Col>
                            <Col>
                                <Table striped dark borderless size="sm" id={"Album".concat(item['id'].toString())} onClick={clickEvent}>
                                    <tbody>
                                        {item['tracks'].map((c, i) =>
                                            <tr key={i} onDoubleClick={() => this.DblClickEvent(i, "Album".concat(item['id'].toString()))}>
                                                <td className="color1" align="left">{c['disk']}.{c['trackNumber']}</td>
                                                <td className="color2" align="left">{c['trackArtist']}</td>
                                                <td className="color3" align="left">{c['trackTitle']}</td>
                                                <td className="color1" align="right">{c['duration']}</td>
                                            </tr>
                                        )}
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
        let currImage = this.state.someImag;

        return (
            <div className="wrapper">
                    <div className="seekbar">
                        <h1> This will be the seekbar </h1>
                    </div>
                <div className="content">
                    <div className="wrapContent">
                        <div className="leftContent">
                           {playList}
                        </div>
                        <div className="lrDivider"> </div>
                        <div className="rightContent">
                            <div className="upperContent">
                                <Media className="rightImage" object src={currImage} />
                            </div>
                            <div className="udDivider"> </div>
                            <div className="lowerContent">
                                <div className="vizContent">
                                    <p> test </p>
                                    {this.state.someImag}
                                    {this.state.playingInfo['info']['albumID']}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    async populateData() {
        const resp = await fetch('donothing');
        const data = await resp.json();
        this.setState({someFile: data})
    }
}