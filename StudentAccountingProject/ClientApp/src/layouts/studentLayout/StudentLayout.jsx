import React, { Component, Suspense } from 'react';
import { Redirect, Route, Switch,Link } from 'react-router-dom';
import * as router from 'react-router-dom';
import { Container } from 'reactstrap';
import { connect } from "react-redux";
import get from 'lodash.get';
import history from "../../utils/history";
import { logout } from '../../views/othersViews/LoginPage/reducer';
import { serverUrl } from '../../config';
import { Button } from 'reactstrap';

import {
    Collapse,
    Navbar,
    NavbarToggler,
    NavbarBrand,
    Nav,
    NavItem,
    NavLink
} from 'reactstrap';

import routes from '../../routes/studentRoutes';


class StudentLayout extends Component {

    state = {
        isOpen:false,
    };

    navbarToogle = () => {
        this.setState({
            isOpen:!this.state.isOpen
        });
    }

    loading = () => <div className="animated fadeIn pt-1 text-center">Loading...</div>

    signOut(e) {
        this.props.logout();
    }

    render() {
        const { login } = this.props;
        console.log(login);
        let isAccess = false;
        console.log("Auth", login.isAuthenticated);
        if (!login.isAuthenticated) {
            return (
                <Redirect to="/login" />
            );
        }
        if (login.isAuthenticated) {
            const { roles } = login.user;
            for (let i = 0; i < roles.length; i++) {
                if (roles[i] === 'Student')
                    isAccess = true;
            }
        }

        const content = (<div className="app">
            <Navbar color="dark" dark expand="md">
                <NavbarBrand href="/#/student/schedule">Schedule</NavbarBrand>
                <NavbarToggler onClick={(e) => { this.navbarToogle() }} />
                <Collapse isOpen={this.state.isOpen} navbar>
                    <Nav className="mr-auto" navbar>
                        <NavItem>
                            <NavLink href="/#/student/mycourses">My courses</NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink href="/#/student/courses">All courses</NavLink>
                        </NavItem>                                 
                    </Nav>         
                    <Nav navbar>
                        <NavItem>
                            <NavLink href="/#/student/profile">Profile</NavLink>
                        </NavItem>       
                        <NavItem className="ml-2">
                            <Button color="warning" onClick={e => { this.signOut(); }}>Logout</Button>
                        </NavItem>           
                    </Nav>  
                </Collapse>
            </Navbar>
            <div className="app-body">
                <main className="main">
                    <Container fluid>
                        <Suspense fallback={this.loading()}>
                            <Switch>
                                {routes.map((route, idx) => {
                                    return route.component ? (
                                        <Route
                                            key={idx}
                                            path={route.path}
                                            exact={route.exact}
                                            name={route.name}
                                            render={props => (
                                                <route.component {...props} />
                                            )} />
                                    ) : (null);
                                })}
                            </Switch>
                        </Suspense>
                    </Container>
                </main>
            </div>
        </div>);
        return (
            isAccess ?
                content :
                <Redirect to="/login" />
        );
    }
}

const mapStateToProps = (state) => {
    return {
        login: get(state, 'login')
    };
}
export default connect(mapStateToProps, { logout })(StudentLayout);
