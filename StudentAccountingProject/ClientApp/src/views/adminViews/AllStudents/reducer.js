import update from "../../../utils/update";
import AllStudentsService from "./AllStudentsService";

export const GET_STUDENTS_STARTED = "GET_STUDENTS_STARTED";
export const GET_STUDENTS_SUCCESS = "GET_STUDENTS_SUCCESS";
export const GET_STUDENTS_FAILED = "GET_STUDENTS_FAILED";

export const CREATE_STUDENT_STARTED = "CREATE_STUDENT_STARTED";
export const CREATE_STUDENT_SUCCESS = "CREATE_STUDENT_SUCCESS";
export const CREATE_STUDENT_FAILED = "CREATE_STUDENT_FAILED";

export const EDIT_STUDENT_STARTED = "EDIT_STUDENT_STARTED";
export const EDIT_STUDENT_SUCCESS = "EDIT_STUDENT_SUCCESS";
export const EDIT_STUDENT_FAILED = "EDIT_STUDENT_FAILED";

export const DELETE_STUDENT_STARTED = "DELETE_STUDENT_STARTED";
export const DELETE_STUDENT_SUCCESS = "DELETE_STUDENT_SUCCESS";
export const DELETE_STUDENT_FAILED = "DELETE_STUDENT_FAILED";

export const CLEAR_STATE = "CLEAR_STATE";

const initialState = {
    getStudents: {
        loading: false,
        success: false,
        failed: false,
        errors: {},
        students: [],
    },
    createStudent: {
        loading: false,
        success: false,
        failed: false,
        error: '',
    },
    editStudent: {
        loading: false,
        success: false,
        failed: false,
        error: '',
    },
    deleteStudent:{
        loading: false,
        success: false,
        failed: false,
        error: '',
    }
};


export function GetAllStudents() {
    return (dispatch) => {
        dispatch(getAllStudentsListActions.started());
        AllStudentsService.GetAllStudents()
            .then((response) => {
                console.log("GetAllStudents", response);
                dispatch(getAllStudentsListActions.success(response.data));
            }, err => { throw err; })
            .catch(err => {
                dispatch(getAllStudentsListActions.failed(err));
            });
    }
}

export function CreateStudent(model) {
    return (dispatch) => {
        dispatch(createStudentListActions.started());
        AllStudentsService.CreateStudent(model)
            .then(() => {
                dispatch(createStudentListActions.success());
            }, err => { throw err; })
            .catch(err => {
                console.error("Create student error", err.response);
                dispatch(createStudentListActions.failed(err.response));
            });
    }
}

export function EditStudent(model) {
    return (dispatch) => {
        dispatch(editStudentListActions.started());
        AllStudentsService.EditStudent(model)
            .then(() => {
                dispatch(editStudentListActions.success());
            }, err => { throw err; })
            .catch(err => {
                console.error("Edit student error", err.response);
                dispatch(editStudentListActions.failed(err.response));
            });
    }
}

export function DeleteStudent(model) {
    return (dispatch) => {
        dispatch(deleteStudentListActions.started());
        AllStudentsService.DeleteStudent(model)
            .then(() => {
                dispatch(deleteStudentListActions.success());
            }, err => { throw err; })
            .catch(err => {
                console.error("Delete student error", err.response);
                dispatch(deleteStudentListActions.failed(err.response));
            });
    }
}

export function ClearState() {
    return (dispatch) => {
        dispatch(clearStateListActions.clear());
    }
}


export const getAllStudentsListActions = {
    started: () => {
        return {
            type: GET_STUDENTS_STARTED
        }
    },
    success: (response) => {
        return {
            type: GET_STUDENTS_SUCCESS,
            payload: response
        }
    },
    failed: (error) => {
        return {
            type: GET_STUDENTS_FAILED,
            errors: error
        }
    }
}

export const createStudentListActions = {
    started: () => {
        return {
            type: CREATE_STUDENT_STARTED
        }
    },
    success: () => {
        return {
            type: CREATE_STUDENT_SUCCESS
        }
    },
    failed: (error) => {
        return {
            type: CREATE_STUDENT_FAILED,
            errors: error
        }
    }
}

export const editStudentListActions = {
    started: () => {
        return {
            type: EDIT_STUDENT_STARTED
        }
    },
    success: () => {
        return {
            type: EDIT_STUDENT_SUCCESS
        }
    },
    failed: (error) => {
        return {
            type: EDIT_STUDENT_FAILED,
            errors: error
        }
    }
}

