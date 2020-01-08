import React, { Component } from 'react';

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
        let tableBody = this.renderTable();
        let tableHead = this.renderHead();
        return (
            <div>
                <h1>Hi!</h1>
                <div>
                    <table className="table table-hover table-sm table-borderless table-dark table-striped">
                        <thead className="thead-light">
                            <tr>
                                {tableHead}
                            </tr>
                        </thead>
                        <tbody>
                            {tableBody}
                        </tbody>
                    </table>
                </div>
                
                <button className="btn btn-primary" onClick={this.populateData}>Re-send request</button>
            </div>
        );
    }

    async populateData() {
        const resp = await fetch('donothing');
        const data = await resp.json();
        this.setState({someFile: data})
    }
}