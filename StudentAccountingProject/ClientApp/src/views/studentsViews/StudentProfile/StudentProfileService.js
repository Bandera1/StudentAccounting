import axios from "axios";
import { serverUrl } from '../../../config';

export default class StudentProfileService {
    static GetStudentInfo() {
        return axios.get(`${serverUrl}api/StudentProfile/GetStudentInfo`)
    };
    static UpdateStudent(model) {
        return axios.post(`${serverUrl}api/StudentProfile/UpdateStudent`,model)
    };
    static UpdatePassword(model) {
        return axios.post(`${serverUrl}api/StudentProfile/UpdatePassword`, model)
    };
    static UpdateImage(model) {
        return axios.post(`${serverUrl}api/Account/changeImage`, model)
    };
}