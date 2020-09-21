import axios from "axios";
import { serverUrl } from '../../../config';

export default class MyCoursesService {
    static GetStudentsCourses() {
        return axios.get(`${serverUrl}api/student/GetMyCourses`)
    };
}