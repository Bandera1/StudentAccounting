import update from "../../../utils/update";
import StudentProfileService from "./StudentProfileService";

export const GET_INFO_STARTED = "GET_INFO_STARTED";
export const GET_INFO_SUCCESS = "GET_INFO_SUCCESS";
export const GET_INFO_FAILED = "GET_INFO_FAILED";

export const UPDATE_STUDENT_STARTED = "UPDATE_STUDENT_STARTED";
export const UPDATE_STUDENT_SUCCESS = "UPDATE_STUDENT_SUCCESS";
export const UPDATE_STUDENT_FAILED = "UPDATE_STUDENT_FAILED";

export const UPDATE_PASSWORD_STARTED = "UPDATE_PASSWORD_STARTED";
export const UPDATE_PASSWORD_SUCCESS = "UPDATE_PASSWORD_SUCCESS";
export const UPDATE_PASSWORD_FAILED = "UPDATE_PASSWORD_FAILED";

export const UPDATE_IMAGE_STARTED = "UPDATE_IMAGE_STARTED";
export const UPDATE_IMAGE_SUCCESS = "UPDATE_IMAGE_SUCCESS";
export const UPDATE_IMAGE_FAILED = "UPDATE_IMAGE_FAILED";

export const CLEAR_STATE = "CLEAR_STATE";

const initialState = {
    getInfo: {
        loading: false,
        success: false,
        failed: false,
        info: {},
        error: '' 
    },
    updateStudent: {
        loading: false,
        success: false,
        failed: false,
        error: '' 
    },
    updatePassword: {
        loading: false,
        success: false,
        failed: false,
        error: ''
    },
    updateImage: {
        loading: false,
        success: false,
        failed: false,
        error: ''
    }
};


export function GetStudentInfo() {
    return (dispatch) => {
        dispatch(getStudenInfoListAcions.started());
        StudentProfileService.GetStudentInfo()
            .then((response) => {
                console.log("response", response);
                dispatch(getStudenInfoListAcions.success(response.data));
            }, err => { throw err; })
            .catch(err => {
                dispatch(getStudenInfoListAcions.failed(err.response));
            });
    }
}

export function UpdateStudent(model) {
    return (dispatch) => {
        dispatch(updateStudenListAcions.started());
        StudentProfileService.UpdateStudent(model)
            .then((response) => {
                console.log("response", response);
                dispatch(updateStudenListAcions.success(response.data));
            }, err => { throw err; })
            .catch(err => {
                dispatch(updateStudenListAcions.failed(err.response));
            });
    }
}

export function UpdatePassword(model) {
    return (dispatch) => {
        dispatch(updatePasswordListAcions.started());
        StudentProfileService.UpdatePassword(model)
            .then((response) => {
                console.log("response", response);
                dispatch(updatePasswordListAcions.success(response.data));
            }, err => { throw err; })
            .catch(err => {
                dispatch(updatePasswordListAcions.failed(err.response));
            });
    }
}

export function UpdateImage(model) {
    return (dispatch) => {
        dispatch(updateImageListAcions.started());
        StudentProfileService.UpdateImage(model)
            .then((response) => {
                console.log("response", response);
                dispatch(updateImageListAcions.success(response.data));
            }, err => { throw err; })
            .catch(err => {
                dispatch(updateImageListAcions.failed(err.response));
            });
    }
}

export function ClearState()
{
    return (dispatch) => {
        dispatch(clearStateListActions.clear());
    }
}

export const getStudenInfoListAcions = {
    started: () => {
        return {
            type: GET_INFO_STARTED
        }
    },
    success: (response) => {
        return {
            type: GET_INFO_SUCCESS,
            payload: response
        }
    },
    failed: (error) => {
        console.error("Error : ", error)
        return {
            type: GET_INFO_FAILED,
            errors: error
        }
    }
}

export const updateStudenListAcions = {
    started: () => {
        return {
            type: UPDATE_STUDENT_STARTED
        }
    },
    success: (response) => {
        return {
            type: UPDATE_STUDENT_SUCCESS,
            payload: response
        }
    },
    failed: (error) => {
        console.error("Error : ", error)
        return {
            type: UPDATE_STUDENT_FAILED,
            errors: error
        }
    }
}

export const updatePasswordListAcions = {
    started: () => {
        return {
            type: UPDATE_PASSWORD_STARTED
        }
    },
    success: (response) => {
        return {
            type: UPDATE_PASSWORD_SUCCESS,
            payload: response
        }
    },
    failed: (error) => {
        console.error("Error : ", error)
        return {
            type: UPDATE_PASSWORD_FAILED,
            errors: error
        }
    }
}

