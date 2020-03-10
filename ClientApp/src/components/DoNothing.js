import React, { Component } from 'react';
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
};

export class DoNothing extends Component {
    constructor(props) {
        super(props);

        this.populateData = this.populateData.bind(this);
        this.state = {
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
                    console.log(imgPath);
                    this.setState({ someImag: imgPath });
                }

                return (
                    <Container fluid={true} key={index} className="albumItem" style={{
                        backgroundImage: 'url(' + imgPath + ')',
                        backgroundSize: 'cover',
                    }}>
                        <Row>
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
                                <Table hover striped dark borderless size="sm" onClick={clickEvent}>
                                    <tbody>
                                        {item['tracks'].map((c, i) =>
                                            <tr key={i}>
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
                                <Media style={ImgStyle2} object src={currImage} />
                            </div>
                            <div className="udDivider"> </div>
                            <div className="lowerContent">
                                <div className="vizContent">
                                    <p> test </p>
                                    {this.state.someImag}
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