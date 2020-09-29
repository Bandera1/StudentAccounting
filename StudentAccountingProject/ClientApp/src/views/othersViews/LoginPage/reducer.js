import update from "../../../utils/update";
import LoginService from "./LoginService";
import isEmpty from "lodash/isEmpty";
import setAuthorizationToken from "../../../utils/setAuthorizationToken";
import jwt from "jsonwebtoken";
import redirectStatusCode from "../../../utils/redirectStatusCode";
import history from "../../../utils/history";
import { push } from "connected-react-router";

export const LOGIN_POST_STARTED = "LOGIN_POST_STARTED";
export const LOGIN_POST_SUCCESS = "LOGIN_POST_SUCCESS";
export const LOGIN_POST_FAILED = "LOGIN_POST_FAILED";
export const LOGIN_SET_CURRENT_USER = "login/SET_CURRENT_USER";

export const LOGIN_LOGOUT = "LOGIN_LOGOUT";


export const initialState = {
    post: {
        loading: false,
        success: false,
        failed: false,
        error: ""
    },
    isAuthenticated: false,
    user: {
        id: "",
        name: "",
        // image: "",
        roles: []
    }
};

export const loginActions = {
    started: () => {
        return {
            type: LOGIN_POST_STARTED
        };
    },

    success: () => {
        return {
            type: LOGIN_POST_SUCCESS
        };
    },

    failed: response => {
        console.error("Login error", response.data)
        return {
            type: LOGIN_POST_FAILED,
            errors: response.data
        };
    },

    setCurrentUser: user => {
        return {
            type: LOGIN_SET_CURRENT_USER,
            user
        };
    }
};

export const logoutActions = {
    logout: () => {
        return {
            type: LOGIN_LOGOUT
        }
    }
}

export const loginReducer = (state = initialState, action) => {
    let newState = state;

    switch (action.type) {
        case LOGIN_POST_STARTED: {
            newState = update.set(state, "post.loading", true);
            newState = update.set(newState, "post.success", false);
            newState = update.set(newState, "post.errors", {});
            newState = update.set(newState, "post.failed", false);
            break;
        }
        case LOGIN_SET_CURRENT_USER: {
            newState = update.set(state, "user.id", action.user.id);
            newState = update.set(newState, "user.name", action.user.name);
            newState = update.set(newState, "user.roles", action.user.roles);
            newState = update.set(newState, "isAuthenticated", !isEmpty(action.user));
            break;
        }
        case LOGIN_POST_SUCCESS: {
            newState = update.set(state, "post.loading", true);
            newState = update.set(newState, "post.failed", false);
            newState = update.set(newState, "post.errors", {});
            newState = update.set(newState, "post.success", true);
            break;
        }
        case LOGIN_POST_FAILED: {
            newState = update.set(state, "post.loading", false);
            newState = update.set(newState, "post.success", false);
            newState = update.set(newState, "post.error", action.errors.errorMessage);
            newState = update.set(newState, "post.failed", true);
            break;
        }

        case LOGIN_LOGOUT: {
            newState = update.set(state, "user.id", undefined);
            newState = update.set(newState, "user.name", undefined);
            newState = update.set(newState, "user.roles", undefined);
            newState = update.set(newState, "isAuthenticated", false);
        }

        default: {
            return newState;
        }
    }

    return newState;
};

export const login = model => {
    return dispatch => {
        dispatch(loginActions.started());
        LoginService.login(model)
            .then(
                response => {
                    console.log("Response",response.data);
                    dispatch(loginActions.success());
                    loginAndRedirect(response.data,dispatch);
                },
                err => {
                    throw err;
                }
            )
            .catch(err => {
                console.error("Login error - ", err);
                dispatch(loginActions.failed(err.response));
                redirectStatusCode(err.response);
            });
    };
};

export const facebookLogin = model => {
    console.log("FACEBOOK LOGIN!!!", model);
    return dispatch => {
        dispatch(loginActions.started());
        LoginService.facebookLogin(model)
            .then(
                response => {
                    console.log("Response", response.data);
                    dispatch(loginActions.success());
                    loginAndRedirect(response.data, dispatch);
                },
                err => {
                    throw err;
                }
            )
            .catch(err => {
                console.error("Login error - ", err);
                dispatch(loginActions.failed(err.response));
                redirectStatusCode(err.response);
            });
    };
};

export const loginAndRedirect = (token,dispatch) =>
{
    loginByJWT(token, dispatch);
    const pushUrl = getUrlToRedirect();
    dispatch(push(pushUrl));
    dispatch(history.go());
    
};

export function getUrlToRedirect() {
    var user = jwt.decode(localStorage.jwtToken);

    if (user == null) {
        return "/";
    }

    let roles = user.roles;
    let path = "";
    if (Array.isArray(roles)) {
        for (let i = 0; i < roles.length; i++) {
            if (roles[i] == "Student") {
                path = "/student/schedule";
                break;
            } else if (roles[i] === "Admin") {
                path = "/admin/students";
                break;
            }
        }
    } else {
        if (roles == "Student") {
            path = "/student/schedule";
        } else if (roles === "Admin") {
            path = "/admin/students";
        }
    }
    console.log("getUrlToRedirect", path);

    return path;
}

export const loginByJWT = (tokens, dispatch) => {
    const { token, refToken } = tokens;
    let myToken = token === undefined ? tokens.data : token;

    console.log('Hello app Token: ', tokens);
    var user = jwt.decode(myToken);

    console.log("token -", jwt.decode(myToken));
    if (!Array.isArray(user.roles)) {
        user.roles = Array.of(user.roles);
    }

    localStorage.setItem("jwtToken", myToken);
    setAuthorizationToken(myToken);
    dispatch(loginActions.setCurrentUser(user));
};

export function logout() {
    return dispatch => {
        logoutByJWT(dispatch);
    };
}

export const logoutByJWT = dispatch => {
    localStorage.removeItem("jwtToken");

    setAuthorizationToken(false);
    dispatch(logoutActions.logout());
    // dispatch(loginActions.setCurrentUser({}));
};
