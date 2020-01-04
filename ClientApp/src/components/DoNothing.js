import React, { Component } from 'react';

export class DoNothing extends Component {
    constructor(props) {
        super(props);
        this.state = { someNum: "" };
    }

    componentDidMount() {
        this.populateData();
    }

    render() {
        let thing = this.state.someNum;
        return (
            <div>
                <h1>Hi!</h1>
                <p>This is HTML.  Below is something we fetch.</p>
                <p>{thing}</p>
            </div>
        );
    }

    async populateData() {
        const resp = await fetch('donothing');
        const data = await resp.json();
        this.setState({someNum: data})
    }
}