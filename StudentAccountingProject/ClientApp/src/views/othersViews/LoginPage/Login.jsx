import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import {
    Button, Card, CardBody, CardGroup,
    Col, Container, Form, Input, InputGroup,
    InputGroupAddon, InputGroupText, Row
} from 'reactstrap';
import classnames from 'classnames';
import get from "lodash.get";
import { connect } from "react-redux";
import * as loginActions from './reducer';
import { Dimmer, Loader } from 'semantic-ui-react'
import FacebookAuth from '../../../components/FacebookAuth'
import 'semantic-ui-css/semantic.min.css'

class Login extends Component {
    state = {
        email: '',
        password: '',
        profileUrl: '',
        errors: {},
        done: false,
        isLoading: false,
        visible: false,
        errorServer: {}
    }

    componentWillMount = () => {
        this.props.logout();
    };

    passwordVisible = (e) => {
        this.setState({
            visible: !this.state.visible,
        });
    }
    
    static getDerivedStateFromProps(nextProps, prevState) {
        return { isLoading: nextProps.loginReducer.post.loading, errorServer: nextProps.loginReducer.post.error };
    }

    setStateByErrors = (name, value) => {
        if (!!this.state.errors[name]) {
            let errors = Object.assign({}, this.state.errors);
            delete errors[name];
            this.setState(
                {
                    [name]: value,
                    errors
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
    onSubmitForm = (e) => {
        e.preventDefault();
        const { email, password } = this.state;
        let errors = {};

        if (email === '') errors.email = "Enter email";


        if (password === '') errors.password = "Enter password";

        const isValid = Object.keys(errors).length === 0
        if (isValid) {
            this.setState({ isLoading: true });
            const model = {
                loginDTO: {
                    email: email,
                    password: password
                }        
            };
            console.log("Login model - ",model);
            this.props.login(model, this.props.history);
        }
        else {
            this.setState({ errors });
        }
    }

    render() {
        const { errors, isLoading, visible, errorServer } = this.state;
        const form = (

            <div className="app flex-row">
                {isLoading && <Dimmer  active>
                    <Loader style={{ maxHeight: "100vh" }}>Loading</Loader>
                </Dimmer>}
                
                <Container>
                    <Row className="justify-content-center mt-5">
                        <Col md="6">
                            <CardGroup>
                                <Card className="p-3">
                                    <CardBody>
                                        <Form onSubmit={this.onSubmitForm}>
                                            {!!errorServer ?
                                                <div className="alert alert-danger">
                                                    {errorServer}.
                                            </div> : ""}
                                            <h1>Sign in</h1>
                                            <p className="text-muted">Sign in your account</p>


                                            <InputGroup className="mb-2">
                                                <span className="input-group-text" id="basic-addon1">@</span>
                                                <Input
                                                    type="text"
                                                    placeholder="Електронна пошта"
                                                    className={classnames("form-control", { "is-invalid": !!errors.email })}
                                                    id="email"
                                                    autoComplete="new-password"
                                                    name="email"
                                                    value={this.state.email}
                                                    onChange={this.handleChange}
                                                />
                                                {!!errors.email ? <div className="invalid-feedback">{errors.email}</div> : ""}
                                            </InputGroup>

                                            <InputGroup className="mb-2">

                                                <InputGroupAddon addonType="prepend">
                                                    <InputGroupText>
                                                        <i className="fa fa-key"/>
                                                    </InputGroupText>
                                                </InputGroupAddon>
                                                <Input className={classnames('form-control', { 'is-invalid': !!errors.password })}
                                                    type={classnames(visible ? "text" : "password")}
                                                    id="password"
                                                    name="password"
                                                    placeholder="Пароль"
                                                    autoComplete="current-password"
                                                    onChange={this.handleChange} />
                                                <InputGroupAddon addonType="append">
                                                    <Button onClick={this.passwordVisible}>
                                                        <i className={classnames(visible ? 'fa fa-eye' : 'fa fa-eye-slash')}></i>
                                                    </Button>
                                                </InputGroupAddon>
                                                {!!errors.password ?
                                                    <div className="invalid-feedback">
                                                        {errors.password}
                                                    </div> : ''}
                                            </InputGroup>
                                            <div className="d-flex justify-content-center">
                                                <div className="p-2 bd-highlight">
                                                    <Button color="primary" className="px-3">Enter</Button>
                                                </div>

                                                <div className="p-2 bd-highlight">
                                                    <Link to="/register">
                                                        <Button color="primary" className="px-3">Sign up</Button>
                                                    </Link>
                                                </div>
                                              
                                            </div>
                                            <div className="d-flex justify-content-center">
                                                <div className="p-2 bd-highlight">
                                                    <FacebookAuth login={this.props.facebookLogin} history={this.props.history}/>
                                                </div>                                             
                                            </div>
                                            {/* <Col xs="5" style={{maxWidth:"50%"}}>
                                            </Col> */}
                                        </Form>
                                    </CardBody>
                                </Card>
                            </CardGroup>
                        </Col>
                    </Row>
                </Container>

            </div>
        );
        return (
            form
        );
    }
}

// GetReducerData
function mapStateToProps(state) {
    console.log('mapStateToProps', state)
    return {
        loginReducer: get(state, 'login'),
    };
}

const mapDispatch = {
    login: (model, history) => {
        return loginActions.login(model, history);
    },
    facebookLogin: (model) => {
        return loginActions.facebookLogin(model);
    },
    logout: () => {
        return loginActions.logout();
    }
}

export default connect(mapStateToProps, mapDispatch)(Login);
