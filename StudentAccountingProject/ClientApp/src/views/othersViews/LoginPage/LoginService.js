import axios from "./node_modules/axios";
import { serverUrl } from '../../../config';

export default class LoginService {
    static login(model) {
        return axios.post(`${serverUrl}api/auth/login`, model)
    };
}