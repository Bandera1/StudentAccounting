import axios from "axios";
import { serverUrl } from '../../../config';

export default class AllCoursesService {
    static GetCourseCount() {
        return axios.get(`${serverUrl}api/courseControl/GetCoursesCount`)
    };
    static GetAuthors() {
        return axios.get(`${serverUrl}api/courseControl/GetAuthors`)
    }
    static GetCourses(model) {
        return axios.post(`${serverUrl}api/courseControl/GetCourses`, model)
    };
    static CreateCourse(model) {
        return axios.post(`${serverUrl}api/courseControl/CreateCourse`, model)
    };
    static DeleteCourse(model) {
        return axios.post(`${serverUrl}api/courseControl/DeleteCourse`, model)
    };
}