import React, { Component } from 'react';
import {
    Button, Card, CardBody, CardGroup,
    Col, Container, Form, Input, InputGroup,
    InputGroupAddon, Row
} from 'reactstrap';
import classnames from 'classnames';
import { Redirect } from 'react-router-dom'
import { Dimmer, Loader } from 'semantic-ui-react'
import 'semantic-ui-css/semantic.min.css'

import get from "lodash.get";
import { connect } from "react-redux";
import * as reducer from './reducer';



class RegisterPage extends Component {

    state = {
        email: '',
        password: '',
        name: '',
        surname: '',
        age:'',
        errorsState: {},
        errorServer: {},
        done: false,
        isLoading: false,
        visible: false
    }

    onSubmitForm = (e) => {
        e.preventDefault();
        const { email, password, name, surname,age } = this.state;

        let errorsState = {};

        if (!email) errorsState.email = "Enter correct email";

        if (password.length < 8) errorsState.password = "The password must contain 8 characters, at least one capital letter";

        if (name.length < 3) errorsState.name = "Enter correct name";

        if (surname < 3) errorsState.surname = "Enter correct surname";

        if(age === '' || age < 1) errorsState.age = "Enter correct age";

        const isValid = Object.keys(errorsState).length === 0
        if (isValid) {
            this.setState({ isLoading: true });
            const model = {
                registerDTO:{
                    email: email,
                    password: password,
                    name: name,
                    surname: surname,
                    age:age
                }          
            };

            this.props.Register(model);
        }
        else {
            this.setState({ errorsState });
        }
    }

    static getDerivedStateFromProps(nextProps, prevState) {
        console.log("Next props", nextProps);

        return { 
            errorServer: nextProps.registerReducer.post.error,
            isLoading: nextProps.registerReducer.post.loading
        };
    }

    setStateByErrors = (name, value) => {
        if (!!this.state.errorsState[name]) {
            let errorsState = Object.assign({}, this.state.errorsState);
            delete errorsState[name];
            this.setState(
                {
                    [name]: value,
                    errorsState
                }
            )
        }
        else {
            this.setState(
                { [name]: value })
        }
    }

    handleChange = (e) => {
        this.setStateByErrors(e.target.name, e.target.value);
    }

    passwordVisible = (e) => {
        this.setState({
            visible: !this.state.visible,
        });
    }

    render() {
        const { errorsState, isLoading, profileUrl, visible, errorServer } = this.state;
        const form = (
            <div className="app flex-row">
                <Container>
                    {isLoading && <Dimmer active>
                        <Loader>Loading</Loader>
                    </Dimmer>}
                    <Row className="justify-content-center mt-5">
                        <Col md="8">
                            <CardGroup>
                                <Card className="p-4">
                                    <CardBody>
                                        <Form onSubmit={this.onSubmitForm}>
                                            {!!errorServer ?
                                                <div className="alert alert-danger">
                                                    {errorServer}.
                                            </div> : ""}
                                            <h1>Sign up</h1>
                                            <p className="text-muted">Create your account</p>
                                            {!!errorsState.email ? <div style={{ color: "red" }}>{errorsState.email}</div> : ""}
                                            <InputGroup className="mb-3">
                                                <Input
                                                    type="email"
                                                    placeholder="Email"
                                                    id="email"
                                                    autoComplete="email"
                                                    name="email"
                                                    value={this.state.email}
                                                    onChange={this.handleChange}
                                                />
                                            </InputGroup>
                                            {!!errorsState.name ? <div style={{ color: "red" }}>{errorsState.name}</div> : ""}
                                            <InputGroup className="mb-3">
                                                <Input
                                                    type="text"
                                                    placeholder="Name"
                                                    id="name"
                                                    autoComplete="name"
                                                    name="name"
                                                    value={this.state.name}
                                                    onChange={this.handleChange}
                                                />
                                            </InputGroup>
                                            {!!errorsState.surname ? <div style={{ color: "red" }}>{errorsState.surname}</div> : ""}
                                            <InputGroup className="mb-3">
                                                <Input
                                                    type="text"
                                                    placeholder="Surname"
                                                    id="surname"
                                                    autoComplete="surname"
                                                    name="surname"
                                                    value={this.state.surname}
                                                    onChange={this.handleChange}
                                                />
                                            </InputGroup>
                                            {!!errorsState.surname ? <div style={{ color: "red" }}>{errorsState.age}</div> : ""}
                                            <InputGroup className="mb-3">
                                                <Input 
                                                    type="number"
                                                    placeholder="Age"
                                                    id="age"
                                                    name="age"
                                                    min={1}
                                                    value={this.state.age}
                                                    onChange={this.handleChange}
                                                />
                                            </InputGroup>
                                            {!!errorsState.password ? <div style={{ color: "red" }}>{errorsState.password}</div> : ""}
                                            <InputGroup className="mb-4">
                                                <Input
                                                    type={classnames(visible ? "text" : "password")}
                                                    id="password"
                                                    name="password"
                                                    placeholder="Password"
                                                    autoComplete="current-password"
                                                    onChange={this.handleChange}
                                                />
                                                <InputGroupAddon addonType="append">
                                                    <Button onClick={this.passwordVisible}>
                                                        <i className={classnames(visible ? 'fa fa-eye' : 'fa fa-eye-slash')}></i>
                                                    </Button>
                                                </InputGroupAddon>
                                            </InputGroup>
                                            <Row>
                                                <Col xs="6">
                                                    <Button type="submit" color="primary" className="px-4">Create</Button>
                                                </Col>
                                            </Row>
                                            {!!errorServer ? <div style={{ color: "red" }}>{errorServer.errorMessage}</div> : ""}
                                        </Form>
                                    </CardBody>
                                </Card>
                            </CardGroup>
                        </Col>
                    </Row>
                </Container>
            </div>
        );
        return (form);
    }
}

const mapStateToProps = state => {
    return {
        registerReducer: get(state, 'register')
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        Register: (model) => {
            dispatch(reducer.Register(model));
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(RegisterPage);