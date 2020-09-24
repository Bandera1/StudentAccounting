import axios from "axios";
import { serverUrl } from '../../../config';

export default class AllStudentsService {
    static GetStudentsCount() {
        return axios.get(`${serverUrl}api/studentControl/GetStudentsCount`)
    };
    static GetAllStudents(model) {
        return axios.post(`${serverUrl}api/studentControl/GetAllStudents`,model)
    };
    static CreateStudent(model) {
        return axios.post(`${serverUrl}api/studentControl/CreateStudent`,model)
    };
    static EditStudent(model) {
        return axios.post(`${serverUrl}api/studentControl/EditStudent`, model)
    };
    static DeleteStudent(model) {
        return axios.post(`${serverUrl}api/studentControl/DeleteStudent`, model)
    };
}