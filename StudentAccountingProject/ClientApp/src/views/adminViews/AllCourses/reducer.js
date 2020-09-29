import update from "../../../utils/update";
import AllCoursesService from "./AllCoursesService";

export const GET_COUNT_STARTED = "GET_COUNT_STARTED";
export const GET_COUNT_SUCCESS = "GET_COUNT_SUCCESS";
export const GET_COUNT_FAILED = "GET_COUNT_FAILED";

export const GET_AUTHORS_STARTED = "GET_AUTHORS_STARTED";
export const GET_AUTHORS_SUCCESS = "GET_AUTHORS_SUCCESS";
export const GET_AUTHORS_FAILED = "GET_AUTHORS_FAILED";

export const GET_COURSES_STARTED = "GET_COURSES_STARTED";
export const GET_COURSES_SUCCESS = "GET_COURSES_SUCCESS";
export const GET_COURSES_FAILED = "GET_COURSES_FAILED";

export const CREATE_COURSE_STARTED = "CREATE_COURSE_STARTED";
export const CREATE_COURSE_SUCCESS = "CREATE_COURSE_SUCCESS";
export const CREATE_COURSE_FAILED = "CREATE_COURSE_FAILED";

export const DELETE_COURSE_STARTED = "DELETE_COURSE_STARTED";
export const DELETE_COURSE_SUCCESS = "DELETE_COURSE_SUCCESS";
export const DELETE_COURSE_FAILED = "DELETE_COURSE_FAILED";

export const CLEAR_STATE = "CLEAR_STATE";

const initialState = {
    getCount: {
        loading: false,
        success: false,
        failed: false,
        error: '',
        count: 0,
    },
    getAuthors: {
        loading: false,
        success: false,
        failed: false,
        error: '',
        authors: [],
    },
    getCourses: {
        loading: false,
        success: false,
        failed: false,
        errors: {},
        сourses: [],
    },
    createCourse: {
        loading: false,
        success: false,
        failed: false,
        error: '',
    },
    deleteCourse: {
        loading: false,
        success: false,
        failed: false,
        error: '',
    }
};

export function GetCourseCount() {
    return (dispatch) => {
        dispatch(getCourseCountListActions.started());
        AllCoursesService.GetCourseCount()
            .then((response) => {
                dispatch(getCourseCountListActions.success(response.data));
            }, err => { throw err; })
            .catch(err => {
                dispatch(getCourseCountListActions.failed(err));
            });
    }
}

export function GetAuthors() {
    return (dispatch) => {
        dispatch(getAuthorsListActions.started());
        AllCoursesService.GetAuthors()
            .then((response) => {
                dispatch(getAuthorsListActions.success(response.data));
            }, err => { throw err; })
            .catch(err => {
                dispatch(getAuthorsListActions.failed(err));
            });
    }
}

export function GetAllCourses(model) {
    return (dispatch) => {
        dispatch(getAllCoursesListActions.started());
        AllCoursesService.GetCourses(model)
            .then((response) => {
                dispatch(getAllCoursesListActions.success(response.data));
            }, err => { throw err; })
            .catch(err => {
                console.error("Error", err);
                dispatch(getAllCoursesListActions.failed(err));
            });
    }
}

export function CreateCourse(model) {
    return (dispatch) => {
        dispatch(createCourseListActions.started());
        AllCoursesService.CreateCourse(model)
            .then(() => {
                dispatch(createCourseListActions.success());
            }, err => { throw err; })
            .catch(err => {
                console.error("Create Course error", err.response);
                dispatch(createCourseListActions.failed(err.response));
            });
    }
}

export function DeleteCourse(model) {
    return (dispatch) => {
        dispatch(deleteCourseListActions.started());
        AllCoursesService.DeleteCourse(model)
            .then(() => {
                dispatch(deleteCourseListActions.success());
            }, err => { throw err; })
            .catch(err => {
                console.error("Delete Course error", err.response);
                dispatch(deleteCourseListActions.failed(err.response));
            });
    }
}

export function ClearState() {
    return (dispatch) => {
        dispatch(clearStateListActions.clear());
    }
}



export const getCourseCountListActions = {
    started: () => {
        return {
            type: GET_COUNT_STARTED
        }
    },
    success: (response) => {
        return {
            type: GET_COUNT_SUCCESS,
            payload: response
        }
    },
    failed: (error) => {
        return {
            type: GET_COUNT_FAILED,
            errors: error
        }
    }
}

export const getAuthorsListActions = {
    started: () => {
        return {
            type: GET_AUTHORS_STARTED
        }
    },
    success: (response) => {
        return {
            type: GET_AUTHORS_SUCCESS,
            payload: response
        }
    },
    failed: (error) => {
        return {
            type: GET_AUTHORS_FAILED,
            errors: error
        }
    }
}

export const getAllCoursesListActions = {
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
        return {
            type: GET_COURSES_FAILED,
            errors: error.errorMessage
        }
    }
}

export const createCourseListActions = {
    started: () => {
        return {
            type: CREATE_COURSE_STARTED
        }
    },
    success: () => {
        return {
            type: CREATE_COURSE_SUCCESS
        }
    },
    failed: (error) => {
        return {
            type: CREATE_COURSE_FAILED,
            errors: error
        }
    }
}

