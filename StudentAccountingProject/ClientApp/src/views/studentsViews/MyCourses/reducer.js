import update from "../../../utils/update";
import MyCoursesService from "./MyCoursesService";

export const GET_COURSES_STARTED = "GET_COURSES_STARTED";
export const GET_COURSES_SUCCESS = "GET_COURSES_SUCCESS";
export const GET_COURSES_FAILED = "GET_COURSES_FAILED";

const initialState = {
    post: {
        loading: false,
        success: false,
        failed: false,
        errors: {}
    },
    courses: []
};


export function GetStudentsCourses() {
    return (dispatch) => {
        dispatch(getCoursesListActions.started());
        MyCoursesService.GetStudentsCourses()
            .then((response) => {
                console.log("response", response);
                dispatch(getCoursesListActions.success(response.data));
            }, err => { throw err; })
            .catch(err => {
                dispatch(getCoursesListActions.failed(err.response));
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
            payload: response
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

export const GetStudentsCoursesReducer = (state = initialState, action) => {
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
        default: {
            return newState;
        }
    }

    return newState;
};
