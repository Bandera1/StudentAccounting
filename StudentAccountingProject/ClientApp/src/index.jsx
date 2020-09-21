import 'bootstrap/dist/css/bootstrap.css';
// import './custom.css'

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { ConnectedRouter } from 'connected-react-router';
import { createBrowserHistory } from 'history';
import configureStore, {history} from './store/configureStore';
import App from './App';
import registerServiceWorker, { unregister } from './registerServiceWorker';
import * as loginActions from './views/othersViews/LoginPage/reducer';
import jwt from 'jsonwebtoken'

import 'primereact/resources/primereact.min.css';
import 'primeicons/primeicons.css';
import 'primereact/resources/primereact.css';

// Create browser history to use in the Redux store
// const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
// const history = createBrowserHistory({ basename: baseUrl });

// Get the application-wide store instance, prepopulating with state from the server where available.
const initialState = window.initialReduxState;
const store = configureStore(history, initialState);
if (localStorage.jwtToken) {
    let data = { token: localStorage.jwtToken, refToken: localStorage.refreshToken };
    let user = jwt.decode(data.token);
    if (!Array.isArray(user.roles)) {
        user.roles = Array.of(user.roles);
    }
    //setAuthorizationToken(data.token);
    // store.dispatch(setCurrentUser(user));
    // store.dispatch(loginActions.setCurrentUser(user));
    loginActions.loginByJWT(data, store.dispatch);
}

ReactDOM.render(
    <Provider store={store}>
        <ConnectedRouter history={history}>
            <App />
        </ConnectedRouter >
    </Provider>,
    document.getElementById('root'));

registerServiceWorker(unregister);