export const deleteCourseListActions = {
    started: () => {
        return {
            type: DELETE_COURSE_STARTED
        }
    },
    success: () => {
        return {
            type: DELETE_COURSE_SUCCESS
        }
    },
    failed: (error) => {
        return {
            type: DELETE_COURSE_FAILED,
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

export const AllCoursesReducer = (state = initialState, action) => {
    let newState = state;
    switch (action.type) {
        //-------------------GET Course COUNT------------------
        case GET_COUNT_STARTED: {
            newState = update.set(state, "getCount.loading", true);
            newState = update.set(newState, "getCount.success", false);
            newState = update.set(newState, "getCount.errorMessage", '');
            newState = update.set(newState, "getCount.failed", false);
            break;
        }
        case GET_COUNT_SUCCESS: {
            newState = update.set(state, "getCount.loading", false);
            newState = update.set(newState, "getCount.failed", false);
            newState = update.set(newState, "getCount.success", true);
            newState = update.set(newState, "getCount.count", action.payload);
            break;
        }
        case GET_COUNT_FAILED: {
            newState = update.set(state, "getCount.loading", false);
            newState = update.set(newState, "getCount.success", false);
            newState = update.set(newState, "getCount.errorMessage", action.errors);
            newState = update.set(newState, "getCount.failed", true);
            break;
        }

        //-------------------GET Authors------------------
        case GET_AUTHORS_STARTED: {
            newState = update.set(state, "getAuthors.loading", true);
            newState = update.set(newState, "getAuthors.success", false);
            newState = update.set(newState, "getAuthors.errorMessage", '');
            newState = update.set(newState, "getAuthors.failed", false);
            break;
        }
        case GET_AUTHORS_SUCCESS: {
            newState = update.set(state, "getAuthors.loading", false);
            newState = update.set(newState, "getAuthors.failed", false);
            newState = update.set(newState, "getAuthors.success", true);
            newState = update.set(newState, "getAuthors.authors", action.payload);
            break;
        }
        case GET_AUTHORS_FAILED: {
            newState = update.set(state, "getAuthors.loading", false);
            newState = update.set(newState, "getAuthors.success", false);
            newState = update.set(newState, "getAuthors.errorMessage", action.errors);
            newState = update.set(newState, "getAuthors.failed", true);
            break;
        }

        //-------------------GET Course------------------------
        case GET_COURSES_STARTED: {
            newState = update.set(state, "getCourses.loading", true);
            newState = update.set(newState, "getCourses.success", false);
            newState = update.set(newState, "getCourses.errorMessage", '');
            newState = update.set(newState, "getCourses.failed", false);
            break;
        }
        case GET_COURSES_SUCCESS: {
            newState = update.set(state, "getCourses.loading", false);
            newState = update.set(newState, "getCourses.failed", false);
            newState = update.set(newState, "getCourses.success", true);
            newState = update.set(newState, "getCourses.Courses", action.payload.courses);
            break;
        }
        case GET_COURSES_FAILED: {
            newState = update.set(state, "getCourses.loading", false);
            newState = update.set(newState, "getCourses.success", false);
            newState = update.set(newState, "getCourses.errorMessage", action.errors);
            newState = update.set(newState, "getCourses.failed", true);
            break;
        }

        //-------------------CREATE Course------------------------
        case CREATE_COURSE_STARTED: {
            newState = update.set(state, "createCourse.loading", true);
            newState = update.set(newState, "createCourse.success", false);
            newState = update.set(newState, "createCourse.errors", {});
            newState = update.set(newState, "createCourse.failed", false);
            break;
        }
        case CREATE_COURSE_SUCCESS: {
            newState = update.set(state, "createCourse.loading", false);
            newState = update.set(newState, "createCourse.failed", false);
            newState = update.set(newState, "createCourse.success", true);
            break;
        }
        case CREATE_COURSE_FAILED: {
            newState = update.set(state, "createCourse.loading", false);
            newState = update.set(newState, "createCourse.success", false);
            newState = update.set(newState, "createCourse.error", action.errors.data.errorMessage);
            newState = update.set(newState, "createCourse.failed", true);
            break;
        }

        //---------------------DELETE Course------------------------
        case DELETE_COURSE_STARTED: {
            newState = update.set(state, "deleteCourse.loading", true);
            newState = update.set(newState, "deleteCourse.success", false);
            newState = update.set(newState, "deleteCourse.errors", {});
            newState = update.set(newState, "deleteCourse.failed", false);
            break;
        }
        case DELETE_COURSE_SUCCESS: {
            newState = update.set(state, "deleteCourse.loading", false);
            newState = update.set(newState, "deleteCourse.failed", false);
            newState = update.set(newState, "deleteCourse.success", true);
            break;
        }
        case DELETE_COURSE_FAILED: {
            newState = update.set(state, "deleteCourse.loading", false);
            newState = update.set(newState, "deleteCourse.success", false);
            newState = update.set(newState, "deleteCourse.error", action.errors.data.errorMessage);
            newState = update.set(newState, "deleteCourse.failed", true);
            break;
        }

        //---------------------CLEAR STATE ------------------------
        case CLEAR_STATE: {
            newState = update.set(state, "getCourses.success", false);
            newState = update.set(newState, "getAuthors.success", false);
            newState = update.set(newState, "createCourse.success", false);
            newState = update.set(newState, "createCourse.failed", false);
            newState = update.set(newState, "deleteCourse.success", false);
            newState = update.set(newState, "deleteCourse.failed", false);
        }

        default: {
            return newState;
        }
    }

    return newState;
};
