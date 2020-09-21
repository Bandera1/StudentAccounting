import update from "../../../utils/update";
import AllCoursesService from "./AllCoursesService";

export const GET_COURSES_STARTED = "GET_COURSES_STARTED";
export const GET_COURSES_SUCCESS = "GET_COURSES_SUCCESS";
export const GET_COURSES_FAILED = "GET_COURSES_FAILED";

export const SUBSCRIBE_TO_COURSE_STARTED = "SUBSCRIBE_TO_COURSE_STARTED";
export const SUBSCRIBE_TO_COURSE_SUCCESS = "SUBSCRIBE_TO_COURSE_SUCCESS";
export const SUBSCRIBE_TO_COURSE_FAILED = "SUBSCRIBE_TO_COURSE_FAILED";

const initialState = {
    post: {
        loading: false,
        success: false,
        failed: false,
        errors: {}
    },
    subscribePost: {
        loading: false,
        success: false,
        failed: false,
        error: ""
    },
    courses:[]
};


export function GetAllCourses() {
    return (dispatch) => {
        dispatch(getCoursesListActions.started());
        AllCoursesService.GetAllCourses()
            .then((response) => {
                console.log("response", response);
                dispatch(getCoursesListActions.success(response.data));
            }, err => { throw err; })
            .catch(err => {
                dispatch(getCoursesListActions.failed(err.response));
            });
    }
}

export function SubscribeToCourse(model) {
    return (dispatch) => {
        dispatch(subscribeToCourseListAction.started());
        AllCoursesService.SubscribeToCourse(model)
            .then((response) => {
                console.log("response", response);
                dispatch(subscribeToCourseListAction.success(response.data));
            }, err => { throw err; })
            .catch(err => {
                dispatch(subscribeToCourseListAction.failed(err.response));
            });
    }
}


export const getCoursesListActions = {
    started: () => {
        return {
            type: GET_COURSES_STARTED
        }
    },
    success: (response) => {
        return {
            type: GET_COURSES_SUCCESS,
            payload:response
        }
    },
    failed: (error) => {
        console.error("Error : ", error)
        return {
            type: GET_COURSES_FAILED,
            errors: error
        }
    }
}

export const subscribeToCourseListAction = {
    started: () => {
        return {
            type: SUBSCRIBE_TO_COURSE_STARTED
        }
    },
    success: (response) => {
        return {
            type: SUBSCRIBE_TO_COURSE_SUCCESS,
            payload: response
        }
    },
    failed: (error) => {
        console.error("Error : ", error)
        return {
            type: SUBSCRIBE_TO_COURSE_FAILED,
            errors: error
        }
    }
}


export const GetAllCoursesReducer = (state = initialState, action) => {
    let newState = state;
    switch (action.type) {
        case GET_COURSES_STARTED: {
            newState = update.set(state, "post.loading", true);
            newState = update.set(newState, "post.success", false);
            newState = update.set(newState, "post.errors", {});
            newState = update.set(newState, "post.failed", false);
            break;
        }
        case GET_COURSES_SUCCESS: {
            newState = update.set(state, "post.loading", false);
            newState = update.set(newState, "post.failed", false);
            newState = update.set(newState, "courses", action.payload);
            newState = update.set(newState, "post.success", true);
            break;
        }
        case GET_COURSES_FAILED: {
            newState = update.set(state, "post.loading", false);
            newState = update.set(newState, "post.success", false);
            newState = update.set(newState, "post.errors", action.errors);
            newState = update.set(newState, "post.failed", true);
            break;
        }
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        case SUBSCRIBE_TO_COURSE_STARTED: {
            newState = update.set(state, "subscribePost.loading", true);
            newState = update.set(newState, "subscribePost.success", false);
            newState = update.set(newState, "subscribePost.error", "");
            newState = update.set(newState, "subscribePost.failed", false);
            break;
        }
        case SUBSCRIBE_TO_COURSE_SUCCESS: {
            newState = update.set(state, "subscribePost.loading", false);
            newState = update.set(newState, "subscribePost.failed", false);
            newState = update.set(newState, "subscribePost.success", true);
            break;
        }
        case SUBSCRIBE_TO_COURSE_FAILED: {
            console.error("Subscribe error", action.errors);
            newState = update.set(state, "subscribePost.loading", false);
            newState = update.set(newState, "subscribePost.success", false);
            newState = update.set(newState, "subscribePost.error", action.errors.data.errorMessage);
            newState = update.set(newState, "subscribePost.failed", true);
            break;
        }
        default: {
            return newState;
        }
    }

    return newState;
};