export const deleteStudentListActions = {
    started: () => {
        return {
            type: DELETE_STUDENT_STARTED
        }
    },
    success: () => {
        return {
            type: DELETE_STUDENT_SUCCESS
        }
    },
    failed: (error) => {
        return {
            type: DELETE_STUDENT_FAILED,
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

export const AllStudentsReducer = (state = initialState, action) => {
    let newState = state;
    switch (action.type) {
        //-------------------GET STUDENT------------------------
        case GET_STUDENTS_STARTED: {
            newState = update.set(state, "getStudents.loading", true);
            newState = update.set(newState, "getStudents.success", false);
            newState = update.set(newState, "getStudents.errors", {});
            newState = update.set(newState, "getStudents.failed", false);
            break;
        }
        case GET_STUDENTS_SUCCESS: {
            newState = update.set(state, "getStudents.loading", false);
            newState = update.set(newState, "getStudents.failed", false);
            newState = update.set(newState, "getStudents.success", true);
            newState = update.set(newState, "getStudents.students", action.payload);
            break;
        }
        case GET_STUDENTS_FAILED: {
            newState = update.set(state, "getStudents.loading", false);
            newState = update.set(newState, "getStudents.success", false);
            newState = update.set(newState, "getStudents.errors", action.errors);
            newState = update.set(newState, "getStudents.failed", true);
            break;
        }

        //-------------------CREATE STUDENT------------------------
        case CREATE_STUDENT_STARTED: {
            newState = update.set(state, "createStudent.loading", true);
            newState = update.set(newState, "createStudent.success", false);
            newState = update.set(newState, "createStudent.errors", {});
            newState = update.set(newState, "createStudent.failed", false);
            break;
        }
        case CREATE_STUDENT_SUCCESS: {
            newState = update.set(state, "createStudent.loading", false);
            newState = update.set(newState, "createStudent.failed", false);
            newState = update.set(newState, "createStudent.success", true);
            break;
        }
        case CREATE_STUDENT_FAILED: {
            newState = update.set(state, "createStudent.loading", false);
            newState = update.set(newState, "createStudent.success", false);
            newState = update.set(newState, "createStudent.error", action.errors.data.errorMessage);
            newState = update.set(newState, "createStudent.failed", true);
            break;
        }

        //---------------------EDIT STUDENT------------------------
        case EDIT_STUDENT_STARTED: {
            newState = update.set(state, "editStudent.loading", true);
            newState = update.set(newState, "editStudent.success", false);
            newState = update.set(newState, "editStudent.errors", {});
            newState = update.set(newState, "editStudent.failed", false);
            break;
        }
        case EDIT_STUDENT_SUCCESS: {
            newState = update.set(state, "editStudent.loading", false);
            newState = update.set(newState, "editStudent.failed", false);
            newState = update.set(newState, "editStudent.success", true);
            break;
        }
        case EDIT_STUDENT_FAILED: {
            newState = update.set(state, "editStudent.loading", false);
            newState = update.set(newState, "editStudent.success", false);
            newState = update.set(newState, "editStudent.error", action.errors.data.errorMessage);
            newState = update.set(newState, "editStudent.failed", true);
            break;
        }

        //---------------------DELETE STUDENT------------------------
        case DELETE_STUDENT_STARTED: {
            newState = update.set(state, "deleteStudent.loading", true);
            newState = update.set(newState, "deleteStudent.success", false);
            newState = update.set(newState, "deleteStudent.errors", {});
            newState = update.set(newState, "deleteStudent.failed", false);
            break;
        }
        case DELETE_STUDENT_SUCCESS: {
            newState = update.set(state, "deleteStudent.loading", false);
            newState = update.set(newState, "deleteStudent.failed", false);
            newState = update.set(newState, "deleteStudent.success", true);
            break;
        }
        case DELETE_STUDENT_FAILED: {
            newState = update.set(state, "deleteStudent.loading", false);
            newState = update.set(newState, "deleteStudent.success", false);
            newState = update.set(newState, "deleteStudent.error", action.errors.data.errorMessage);
            newState = update.set(newState, "deleteStudent.failed", true);
            break;
        }

 //---------------------CLEAR STATE ------------------------
        case CLEAR_STATE: {
            newState = update.set(state, "getStudents.success", false);
            newState = update.set(newState, "createStudent.success", false);
            newState = update.set(newState, "createStudent.failed", false);
            newState = update.set(newState, "editStudent.success", false);
            newState = update.set(newState, "editStudent.failed", false);
            newState = update.set(newState, "deleteStudent.success", false);
            newState = update.set(newState, "deleteStudent.failed", false);
        }

        default: {
            return newState;
        }
    }

    return newState;
};
