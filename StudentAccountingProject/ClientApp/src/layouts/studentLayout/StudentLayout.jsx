import React, { Component, Suspense } from 'react';
import { Redirect, Route, Switch,Link } from 'react-router-dom';
import * as router from 'react-router-dom';
import { Container } from 'reactstrap';
import { connect } from "react-redux";
import get from 'lodash.get';
import history from "../../utils/history";
import { logout } from '../../views/othersViews/LoginPage/reducer';
import { serverUrl } from '../../config';
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


// const AdminNavbar = React.lazy(() => import('./AdminNavbar'));

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
        e.preventDefault();
        this.props.logout();
        this.props.history.push('/login')
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
                <NavbarBrand href="/#/student/schedule" onClick={e => { history.push("/#/student/schedule");history.go()}}>Schedule</NavbarBrand>
                <NavbarToggler onClick={(e) => { this.navbarToogle() }} />
                <Collapse isOpen={this.state.isOpen} navbar>
                    <Nav className="mr-auto" navbar>
                        <NavItem>
                            <NavLink href="/#/student/mycourses" onClick={e => { history.push("/#/student/mycourses"); history.go() }}>My courses</NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink href="/#/student/courses" onClick={e => { history.push("/#/student/courses"); history.go() }}>All courses</NavLink>
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
