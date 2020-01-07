import React, { Component } from 'react';

export class DoNothing extends Component {


    constructor(props) {
        super(props);

        this.populateData = this.populateData.bind(this);
        this.state = {
            someNum: "", someData: {
                headers: [
                    1,
                    2,
                    3
                ],
                rows: [
                    { thing1: 'a', thing2: 'b', thing3: 'x', thing4: '1' },
                    { thing1: 'c', thing2: 'd', thing3: 'y', thing4: '2' },
                    { thing1: 'e', thing2: 'f', thing3: 'z', thing4: '3' },
                    { thing1: 'g', thing2: 'h', thing3: '?', thing4: '4' }
                ]
            }
        };
    }

    renderTable() {
        return (
            this.state.someData.rows.map((thing, index) => {
                const { thing1, thing2, thing3, thing4 } = thing

                return (
                    <tr key={index}>
                        <td>{thing1}</td>
                        <td>{thing2}</td>
                        <td>{thing3}</td>
                        <td align='right'>{thing4}</td>
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
        let thing = this.state.someNum;
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
                <p>This is HTML.  Below is something we fetch.</p>
                <p>{this.state.someNum}</p>
                <button className="btn btn-primary" onClick={this.populateData}>Re-send request</button>
            </div>
        );
    }

    async populateData() {
        //TODO: Have the query return something related to the filesystem like # of MP3, 
        //or something more complex like structured data for each filetype(.ogg, .mp3, .flacs, etc.)
        console.log("GET sent0");
        const resp = await fetch('donothing');
        const data = await resp.json();
        console.log("GET sent1");
        this.setState({someNum: data})
    }
}