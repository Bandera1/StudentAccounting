import update from "../../../utils/update";
import ConfirmEmailService from "./ConfirmEmailService";

export const CONFIRM_EMAIL_STARTED = "CONFIRM_EMAIL_STARTED";
export const CONFIRM_EMAIL_SUCCESS = "CONFIRM_EMAIL_SUCCESS";
export const CONFIRM_EMAIL_FAILED = "CONFIRM_EMAIL_FAILED";

const initialState = {
    post: {
        loading: false,
        success: false,
        failed: false,
        errors: {}
    },
};


export function ConfirmEmail(model) {
    return (dispatch) => {
        dispatch(confirmEmailListActions.started());
        ConfirmEmailService.ConfirmEmail(model)
            .then((response) => {
                console.log("response", response);
                dispatch(confirmEmailListActions.success(response.data));
            }, err => { throw err; })
            .catch(err => {
                dispatch(confirmEmailListActions.failed(err.response));
            });
    }
}


export const confirmEmailListActions = {
    started: () => {
        return {
            type: CONFIRM_EMAIL_STARTED
        }
    },
    success: (response) => {
        return {
            type: CONFIRM_EMAIL_SUCCESS,
            payload: response
        }
    },
    failed: (error) => {
        return {
            type: CONFIRM_EMAIL_FAILED,
            errors: error
        }
    }
}

export const ConfirmEmailReducer = (state = initialState, action) => {
    let newState = state;
    switch (action.type) {
        case CONFIRM_EMAIL_STARTED: {
            newState = update.set(state, "post.loading", true);
            newState = update.set(newState, "post.success", false);
            newState = update.set(newState, "post.errors", {});
            newState = update.set(newState, "post.failed", false);
            break;
        }
        case CONFIRM_EMAIL_SUCCESS: {
            newState = update.set(state, "post.loading", false);
            newState = update.set(newState, "post.failed", false);
            newState = update.set(newState, "post.success", true);
            break;
        }
        case CONFIRM_EMAIL_FAILED: {
            newState = update.set(state, "post.loading", false);
            newState = update.set(newState, "post.success", false);
            newState = update.set(newState, "post.errors", action.errors);
            newState = update.set(newState, "post.failed", true);
            break;
        }
        default: {
            return newState;
        }
    }

    return newState;
};
