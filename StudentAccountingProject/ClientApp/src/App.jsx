import React, { Suspense, Component } from 'react';
import { Route, Switch, HashRouter as Router } from "react-router-dom";

import './custom.css'
import 'font-awesome/css/font-awesome.css'



const LoginPage = React.lazy(() => import("./views/othersViews/LoginPage"));



class App extends Component {

    state = {
        isLoading: false,
        isError: false
    }

    render() {
        return (
            <Router>
                <Suspense fallback={<div>Загрузка...</div>}>
                    <Switch>
                        <Route exact path="/" name="Login" render={props => <LoginPage {...props} />} />
                    </Switch>
                </Suspense>
            </Router>
        );
    }
};

export default App;