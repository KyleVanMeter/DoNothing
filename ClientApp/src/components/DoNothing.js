import React, { Component } from 'react';
import { Container, Row, Col, Table, Media } from 'reactstrap';
import Placeholder from './temp.PNG';
import ForeignTest from 'E:/Music/Main/Bleep/4 Hero - Combat Dancin\' (1990)/cover.jpg'
import './DoNothing.css'

import "bootstrap/dist/css/bootstrap.css";

const Box = props => <div className="box">{props.children} </div>;
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
                return (
                    <Container fluid={true} key={index}>
                        <Row>
                            <Col>
                                <Box> <h1>{item['artists']} - {item['albumName']} ({item['year']}) </h1> </Box>
                            </Col>
                        </Row>
                        <Row>
                            <Col xs="2">
                                <Media left-href="#">
                                    <Media style={ImgStyle} object src={ForeignTest} />
                                </Media>
                            </Col>
                            <Col>
                                <Table hover striped dark borderless size="sm">
                                    <tbody>
                                        {item['tracks'].map((c, i) =>
                                            <tr key={i}>
                                                <td>{c['disk']}.{c['trackNumber']}</td>
                                                <td>{c['trackArtist']}</td>
                                                <td>{c['trackTitle']}</td>
                                                <td align="right">{c['duration']}</td>
                                            </tr>
                                        )}
                                    </tbody>
                                </Table>
                            </Col>
                        </Row>
                    </Container>)
            }))
    }

    renderTable() {
        return (
            this.state.someFile.map((item, index) => {
                return (
                    <tr key={index}>
                        <td>{item['track']}</td>
                        <td>{item['artist'].join(", ")}</td>
                        <td>{item['album']}</td>
                        <td>{item['year']}</td>
                        <td align='right'>{item['time']}</td>
                    </tr>
                )
            })
        )
    }

    renderHead() {
        return (
            this.state.someData.headers.map((col, index) => {
                if (index === this.state.someData.headers.length-1) {
                    return <th key={index} colSpan="2">{col}</th>
                } else {
                    return <th key={index}>{col}</th>
                }
            })
        )
    }

    componentDidMount() {
        this.populateData();
    }

    render() {
        let test = this.renderContainerHeader();

        return (
            <div>
                <Container>
                    {test}
                </Container>            
            </div>
        );
    }

    async populateData() {
        const resp = await fetch('donothing');
        const data = await resp.json();
        this.setState({someFile: data})
    }
}