import React, { Suspense, Component } from 'react';
import { Route, Switch, HashRouter as Router } from "react-router-dom";
import { Redirect } from "react-router-dom";


// import './custom.css'import 'font-awesome/css/font-awesome.min.css';
import 'primereact/resources/themes/rhea/theme.css';
import 'primereact/resources/primereact.min.css';
import 'primeicons/primeicons.css';
import 'font-awesome/css/font-awesome.css'


//Layouts
const StudentLayout = React.lazy(() => import("./layouts/studentLayout/StudentLayout"));
const AdminLayout = React.lazy(() => import("./layouts/adminLayout/AdminLayout"));


//Pages
const LoginPage = React.lazy(() => import("./views/othersViews/LoginPage"));
const Register = React.lazy(() => import("./views/othersViews/RegisterPage"));
const ConfirmEmail = React.lazy(() => import("./views/othersViews/ConfirmEmail"));


class App extends Component {

    state = {
        isLoading: false,
        isError: false
    }

    render() {
        return (
            <Router>
                <Suspense  fallback={<div>Загрузка...</div>}>
                    <Switch>
                        <Route exact path="/login" name="Login" render={props => <LoginPage {...props} />} />
                        <Route exact path="/register" name="Register" render={props => <Register {...props} />} /> 
                        <Route path="/student" name="Student" render={props => <StudentLayout {...props} />} />
                        <Route path="/admin" name="Student" render={props => <AdminLayout {...props} />} />
                        <Route exact path="/confirm/email/:id?" name="Confirm email" render={props => <ConfirmEmail {...props} />} />                      
                        <Redirect from="/" to="/login" />
                    </Switch>
                </Suspense>
            </Router>
        );
    }
};

export default App;