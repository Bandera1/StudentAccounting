import { applyMiddleware, combineReducers, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import { connectRouter, routerMiddleware } from 'connected-react-router';
// import { createBrowserHistory } from 'history';
import createHistory from 'history/createHashHistory';


//reducers
import { loginReducer } from '../views/othersViews/LoginPage/reducer';
import { registerReducer } from '../views/othersViews/RegisterPage/reducer';
import { GetAllCoursesReducer } from '../views/studentsViews/AllCorses/reducer';
import { GetStudentsCoursesReducer } from '../views/studentsViews/MyCourses/reducer';
import { ConfirmEmailReducer } from '../views/othersViews/ConfirmEmail/reducer';
import { AllStudentsReducer } from '../views/adminViews/AllStudents/reducer';
import { AllCoursesReducer } from '../views/adminViews/AllCourses/reducer';




// Create browser history to use in the Redux store
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
export const history = createHistory({ basename: baseUrl });

export default function configureStore(history, initialState) {
    const reducers = {
        login: loginReducer,  
        register: registerReducer,
        getAllCourses: GetAllCoursesReducer,
        studentsCourses: GetStudentsCoursesReducer,
        confirmEmail: ConfirmEmailReducer,
        AllStudents: AllStudentsReducer,
        AllCourses: AllCoursesReducer,
    };

    const middleware = [
        thunk,
        routerMiddleware(history)
    ];

    // In development, use the browser's Redux dev tools extension if installed
    const enhancers = [];
    const isDevelopment = process.env.NODE_ENV === 'development';
    if (isDevelopment && typeof window !== 'undefined' && window.devToolsExtension) {
        enhancers.push(window.devToolsExtension());
    }



    const rootReducer = combineReducers({
        ...reducers,
        router: connectRouter(history)
    });

    return createStore(
        rootReducer,
        initialState,
        compose(applyMiddleware(...middleware), ...enhancers)
    );
}