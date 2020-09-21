import axios from "axios";
import { serverUrl } from '../../../config';

export default class AllCoursesService {
    static GetAllCourses() {
        return axios.get(`${serverUrl}api/student/GetAllCourses`)
    };
    static SubscribeToCourse(model){
        return axios.post(`${serverUrl}api/student/SubscribeToCourse`,model)
    };
}