export const updateImageListAcions = {
    started: () => {
        return {
            type: UPDATE_IMAGE_STARTED
        }
    },
    success: (response) => {
        return {
            type: UPDATE_IMAGE_SUCCESS,
            payload: response
        }
    },
    failed: (error) => {
        console.error("Error : ", error)
        return {
            type: UPDATE_IMAGE_FAILED,
            errors: error
        }
    }
}

export const clearStateListActions = {
    clear: () => {
        return {
            type: CLEAR_STATE
        }
    }
}

export const StudentProfileReducer = (state = initialState, action) => {
    let newState = state;
    switch (action.type) {
        case GET_INFO_STARTED: {
            newState = update.set(state, "getInfo.loading", true);
            newState = update.set(newState, "getInfo.success", false);
            newState = update.set(newState, "getInfo.info", {});
            newState = update.set(newState, "getInfo.error", '');
            newState = update.set(newState, "getInfo.failed", false);
            break;
        }
        case GET_INFO_SUCCESS: {
            newState = update.set(state, "getInfo.loading", false);
            newState = update.set(newState, "getInfo.failed", false);
            newState = update.set(newState, "getInfo.info", action.payload);
            newState = update.set(newState, "getInfo.success", true);
            break;
        }
        case GET_INFO_FAILED: {
            newState = update.set(state, "getInfo.loading", false);
            newState = update.set(newState, "getInfo.success", false);
            newState = update.set(newState, "getInfo.error", action.errors.data.errorMessage);
            newState = update.set(newState, "getInfo.failed", true);
            break;
        }

        //------------UPDATE STUDENT--------------
        case UPDATE_STUDENT_STARTED: {
            newState = update.set(state, "updateStudent.loading", true);
            newState = update.set(newState, "updateStudent.success", false);
            newState = update.set(newState, "updateStudent.error", '');
            newState = update.set(newState, "updateStudent.failed", false);
            break;
        }
        case UPDATE_STUDENT_SUCCESS: {
            newState = update.set(state, "updateStudent.loading", false);
            newState = update.set(newState, "updateStudent.failed", false);
            newState = update.set(newState, "updateStudent.success", true);
            newState = update.set(newState, "updateStudent.error", '');
            break;
        }
        case UPDATE_STUDENT_FAILED: {
            newState = update.set(state, "updateStudent.loading", false);
            newState = update.set(newState, "updateStudent.success", false);
            newState = update.set(newState, "updateStudent.error", action.errors.data.errorMessage);
            newState = update.set(newState, "updateStudent.failed", true);
            break;
        }

        //------------UPDATE PASSWORD--------------
        case UPDATE_PASSWORD_STARTED: {
            newState = update.set(state, "updatePassword.loading", true);
            newState = update.set(newState, "updatePassword.success", false);
            newState = update.set(newState, "updatePassword.error", '');
            newState = update.set(newState, "updatePassword.failed", false);
            break;
        }
        case UPDATE_PASSWORD_SUCCESS: {
            newState = update.set(state, "updatePassword.loading", false);
            newState = update.set(newState, "updatePassword.failed", false);
            newState = update.set(newState, "updatePassword.success", true);
            newState = update.set(newState, "updatePassword.error", '');
            break;
        }
        case UPDATE_PASSWORD_FAILED: {
            newState = update.set(state, "updatePassword.loading", false);
            newState = update.set(newState, "updatePassword.success", false);
            newState = update.set(newState, "updatePassword.error", action.errors.errorMessage);
            newState = update.set(newState, "updatePassword.failed", true);
            break;
        }

        //------------UPDATE IMAGE--------------
        // case UPDATE_IMAGE_STARTED: {
        //     newState = update.set(state, "updateImage.loading", true);
        //     newState = update.set(newState, "updateImage.success", false);
        //     newState = update.set(newState, "updateImage.error", '');
        //     newState = update.set(newState, "updateImage.failed", false);
        //     break;
        // }
        // case UPDATE_IMAGE_SUCCESS: {
        //     newState = update.set(state, "updateImage.loading", false);
        //     newState = update.set(newState, "updateImage.failed", false);
        //     newState = update.set(newState, "updateImage.success", true);
        //     newState = update.set(newState, "updateImage.error", '');
        //     break;
        // }
        // case UPDATE_IMAGE_FAILED: {
        //     newState = update.set(state, "updateImage.loading", false);
        //     newState = update.set(newState, "updateImage.success", false);
        //     newState = update.set(newState, "updateImage.error", action.errors.errorMessage);
        //     newState = update.set(newState, "updateImage.failed", true);
        //     break;
        // }

        case CLEAR_STATE: {
            newState = update.set(state, "updateImage.success", false);
            newState = update.set(newState, "updateImage.failed", false);
            newState = update.set(newState, "updateStudent.failed", false);
            newState = update.set(newState, "updateStudent.success", false);
        }

        default: {
            return newState;
        }
    }

    return newState;
};
