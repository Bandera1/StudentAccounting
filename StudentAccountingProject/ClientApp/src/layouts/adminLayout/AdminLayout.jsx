import React, { Component, Suspense } from 'react';
import { Redirect, Route, Switch, Link } from 'react-router-dom';
import * as router from 'react-router-dom';
import { Container } from 'reactstrap';
import { connect } from "react-redux";
import get from 'lodash.get';
import { logout } from '../../views/othersViews/LoginPage/reducer';
import history from "../../utils/history";
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

import routes from '../../routes/adminRoutes';


class AdminLayout extends Component {
    state = {
        isOpen: false,
    };

    navbarToogle = () => {
        this.setState({
            isOpen: !this.state.isOpen
        });
    }

    loading = () => <div className="animated fadeIn pt-1 text-center">Loading...</div>

    signOut(e) {
        this.props.logout();
        // this.props.history.push('/login')
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
                if (roles[i] === 'Admin')
                    isAccess = true;
            }          
        }

        const content = (<div className="app">
            <Navbar color="dark" dark expand="md">
                <NavbarToggler onClick={(e) => { this.navbarToogle() }} />
                <Collapse isOpen={this.state.isOpen} navbar>
                    <Nav className="mr-auto" navbar>
                        <NavItem>
                            <NavbarBrand href="/#/admin/students" >Students list</NavbarBrand>
                        </NavItem>
                        <NavItem>
                            <NavbarBrand href="/#/admin/courses" >Courses list</NavbarBrand>                        
                        </NavItem>
                    </Nav>
                    <Nav navbar>
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
            isAccess == true ?
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
export default connect(mapStateToProps, { logout })(AdminLayout);

