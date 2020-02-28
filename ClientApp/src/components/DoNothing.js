import React, { Component } from 'react';
import { Container, Row, Col, Table, Media } from 'reactstrap';
import Placeholder from './temp.PNG';
import './DoNothing.css'

import "bootstrap/dist/css/bootstrap.css";

var ImgStyle = {
    minWidth: "64px",
};

export class DoNothing extends Component {
    constructor(props) {
        super(props);

        this.populateData = this.populateData.bind(this);
        this.state = {
            someNum: "", someFile: [],
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

                return (
                    <Container fluid={true} key={index} className="albumItem">
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
                                <Table hover striped dark borderless size="sm">
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
        let test = this.renderContainerHeader();

        return (
            <div className="wrapper">
                    <div className="seekbar">
                        <h1> This will be the seekbar </h1>
                    </div>
                <div className="content">
                    <div className="wrapContent">
                        <div className="leftContent">
                           {test}
                        </div>
                        <div className="lrDivider"> </div>
                        <div className="rightContent">
                            <Media style={ImgStyle} object src={Placeholder} />
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