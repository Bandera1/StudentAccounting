import axios from "axios";
import { serverUrl } from '../../../config';

export default class AllStudentsService {
    static GetStudentsCount() {
        return axios.get(`${serverUrl}api/studentControl/GetStudentsCount`)
    };
    static GetAllStudents(from,to) {
        return axios.get(`${serverUrl}api/studentControl/GetAllStudents/${from}/${to}`)
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